using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Missile : MonoBehaviour
    {
        [SerializeField] private float speed = 50f;
        [SerializeField] private float lifeTime = 4f;
        [SerializeField] private float spawnImmunityDuration = 0.1f;

        [Header("Prefab d'Explosion")]
        [SerializeField] private GameObject explosionPrefab;

        private Rigidbody _rigidBody;
        private float _lifeTimer;
        private GameObject _explosionInstance;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();

            // Instanciation unique au lancement de la scène
            if (explosionPrefab != null)
            {
                _explosionInstance = Instantiate(explosionPrefab);
                _explosionInstance.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _lifeTimer = 0f;

            if (_rigidBody != null)
            {
                _rigidBody.linearVelocity = transform.forward * speed;
            }

            if (_explosionInstance != null)
            {
                _explosionInstance.SetActive(false);
            }
        }

        private void Update()
        {
            _lifeTimer += Time.deltaTime;

            if (_lifeTimer >= lifeTime)
            {
                Explode();
            }
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (_lifeTimer < spawnImmunityDuration) return;
            if (otherCollider.isTrigger) return;
            if (otherCollider.CompareTag("Player") || otherCollider.gameObject.layer == LayerMask.NameToLayer("Player")) return;

            Explode();
        }

        private void Explode()
        {
            if (_explosionInstance != null)
            {
                // Place l'explosion à l'endroit de l'impact et l'active
                _explosionInstance.transform.position = transform.position;
                _explosionInstance.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}