using UnityEngine;
namespace Game.Managers
{
    public class CameraController : MonoBehaviour
    {
        private Transform _player;
        private Vector3 _desiredPos;
        private Vector3 _currentPos;

        public void SetPlayer(Transform t)
        {
            _player = t;
            transform.LookAt(t, Vector3.forward);
            transform.position += new Vector3(0, yOffset, 0);
        }


        public float yOffset;
        [Tooltip("Max range of cam to player")] public float yOffsetMax;
        public float lerpSpeed;

        private float lerping;

        private void Update()
        {
            if (_player != null)
            {
                _currentPos = transform.position;
                _desiredPos = new Vector3(_player.position.x, _player.position.y + yOffset, _player.position.z);
                if (_desiredPos != _currentPos)
                {
                    Vector3 rangeCheck = _player.position - _currentPos;
                    if (rangeCheck.y > yOffsetMax)
                    {
                        float fixRange = rangeCheck.y - yOffsetMax;
                        transform.position = new Vector3(transform.position.x, transform.position.y + fixRange, transform.position.z);
                        _currentPos = transform.position;
                    }

                    lerping = 0f;
                    lerping += Time.deltaTime * lerpSpeed;
                    transform.position = Vector3.Lerp(_currentPos, _desiredPos, lerping);
                    transform.LookAt(_player, Vector3.forward);
                }
            }
        }
    }


}