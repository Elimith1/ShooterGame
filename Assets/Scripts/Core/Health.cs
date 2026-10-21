using UnityEngine;

namespace Core
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Settings")]
        [SerializeField] private int initialHealth = 10;
        [SerializeField] private float invulnerabilityDuration = 0.5f;

        public int CurrentHealth { get; private set; }
        private float _lastDamageTime = -999f;

        private void Awake()
        {
            CurrentHealth = initialHealth;
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            // Vérification de la période d'invulnérabilité (0.5s)
            if (Time.time < _lastDamageTime + invulnerabilityDuration) return;

            _lastDamageTime = Time.time;
            CurrentHealth -= damageInfo.Amount;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
        }

        private void Die()
        {
            // Désactive le joueur ou l'ennemi à sa mort
            gameObject.SetActive(false);
        }
    }
}