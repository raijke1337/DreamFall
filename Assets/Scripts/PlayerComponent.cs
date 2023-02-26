using Game.Control;
using Game.Extras;
using Game.Managers;
using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider), typeof(PlayerController))]
    public class PlayerComponent : MonoBehaviour
    {
        public delegate void PlayerGameEvents<T>(T arg);
        public delegate void PlayerGameEvents();
        public PlayerGameEvents<BaseItem> PickUpItemEvent;
        public PlayerGameEvents<SectorComponent> SectorClearEvent;
        public PlayerGameEvents<bool> PauseClickEvent;
        public PlayerGameEvents<bool> AimClickEvent;
        private PlayerController _ctrl;

        private void OnCollisionEnter(Collision collision)
        {

            if (collision.transform.CompareTag("Item"))
            {
                try
                {
                    PickUpItemEvent?.Invoke(collision.gameObject.GetComponent<BaseItem>());
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"{e.Message} when {this} collided with {collision.gameObject}");
                }
            }
            if (collision.transform.CompareTag("Wall"))
            {
                AudioManager.Instance.PlaySound("Wall");
                _ctrl.RecalcDirection(collision.GetContact(0).normal);
            }

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Sector"))
            {
                SectorClearEvent?.Invoke(other.gameObject.GetComponent<SectorComponent>());
            }
        }

        public void SetPlayerMovementData(PlayerMovementData data)
        {
            if (_ctrl == null) _ctrl = GetComponent<PlayerController>(); 
            _ctrl.MovementData = data;
            _ctrl.PauseEvent += PauseWasClicked;
            _ctrl.AimEvent += OnAimingToggle;
        }
        public void OnUpdateInGame(float deltaTime)
        {
            _ctrl.RunUpdate(deltaTime);
        }
        public Vector3 GetAimingDirection { get => _ctrl.GetAimDirection; }
        public float GetControlPercent { get => _ctrl.GetControlPercent(); }
        public float GetSpeedPercent { get => _ctrl.GetSpeedPercent(); }
        private void PauseWasClicked() => PauseClickEvent?.Invoke(true);
        private void OnAimingToggle(bool t) => AimClickEvent.Invoke(t);
        private void OnDisable()
        {
            _ctrl.PauseEvent -= PauseWasClicked;
            _ctrl.AimEvent -= OnAimingToggle;
        }
        public void SetSpeed(int speed) => _ctrl.IncreaseSpeed(speed);
        public void SetPause(bool isPause) => _ctrl.IsPaused = isPause;
    }
}