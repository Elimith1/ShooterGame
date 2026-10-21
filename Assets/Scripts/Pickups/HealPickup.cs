using Core;
using UnityEngine;

namespace Pickups
{
    public class HealPickup : Collectible
    {
        [SerializeField] private int healAmount = 1;

        protected override void OnCollect(GameObject player)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }
        }
    }
}