using UI;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Missiles Settings")]
        [SerializeField] private int initialMissiles = 5;
        [SerializeField] private int maxMissiles = 20;

        [Header("References")]
        [SerializeField] private HUDManager hudManager;

        public int CurrentMissiles { get; private set; }

        private void Awake()
        {
            CurrentMissiles = initialMissiles;
        }

        private void Start()
        {
            UpdateHUD();
        }

        public int GetMaxMissiles()
        {
            return maxMissiles;
        }

        public bool HasMissiles()
        {
            return CurrentMissiles > 0;
        }

        public bool UseMissile()
        {
            if (CurrentMissiles > 0)
            {
                CurrentMissiles--;
                UpdateHUD();
                return true;
            }
            return false;
        }

        public void AddMissiles(int amount)
        {
            CurrentMissiles = Mathf.Min(CurrentMissiles + amount, maxMissiles);
            UpdateHUD();
            Debug.Log($"[Inventaire] +{amount} missiles ! Total : {CurrentMissiles}");
        }

        private void UpdateHUD()
        {
            if (hudManager != null)
            {
                hudManager.UpdateAmmo(CurrentMissiles, maxMissiles);
            }
        }
    }
}