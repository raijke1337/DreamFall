
using TMPro;
using UnityEngine;


namespace Game.Managers
{
    public class CameraController : MonoBehaviour
    {
        private Vector3 _target;
        private PlayerComponent _player;
        [SerializeField] private float SmoothDampTime = 1f;
        [SerializeField] private float zOffset;
        public float CurrentOffsetZ => _target.z - transform.position.z;

        private Vector3 _velo = Vector3.zero;

        public void SetPlayer(PlayerComponent t)
        {
            _player = t;
            
            transform.position += new Vector3(0, 0, -zOffset);
            transform.LookAt(t.transform, Vector3.forward);
        }

        private void Update()
        {
            if (_player == null) return;

            //_target = _player.GetMouseTarget; // alt look kinda nauseating
            _target = _player.transform.position;   
            {
                Vector3 tgt = new Vector3 ( _target.x, _target.y, _target.z-zOffset);
                transform.position = Vector3.SmoothDamp
                    (transform.position, tgt, ref _velo, SmoothDampTime);
            }
        }
    }


}