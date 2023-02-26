using Game.Managers;
using UnityEngine;
namespace Game
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class BaseItem : MonoBehaviour
    {
        [SerializeField] private int _debugScore = 1;

        private Vector3 _rotateRandom;

        public int Score { get; protected set; }

        public string AudioPickup;

        private void OnEnable()
        {
            _rotateRandom = Random.insideUnitSphere;
        }
        void Start()
        {
            Score = _debugScore;
        }

        private void Update()
        {
            transform.eulerAngles +=  _rotateRandom;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySound(AudioPickup);
                Destroy(gameObject);
            }
        }

    }

}