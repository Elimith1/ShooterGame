using UnityEngine;

namespace Pickups
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [Header("Chance Settings")]
        [Tooltip("1 chance sur dropChanceDenominator de faire apparaître un item")]
        [SerializeField] private int dropChanceDenominator = 15;

        [Header("Prefabs des Collectibles")]
        [SerializeField] private GameObject healPrefab;
        [SerializeField] private GameObject multishotPrefab;
        [SerializeField] private GameObject missileSupplyPrefab;

        [Header("Taille des Pools")]
        [SerializeField] private int poolSizePerType = 5;

        private GameObject[] _healPool;
        private GameObject[] _multishotPool;
        private GameObject[] _missilePool;

        private void Awake()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            _healPool = CreatePool(healPrefab);
            _multishotPool = CreatePool(multishotPrefab);
            _missilePool = CreatePool(missileSupplyPrefab);
        }

        private GameObject[] CreatePool(GameObject prefab)
        {
            if (prefab == null) return new GameObject[0];

            GameObject[] pool = new GameObject[poolSizePerType];
            for (int i = 0; i < poolSizePerType; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                pool[i] = obj;
            }
            return pool;
        }

        public void TrySpawnCollectible(Vector3 position)
        {
            int randomRoll = Random.Range(1, dropChanceDenominator + 1);

            if (randomRoll == 1)
            {
                int typeRoll = Random.Range(0, 3);
                GameObject[] targetPool = null;

                switch (typeRoll)
                {
                    case 0: targetPool = _healPool; break;
                    case 1: targetPool = _multishotPool; break;
                    case 2: targetPool = _missilePool; break;
                }

                SpawnFromPool(targetPool, position);
            }
        }

        private void SpawnFromPool(GameObject[] pool, Vector3 position)
        {
            if (pool == null) return;

            for (int i = 0; i < pool.Length; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].transform.position = position;
                    pool[i].transform.rotation = Quaternion.identity;
                    pool[i].SetActive(true);
                    break;
                }
            }
        }
    }
}