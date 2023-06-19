using Game.Extras;
using Game.Managers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static Game.PlayerComponent;

namespace Game.Control
{

    public class PlayerController : MonoBehaviour
    {

        private Collider _col;
        GameCtrl _control;
        private PlayerMovementData _data;
        public PlayerGameEvents PauseEvent;

        private float _moveSpeedZ;

        private Vector3 mousePos;

        //private Vector3 controlPos;
        public bool IsPaused { get; set; }
#if UNITY_EDITOR
        public Vector3 DebugVector3 { get => mousePos; }
#endif
        public Vector3 GetMousePos => mousePos;

        public float GetSpeedPercent()
        {
            return _moveSpeedZ / _data.MaxSpeedClamp;
        }

        public PlayerMovementData MovementData
        {
            get => _data;
            set
            {
                _data = value;
                IsPaused = false;
                _moveSpeedZ =  _data.StartSpeed;        
            }
        }
        public void IncreaseSpeed(int boostStage)
        {
            var newSpeed = _data.SpeedStep * boostStage;
            newSpeed = Mathf.Clamp(newSpeed, 0, _data.MaxSpeedClamp);
            Debug.Log($"Speed change from {_moveSpeedZ} to {newSpeed}");
            _moveSpeedZ = newSpeed;

        }
        public void RunUpdate(float delta)
        {
            if (IsPaused) return;

            var input = Mouse.current.position.ReadValue();
            var input3d = new Vector3(input.x, input.y, GameManager.Instance.GetCameraDistance);

            mousePos = Camera.main.ScreenToWorldPoint(input3d);

            transform.position = mousePos;

            transform.position += new Vector3(0, 0, _moveSpeedZ * delta);
            
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(mousePos, 0.3f);
        }

        public void RecalcDirection(Vector3 normal)
        {

            //bouncedDirectionInitialVector = Vector3.Reflect(currentDirection, normal);
            //currentDirection = new Vector3(bouncedDirectionInitialVector.x,0.1f, bouncedDirectionInitialVector.y);
            //bouncedSpeedLerp = 0f;

            if (_data.ControlsDisabledOnCollisionTimer > 0f) StartCoroutine(CollisionsBandaid(_data.ControlsDisabledOnCollisionTimer));
        }

        private void OnPauseButton(InputAction.CallbackContext obj)
        {
            PauseEvent?.Invoke();
        }

        private IEnumerator CollisionsBandaid(float time)
        {
            
            _col.isTrigger = true;
            float done = 0f;
            while (done <= time)
            {
                done += Time.deltaTime;
             yield return null;
            }

            _col.isTrigger = false;
            yield return null;
        }

        #region bind controls
        private void ControlBind(bool isOn)
        {
            if (isOn)
            {
                _control.Default.Enable();
                _control.Default.Pause.performed += OnPauseButton;
                _col = GetComponent<Collider>();
                Assert.IsNotNull(_col);
            }
            else
            {
                _control.Default.Pause.performed -= OnPauseButton;
                _control.Default.Disable();
            }
        }
        private void OnEnable()
        {
            _control = new GameCtrl();
            ControlBind(true);
        }
        private void OnDisable()
        {
            ControlBind(false);
            _control.Dispose();
        }



        #endregion
    }
}