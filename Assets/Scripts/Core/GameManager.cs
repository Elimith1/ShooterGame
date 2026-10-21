using Pickups;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CollectibleSpawner collectibleSpawner;
        
        [Header("Debug Spawn Random")]
        [SerializeField] private float spawnRadius = 5f;

        private void Awake()
        {
            if (collectibleSpawner == null)
            {
                collectibleSpawner = FindAnyObjectByType<CollectibleSpawner>();
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
                // Calcule un décalage aléatoire autour du joueur sur le plan XZ
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