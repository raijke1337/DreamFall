using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "New Player Movement Data", menuName = "Settings Files/Player Movement Data")]
    public class PlayerMovementDataSO : ScriptableObject
    {
        public string MoveDataID;
        public float StartSpeed;
        public float SpeedStep;
        public float ControlSpeed;
        //public int BoostCounter;
        //public float BoostRecharge;
        public float ControlsDisabledOnCollisionTimer;
        public float speedRegainMult;
}
