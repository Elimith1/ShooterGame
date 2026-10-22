using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Missile : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 50f;
        [SerializeField] private float lifeTime = 4f;
        [SerializeField] private int damage = 5;

        private Rigidbody _rigidBody;
        private float _lifeTimer;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _lifeTimer = 0f;

            if (_rigidBody != null)
            {
                _rigidBody.linearVelocity = transform.forward * speed;
            }
        }

        private void Update()
        {
            _lifeTimer += Time.deltaTime;

            if (_lifeTimer >= lifeTime)
            {
                gameObject.SetActive(false);
            }
        }

        // Méthode appelée par MissileExplosion.cs dans son Start()
        public int GetDamage()
        {
            return damage;
        }
    }
}