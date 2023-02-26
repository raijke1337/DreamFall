using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Managers
{
    public class UIController : MonoBehaviour
    {
        #region Ingame
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private Transform _pausePanel;
        [SerializeField] private Transform _winPanel;
        [SerializeField,Tooltip("Score field")] private TextMeshProUGUI _winText;

        [SerializeField] private Image _speedBar;
        [SerializeField] private Image _impulseBar;

        [SerializeField] private RectTransform _aim;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">score</param>
        /// <param name="c">combo</param>
        public void UpdateScore(int s, int c)
        {
            if (_score == null) return;
            _score.text = $"Score: {s} \nCombo: {c}";
        }
        private void TogglePausePanel(bool IsPause)
        {
            if (_pausePanel!= null)
            {
                _pausePanel.gameObject.SetActive(IsPause);
            }                       
        }
        public void OnResume()
        {
            GameManager.Instance.TogglePause();
        }

        public void UpdateSpeed (float prog)
        {
            _speedBar.fillAmount = prog;
        }
        public void UpdateImpulses (float prog)
        {
            _impulseBar.fillAmount = prog;
        }
        public void UpdateAim(bool isShow, Vector3 dir)
        {
            _aim.gameObject.SetActive(isShow);
            if (isShow)
            {
                var v = Camera.main.WorldToScreenPoint(dir);
                var scrPoint = new Vector2(v.x, v.y);
                Debug.Log($"Desired aim is {scrPoint}");
            }
        }

        //run from GM!
        public void OnSwitchPauseState(bool isPause)
        {
            TogglePausePanel(isPause);
            ClickSound();
        }
        public void ShowWinPanel(int score)
        {
            _winPanel.gameObject.SetActive(true);
            _winText.text += score.ToString();
        }
        //

        public void OnMainMenu()
        {
            ClickSound();
            GameManager.Instance.ReturnToMenu();
        }

        #endregion
        #region menus

        [SerializeField] private RectTransform _levelSelect;
        [SerializeField] private RectTransform _modeSelect;
        [SerializeField] private float _windowsSwitchTime;
        private string lv;
        private string mode;
        private string move = "base";


        public void OnLevelSelected(string ID)
        {
            ClickSound();
            lv = ID;
            StartCoroutine(WindowSwitch(_levelSelect, _modeSelect));
        }
        public void OnModeSelected(string m)
        {
            ClickSound();
            mode = m;
            OnConfirmLevel();
        }
        private void ClickSound()
        {
            AudioManager.Instance.PlaySound("ButtonClick");
        }
        public void OnConfirmLevel()
        {
            GameManager.Instance.StartLevel(lv, mode, move); 
        }
        public void OnCancelLevel()
        {
            StartCoroutine(WindowSwitch(_modeSelect, _levelSelect));
        }

        public void OnExit()
        {
            ClickSound();
            GameManager.Instance.ExitGame();
        }

        private IEnumerator WindowSwitch(RectTransform hide, RectTransform show)
        {
            float K = 1 / _windowsSwitchTime;
            float prog = 0f;
            show.localScale = Vector3.zero;
            show.gameObject.SetActive(true);
            while (prog < _windowsSwitchTime)
            {
                prog += Time.deltaTime;
                hide.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, prog*K);
                yield return null;
            }
            hide.gameObject.SetActive(false);
            prog = 0f;
            while (prog < _windowsSwitchTime)
            {
                prog += Time.deltaTime;
                show.localScale = Vector3.Lerp(Vector3.zero,Vector3.one, prog*K);
                yield return null;
            }

            yield return null;
        }

        #endregion
    }
}