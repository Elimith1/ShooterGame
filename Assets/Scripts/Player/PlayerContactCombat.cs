using Core;
using UnityEngine;

namespace Player
{
    public class PlayerContactCombat : MonoBehaviour
    {
        [SerializeField] private Health playerHealth;

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (!otherCollider.CompareTag("Enemy")) return;

            bool isStompDetector = gameObject.name.Contains("StompDetector");

            // Le joueur subit 1 point de dégât s'il ne saute pas sur la tête
            if (!isStompDetector && playerHealth != null)
            {
                playerHealth.TakeDamage(new DamageInfo(1, transform.position, Vector3.up, otherCollider.gameObject));
            }

            // L'alien est éliminé au contact (999 de dégâts)
            Health enemyHealth = otherCollider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(new DamageInfo(999, otherCollider.transform.position, Vector3.up, gameObject));
            }
        }
    }
}