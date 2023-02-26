using System;
using UnityEngine;


namespace Game.Extras
{
    [Serializable]
    public class AudioData
    {
        public string ID;
        public AudioClip Clip;

        public bool Loop;
        [Range(0, 1)] public float Volume;
        [Range(.1f, 3)] public float Pitch;

    }

    [Serializable]
    public struct PlayerMovementData
    {
        public PlayerMovementData(PlayerMovementDataSO data)
        {
            StartSpeed = data.StartSpeed;
            SpeedStep = data.SpeedStep;
            ImpulseMult = data.ImpulseMult;
            BoostCounter = data.BoostCounter;
            BoostRecharge = data.BoostRecharge;
            MaxSpeedClamp = data.StartSpeed;
            MovementDataID = data.MoveDataID;
            ColliderDelay = data.ColliderDelay;
            speedRegainMult = data.speedRegainMult;

        }
        public string MovementDataID;
        [Tooltip("Starting speed")] public float StartSpeed;
        [Tooltip("Speed increase per 1 boost")] public float SpeedStep;
        [Tooltip("Max speed of player"), HideInInspector] public float MaxSpeedClamp; // hidden because it is calculated by GM and set on instantiation
        [Tooltip("Control impulse multiplier")] public float ImpulseMult;
        [Tooltip("Control impulses count")] public int BoostCounter;
        [Tooltip("Recharge of impluses")] public float BoostRecharge;
        [Tooltip("Disable collider for f seconds to prevent glitches")] public float ColliderDelay;
        [Tooltip("Mult for regaining speed after collision with wall")] public float speedRegainMult;
    }

    [Serializable]
    public struct LevelData
    {
        public string LevelID;
        public string LevelName;
        public int SceneIndex;
        public string MusicID;
        public LevelData(LevelDataSO data)
        {
            LevelID = data.LevelID;
            SceneIndex = data.SceneIndex;
            MusicID = data.MusicID;
            LevelName = data.LevelName;
        }
    }
    [Serializable]
    public struct LevelRules
    {
        public string LevelRulesID; // maybe use for easy / med / hard
        [Tooltip("Time player gets on level")] public float TotalTime;
        [Tooltip("Combo counter to get 1 speed boost")] public int ComboToSpeedUp;
        [Tooltip("Maximum speed boosts")] public int MaxBoosts;
        [Tooltip("Multiplier of score per combo"), Range(0, 1)] public float ComboScoreMult;

        public LevelRules(LevelRulesSO data)
        {
            LevelRulesID = data.RulesID;
            TotalTime = data.TotalTime;
            ComboScoreMult = data.ComboScoreMult;
            MaxBoosts = data.MaxBoosts;
            ComboToSpeedUp = data.ComboToSpeedUp;
        }
    }

    public class Timer
    {
        private float timeLeft;
        public bool DeltaOperation(float time)
        {
            timeLeft -= time;
            return timeLeft <= 0;
        }
        public Timer(float time)
        {
            timeLeft = time;
        }
    }
    public enum GameMode
    {
        Menu,
        Game
    }
    [Serializable]
    public struct SpawnRange
    {
        public float MinRange;
        public float MaxRange;
    }
}