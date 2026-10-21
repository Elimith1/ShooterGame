using Player;
using UnityEngine;

namespace Pickups
{
    public class MissilePickup : Collectible
    {
        [SerializeField] private int missileAmount = 5;

        protected override void OnCollect(GameObject player)
        {
            PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.AddMissiles(missileAmount);
            }
        }
    }
}