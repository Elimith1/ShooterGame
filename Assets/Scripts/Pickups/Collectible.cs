using UnityEngine;

namespace Pickups
{
    public abstract class Collectible : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private float lifeTime = 15f;

        private float _lifeTimer;

        private void OnEnable()
        {
            _lifeTimer = 0f;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

            _lifeTimer += Time.deltaTime;
            if (_lifeTimer >= lifeTime)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.CompareTag("Player"))
            {
                OnCollect(otherCollider.gameObject);
                gameObject.SetActive(false);
            }
        }

        protected abstract void OnCollect(GameObject player);
    }
}