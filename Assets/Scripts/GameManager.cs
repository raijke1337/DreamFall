using Game.Extras;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private GameMode _gameMode;

        [SerializeField] private PlayerComponent _playerFPrefab;
        [SerializeField] private PlayerComponent _playerMPrefab;

        private PlayerComponent _player;
        private CameraController _camCtrl;
        private GameObserver _gameObserver;
        private UIController _ui;
        private TunnelController _tunnel;

        private bool IsPause = false;

        private Dictionary<string,LevelData> _levelDatas;
        private Dictionary<string,LevelRules> _levelRules;
        private Dictionary<string,PlayerMovementData> _moveDatas;

        private string _currLevelID;
        private string _currRulesID;
        private string _currMoveID;

        private bool UpdatingAim = false;

        private void Update()
        {
            if (_gameMode== GameMode.Game)
            {
                if (_player == null || _gameObserver == null || IsPause) return;
                _player.OnUpdateInGame(Time.deltaTime);
                UpdateUI();
                bool timeUp = _gameObserver.DeltaUpdate(Time.deltaTime);
                if (timeUp)
                {
                    OnLevelComplete();
                }
            }
        }

        private async void OnEnable()
        {
            await Task.Delay(10);
            GrabConfigs();
            SceneManager.sceneLoaded += OnChangeScene;
        }
        private void GrabConfigs()
        {

            _levelDatas = new Dictionary<string, LevelData>();
            _levelRules = new Dictionary<string, LevelRules>();
            _moveDatas = new Dictionary<string, PlayerMovementData>();

            var datas = ScriptableObjectStorage.Instance.GetConfigsOfType<LevelDataSO>();
            foreach (var d in datas)
            {
                _levelDatas[d.LevelID] = new LevelData(d);
            }
            var datas2 = ScriptableObjectStorage.Instance.GetConfigsOfType<PlayerMovementDataSO>();
            foreach (var d in datas2)
            {
                _moveDatas[d.MoveDataID] = new PlayerMovementData(d);
            }
            var datas3 = ScriptableObjectStorage.Instance.GetConfigsOfType<LevelRulesSO>();
            foreach (var d in datas3)
            {
                _levelRules[d.RulesID] = new LevelRules(d);
            }
        }

        public void StartLevel(string levelID, string rulesID, string moveID)
        {
            _currLevelID = levelID;
            _currRulesID = rulesID;
            _currMoveID = moveID;

            SceneManager.LoadScene(_levelDatas[_currLevelID].SceneIndex);
            AudioManager.Instance.PlayMusic(_levelDatas[_currLevelID].MusicID);
        }
        private void OnChangeScene(Scene sc, LoadSceneMode mode)
        {
            if (sc.buildIndex == 0) _gameMode = GameMode.Menu;
            else _gameMode = GameMode.Game;
            _ui = FindObjectOfType<UIController>(); 
            Assert.IsNotNull(_ui);
            AfterSceneLoaded();
        }
        private async void AfterSceneLoaded()
        {
            await Task.Delay(10); // lol

            if (IsPause) TogglePause(false);
            if (_gameMode == GameMode.Menu && _player != null) // going back to main menu
            {
                _player.PickUpItemEvent -= ProcessItem;
                _player.SectorClearEvent -= OnSectorClear;
                _player.PauseClickEvent -= TogglePause;
                _player.AimClickEvent -= OnAimClick;
                AudioManager.Instance.PlayMusic("MainMenu");
                transform.position = Vector3.zero;
            }
            if (_gameMode == GameMode.Game)
            {
                transform.position = Vector3.zero;
                _gameObserver = new GameObserver(_levelRules[_currRulesID]);
                PlacePlayer(_moveDatas[_currMoveID]);
                _camCtrl = GetComponent<CameraController>();
                _camCtrl.SetPlayer(_player.transform);
                _ui.UpdateScore(0, 0);
                _tunnel = FindObjectOfType<TunnelController>();
                _tunnel.Generate();
            }

        }

        private void PlacePlayer(PlayerMovementData data)
        {
            _player = Instantiate(_playerFPrefab);
            _player.transform.parent = null;
            _player.PickUpItemEvent += ProcessItem;
            _player.SectorClearEvent += OnSectorClear;
            _player.AimClickEvent += OnAimClick;


            data.MaxSpeedClamp = data.StartSpeed;
            data.MaxSpeedClamp += data.SpeedStep * _gameObserver.GetCurrentRules.MaxBoosts;
            _player.SetPlayerMovementData(data);
            _player.PauseClickEvent += TogglePause;

        }
        private void ProcessItem(BaseItem item)
        {
            var sc = _gameObserver.OnBaseItemPicked(item);
            _ui.UpdateScore(sc,_gameObserver.GetCurrentComboCounter);
            _player.SetSpeed(_gameObserver.GetBoostStage());
        }
        private void UpdateUI()
        {
            _ui.UpdateImpulses(_player.GetControlPercent);
            _ui.UpdateSpeed(_player.GetSpeedPercent);
            if (UpdatingAim)
            {
                _ui.UpdateAim(UpdatingAim, _player.GetAimingDirection);
            }
        }

        private void OnLevelComplete()
        {
            _ui.ShowWinPanel(_gameObserver.GetCurrentScore);
            TogglePause(false);
        }
        private void OnAimClick(bool isAim)
        {
            //UpdatingAim = isAim; // TODO
            //_ui.UpdateAim(isAim,Vector3.zero); // case - aim released - call once to hide element, get real vector in upd 
        }

        private void OnSectorClear (SectorComponent sect)
        {
            _tunnel.OnSectorCleared(sect);
        }

        public void TogglePause (bool showPanel = true)
        {
            IsPause = !IsPause;
            Time.timeScale = IsPause ? 0 : 1;
            _player.SetPause(IsPause);
            if (showPanel) _ui.OnSwitchPauseState(IsPause);
        }
        public void ReturnToMenu()
        {
            AudioManager.Instance.PlayMusic("MainMenu");
            SceneManager.LoadScene(0);
        }
        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }

    }


}