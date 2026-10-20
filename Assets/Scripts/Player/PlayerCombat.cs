using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private PlayerInputReader inputReader;

        [Header("Primary Fire")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float fireRate = 0.15f;
        [SerializeField] private int bulletPoolSize = 30;

        [Header("Missiles")]
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private float missileCooldown = 1.5f;
        [SerializeField] private int missilePoolSize = 10;

        private GameObject[] _bulletPool;
        private GameObject[] _missilePool;

        private float _nextFireTime;
        private float _nextMissileTime;

        private bool _isMultishotActive;
        private float _multishotTimer;

        private void Awake()
        {
            if (inputReader == null)
            {
                inputReader = GetComponent<PlayerInputReader>();
            }

            InitializeBulletPool();
            InitializeMissilePool();
        }

        private void InitializeBulletPool()
        {
            if (bulletPrefab == null) return;

            _bulletPool = new GameObject[bulletPoolSize];
            for (int i = 0; i < bulletPoolSize; i++)
            {
                _bulletPool[i] = Instantiate(bulletPrefab);
                _bulletPool[i].SetActive(false);
            }
        }

        private void InitializeMissilePool()
        {
            if (missilePrefab == null) return;

            _missilePool = new GameObject[missilePoolSize];
            for (int i = 0; i < missilePoolSize; i++)
            {
                _missilePool[i] = Instantiate(missilePrefab);
                _missilePool[i].SetActive(false);
            }
        }

        private void Update()
        {
            UpdateMultishotTimer();

            if (inputReader.IsFiring)
            {
                if (Time.time >= _nextFireTime)
                {
                    FireBullet();
                }
            }
            else if (inputReader.IsMissileTriggered)
            {
                if (Time.time >= _nextMissileTime)
                {
                    FireMissile();
                }

                inputReader.ResetMissileTrigger();
            }
        }

        private void UpdateMultishotTimer()
        {
            if (!_isMultishotActive) return;

            _multishotTimer -= Time.deltaTime;
            if (_multishotTimer <= 0f)
            {
                _isMultishotActive = false;
            }
        }

        private void FireBullet()
        {
            _nextFireTime = Time.time + fireRate;

            if (firePoint == null || _bulletPool == null) return;

            Vector3 rootForward = transform.root.forward;
            Vector3 rootRight = transform.root.right;

            SpawnBulletFromPool(firePoint.position, Quaternion.LookRotation(rootForward));

            if (_isMultishotActive)
            {
                Vector3 leftDirection = (rootForward * 0.5f - rootRight * 0.5f).normalized;
                Vector3 rightDirection = (rootForward * 0.5f + rootRight * 0.5f).normalized;

                SpawnBulletFromPool(firePoint.position, Quaternion.LookRotation(leftDirection));
                SpawnBulletFromPool(firePoint.position, Quaternion.LookRotation(rightDirection));
            }
        }

        private void FireMissile()
        {
            _nextMissileTime = Time.time + missileCooldown;

            if (firePoint == null || _missilePool == null) return;

            Vector3 rootForward = transform.root.forward;
            SpawnMissileFromPool(firePoint.position, Quaternion.LookRotation(rootForward));
        }

        private void SpawnBulletFromPool(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            for (int i = 0; i < _bulletPool.Length; i++)
            {
                if (!_bulletPool[i].activeInHierarchy)
                {
                    _bulletPool[i].transform.position = spawnPosition;
                    _bulletPool[i].transform.rotation = spawnRotation;
                    _bulletPool[i].SetActive(true);
                    break;
                }
            }
        }

        private void SpawnMissileFromPool(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            for (int i = 0; i < _missilePool.Length; i++)
            {
                if (!_missilePool[i].activeInHierarchy)
                {
                    _missilePool[i].transform.position = spawnPosition;
                    _missilePool[i].transform.rotation = spawnRotation;
                    _missilePool[i].SetActive(true);
                    break;
                }
            }
        }

        public void ActivateMultishot(float duration)
        {
            _multishotTimer = _isMultishotActive ? _multishotTimer + duration : duration;
            _isMultishotActive = true;
        }
    }
}