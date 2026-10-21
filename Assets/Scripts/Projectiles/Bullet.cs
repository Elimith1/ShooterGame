using Core;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 80f;
        [SerializeField] private float lifeTime = 5f;
        [SerializeField] private int damageAmount = 1;

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

        private void OnTriggerEnter(Collider otherCollider)
        {
            //Ignorer le Joueur
            if (otherCollider.CompareTag("Player")) return;

            // Ignorer les autres Balles
            if (otherCollider.CompareTag("Bullet")) return;

           // Appliquer les dégâts aux ennemis / cibles
            Health targetHealth = otherCollider.GetComponent<Health>();
            if (targetHealth != null)
            {
                DamageInfo damage = new DamageInfo(damageAmount, transform.position, transform.forward, gameObject);
                targetHealth.TakeDamage(damage);
            }
            // Désactiver la balle au choc
            gameObject.SetActive(false);
        }
    }
}