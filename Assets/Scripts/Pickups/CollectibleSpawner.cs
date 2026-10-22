using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pickups
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [SerializeField] private int dropChanceDenominator = 15;

        [Header("Prefabs des Collectibles")]
        [SerializeField] private GameObject healPrefab;
        [SerializeField] private GameObject multishotPrefab;
        [SerializeField] private GameObject missileSupplyPrefab;
        [SerializeField] private int poolSizePerType = 5;

        private List<GameObject> _healPool;
        private List<GameObject> _multishotPool;
        private List<GameObject> _missilePool;

        // Index de parcours circulaire pour chaque pool
        private int _healIndex;
        private int _multishotIndex;
        private int _missileIndex;

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

        private List<GameObject> CreatePool(GameObject prefab)
        {
            List<GameObject> pool = new List<GameObject>(poolSizePerType);

            if (prefab == null) return pool;

            for (int i = 0; i < poolSizePerType; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                pool.Add(obj);
            }

            return pool;
        }

        public void TrySpawnCollectible(Vector3 position)
        {
            int randomRoll = Random.Range(1, dropChanceDenominator + 1);

            if (randomRoll == 1)
            {
                int typeRoll = Random.Range(0, 3);

                switch (typeRoll)
                {
                    case 0:
                        SpawnFromPool(_healPool, ref _healIndex, position);
                        break;
                    case 1:
                        SpawnFromPool(_multishotPool, ref _multishotIndex, position);
                        break;
                    case 2:
                        SpawnFromPool(_missilePool, ref _missileIndex, position);
                        break;
                }
            }
        }

        private void SpawnFromPool(List<GameObject> pool, ref int index, Vector3 position)
        {
            if (pool == null || pool.Count == 0) return;

            // Récupération directe par index
            GameObject item = pool[index];

            item.SetActive(false);
            item.transform.position = position;
            item.transform.rotation = Quaternion.identity;
            item.SetActive(true);

            // Incrémentation circulaire de l'index
            index = (index + 1) % pool.Count;
        }
    }
}