using System.Collections;
using System.Collections.Generic;
using UnityEngine;




    [CreateAssetMenu(fileName = "New Level Rules", menuName = "Settings Files/Level Rules Data")]
    public class LevelRulesSO : ScriptableObject
    {
        public string RulesID;
        public float TotalTime;
        public int ComboToSpeedUp;
        public int MaxBoosts;
        public float ComboScoreMult;
    }
