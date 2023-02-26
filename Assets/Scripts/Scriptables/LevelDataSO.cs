using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "New Level Data", menuName = "Settings Files/Level Data")]
    public class LevelDataSO : ScriptableObject
    {
        public string LevelID;
        public string LevelName;
        public int SceneIndex;
        public string MusicID;
    }
