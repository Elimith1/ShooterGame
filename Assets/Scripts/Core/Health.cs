using UI;
using UnityEngine;

namespace Core
{
    public class Health : MonoBehaviour, IDamageable
    {
        [Header("Settings")]
        [SerializeField] private int initialHealth = 10;
        [SerializeField] private float invulnerabilityDuration = 0.5f;

        [Header("References")]
        [SerializeField] private HUDManager hudManager;

        public int CurrentHealth { get; private set; }
        private float _lastDamageTime = -999f;

        private void Awake()
        {
            CurrentHealth = initialHealth;
        }

        private void Start()
        {
            UpdateHUDHealth();
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            // Période d'invulnérabilité de 0.5s
            if (Time.time < _lastDamageTime + invulnerabilityDuration) return;

            _lastDamageTime = Time.time;
            CurrentHealth -= damageInfo.Amount;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                UpdateHUDHealth();
                Die();
            }
            else
            {
                UpdateHUDHealth();
            }
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
            UpdateHUDHealth();
        }

        private void UpdateHUDHealth()
        {
            // Met à jour le HUD uniquement si c'est le joueur qui possède ce composant
            if (hudManager != null && CompareTag("Player"))
            {
                hudManager.UpdateHealth(CurrentHealth, initialHealth);
            }
        }

        private void Die()
        {
            if (hudManager != null && CompareTag("Player"))
            {
                hudManager.ShowGameOver();
            }

            // Désactivation pour l'Object Pooling
            gameObject.SetActive(false);
        }
    }
}