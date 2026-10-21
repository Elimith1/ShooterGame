using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Missiles Settings")]
        [SerializeField] private int initialMissiles = 5;
        [SerializeField] private int maxMissiles = 20;

        public int CurrentMissiles { get; private set; }

        private void Awake()
        {
            CurrentMissiles = initialMissiles;
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
                return true;
            }
            return false;
        }

        public void AddMissiles(int amount)
        {
            CurrentMissiles = Mathf.Min(CurrentMissiles + amount, maxMissiles);
            Debug.Log($"[Inventaire] +{amount} missiles ! Total : {CurrentMissiles}");
        }
    }
}