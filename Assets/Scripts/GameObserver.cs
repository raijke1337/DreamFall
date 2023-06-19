using Game.Extras;
using UnityEngine;
namespace Game.Managers
{
    public class GameObserver
    {
        public GameObserver(LevelRules rules)
        {
            GetCurrentRules = rules;
            _currentScore = 0;
            _currentTimeLevel = 0;
            _comboCounter = 0;
        }
        private float _currentTimeLevel;
        private int _currentScore;
        private int _comboCounter;
        private LevelRules levelRules;

        public int GetCurrentScore { get => _currentScore; }
        public int GetCurrentComboCounter { get => _comboCounter; }
        public LevelRules GetCurrentRules { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">BaseItem</param>
        /// <returns>current score</returns>
        public int OnBaseItemPicked(BaseItem item)
        {
            _currentScore += item.Score + (int)(item.Score*_comboCounter*GetCurrentRules.ComboScoreMult);
            _comboCounter++;
            return _currentScore;
        }
        public void OnItemMissed()
        {
            _comboCounter = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta">deltatime</param>
        /// <returns>is time expired</returns>
        public bool DeltaUpdate(float delta)
        {
            _currentTimeLevel += delta;
            return _currentTimeLevel >= GetCurrentRules.TotalTime;
        }
        // boost is calculated
        public int GetBoostStage()
        {
            return Mathf.Clamp(Mathf.FloorToInt(_comboCounter / GetCurrentRules.ComboToSpeedUp),1,GetCurrentRules.MaxBoosts);
        }
    }
}