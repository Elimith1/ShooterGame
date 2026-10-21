using Player;
using UnityEngine;

namespace Pickups
{
    public class MultishotPickup : Collectible
    {
        [SerializeField] private float duration = 10f;

        protected override void OnCollect(GameObject player)
        {
            PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.ActivateMultishot(duration);
            }
        }
    }
}