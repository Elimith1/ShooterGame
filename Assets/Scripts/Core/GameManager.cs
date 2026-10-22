using Pickups;
using UI;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CollectibleSpawner collectibleSpawner;
        
        [SerializeField] private float spawnRadius = 5f;

        private void Awake()
        {
            if (collectibleSpawner == null)
            {
                collectibleSpawner = GetComponent<CollectibleSpawner>();
            }
        }

        private void Update()
        {
            // Touche de test 'K'
            if (Input.GetKeyDown(KeyCode.K))
            {
                TestSpawnCollectible();
            }
        }

        public void TestSpawnCollectible()
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null && collectibleSpawner != null)
            {
                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
                Vector3 randomPosition = player.transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

                collectibleSpawner.TrySpawnCollectible(randomPosition);
            }
        }

        // Appelé à la mort d'un ennemi
        public void OnEnemyKilled(Vector3 enemyPosition)
        {
            if (collectibleSpawner != null)
            {
                collectibleSpawner.TrySpawnCollectible(enemyPosition);
            }
        }
    }
}