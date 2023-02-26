using Game.Extras;
using Game.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static Game.PlayerComponent;

namespace Game.Control
{
    public class PlayerController : MonoBehaviour
    {
        
        private Vector2 ctrlFrom;
        private Vector2 ctrlTo;
        private Collider _col;

        private Vector3 currentDirection;
        private PlayerMovementData _data;

        private float currentDesiredSpeedY;
        private float currentSpeedY;
        private float bouncedSpeedLerp = 0f;

        private bool isValidAim = false;

        public PlayerGameEvents PauseEvent;
        public PlayerGameEvents<bool> AimEvent;

        GameCtrl _control;
        private Queue<Timer> _timers;

        private Vector3 _aimDir;
        public Vector3 GetAimDirection => _aimDir;

        public bool IsPaused { get; set; }

        public float GetSpeedPercent()
        {
            return currentSpeedY / _data.MaxSpeedClamp;
        }
        public float GetControlPercent()
        {
            float f = _data.BoostCounter - _timers.Count;
                f = f / _data.BoostCounter;
            return f;
        }
        public PlayerMovementData MovementData
        {
            get => _data;
            set
            {
                _data = value;
                IsPaused = false;
                currentDesiredSpeedY = value.StartSpeed;
                currentDirection = new Vector3(0, -currentDesiredSpeedY, 0); // negative because he is falling down
                _timers = new Queue<Timer>();
            }
        }
        public void IncreaseSpeed(int boostStage)
        {
            currentDesiredSpeedY = _data.StartSpeed + _data.SpeedStep * boostStage;
        }

        private void AimStart(InputAction.CallbackContext callbackContext)
        {
            if (IsPaused) return;
            if (_timers.Count >= _data.BoostCounter) return; // cant control because no impulses left

            ctrlFrom = _control.Default.Aim.ReadValue<Vector2>();
            Ray r = Camera.main.ScreenPointToRay(ctrlFrom);
            if (Physics.Raycast(r, out RaycastHit h))
            {
                ToggleAiming(h.collider.CompareTag("Player"));
            }

        }
        private void AimDone(InputAction.CallbackContext callbackContext)
        {
            if (IsPaused) return;
            if (!isValidAim) return;
            if (_timers.Count >= _data.BoostCounter) return;

            //Debug.Log($"Generated a movement vector {desiredVector} from {origVector}");

            currentDirection = new Vector3(_aimDir.x,currentDirection.y, _aimDir.z); // TODO different strength of impulse depending on vector length

            _timers.Enqueue(new Timer(_data.BoostRecharge));
            ToggleAiming(false);
        }
        private void ToggleAiming(bool isSlow)
        {
            isValidAim = isSlow;
            AimEvent?.Invoke(isSlow);
            Time.timeScale = isSlow ? 0.3f : 1;
        }
        public void RunUpdate(float delta)
        {
            if (IsPaused) return;
            if (currentDirection == null) return;
            if (isValidAim)
            {
                ctrlTo = _control.Default.Aim.ReadValue<Vector2>();
                Vector2 vect = ctrlFrom - ctrlTo;
                Vector3 origVector = new Vector3(vect.x, 0, vect.y);
                _aimDir = Vector3.ClampMagnitude(origVector, 1f * _data.ImpulseMult);
            }

            foreach (var t in _timers.ToList())
            {
                if (t.DeltaOperation(delta))
                {
                    _timers.Dequeue();
                }
            }
            // this part slows the char down after wall impact
            bouncedSpeedLerp = Mathf.Clamp01(bouncedSpeedLerp += delta*_data.speedRegainMult);
            currentSpeedY = Mathf.Lerp(0, currentDesiredSpeedY, bouncedSpeedLerp);
            currentDirection = new Vector3(currentDirection.x, -currentSpeedY, currentDirection.z);
            //

            transform.position += currentDirection * delta;
        }
        public void RecalcDirection(Vector3 normal)
        {
            var reflect = Vector3.Reflect(currentDirection, normal);
            currentDirection = new Vector3(reflect.x,0.1f,reflect.z);
            bouncedSpeedLerp = 0f;

            if (_data.ColliderDelay > 0f) StartCoroutine(CollisionsBandaid(_data.ColliderDelay));
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
                isValidAim = false;
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
                _control.Default.Click.started += AimStart;
                _control.Default.Click.canceled += AimDone;
                _control.Default.Pause.performed += OnPauseButton;
                _col = GetComponent<Collider>();
                Assert.IsNotNull(_col);
            }
            else
            {
                _control.Default.Click.started -= AimStart;
                _control.Default.Click.canceled -= AimDone;
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