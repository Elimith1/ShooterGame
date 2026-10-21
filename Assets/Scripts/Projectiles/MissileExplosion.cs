using Core;
using UnityEngine;

namespace Player
{
    public class MissileExplosion : MonoBehaviour
    {
        [Header("Dégâts de zone")]
        [SerializeField] private int explosionDamage = 5;
        [SerializeField] private float damageDuration = 0.15f;
        [SerializeField] private SphereCollider explosionCollider;

        [Header("Effet Visuel")]
        [SerializeField] private ParticleSystem explosionParticles;

        private float _timer;

        private void Awake()
        {
            if (explosionCollider == null)
            {
                explosionCollider = GetComponent<SphereCollider>();
            }

            if (explosionParticles == null)
            {
                explosionParticles = GetComponentInChildren<ParticleSystem>();
            }
        }

        private void OnEnable()
        {
            _timer = 0f;

            // 1. Activer le collider de dégâts au départ
            if (explosionCollider != null)
            {
                explosionCollider.isTrigger = true;
                explosionCollider.enabled = true;
            }

            // 2. Forcer le système de particules à rejouer depuis le début
            if (explosionParticles != null)
            {
                explosionParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                explosionParticles.Play(true);
            }
        }

        private void Update()
        {
            _lifeTimer();
        }

        private void _lifeTimer()
        {
            _timer += Time.deltaTime;

            // Coupe les dégâts après une fraction de seconde
            if (_timer >= damageDuration && explosionCollider != null)
            {
                explosionCollider.enabled = false;
            }

            // Se désactive automatiquement dès que la particule a fini d'exécuter son animation
            if (explosionParticles != null)
            {
                if (!explosionParticles.IsAlive(true))
                {
                    gameObject.SetActive(false);
                }
            }
            else if (_timer >= 2f)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            // Ignorer le joueur
            if (otherCollider.CompareTag("Player") || otherCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return;
            }

            // Dégâts de zone simples
            Health targetHealth = otherCollider.GetComponent<Health>();
            if (targetHealth != null)
            {
                DamageInfo damage = new DamageInfo(explosionDamage, transform.position, Vector3.up, gameObject);
                targetHealth.TakeDamage(damage);
            }
        }
    }
}