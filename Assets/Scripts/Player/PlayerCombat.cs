using UI;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private PlayerInputReader inputReader;
        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private HUDManager hudManager;

        [Header("Primary Fire")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float fireRate = 0.15f;
        [SerializeField] private int bulletPoolSize = 90;

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
            if (inputReader == null) inputReader = GetComponent<PlayerInputReader>();
            if (playerInventory == null) playerInventory = GetComponent<PlayerInventory>();

            InitializeBulletPool();
            InitializeMissilePool();
        }

        private void Start()
        {
            if (hudManager != null)
            {
                hudManager.UpdateTripleShotTimer(0f);
            }
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

            if (inputReader == null) return;

            bool isFiringBullets = inputReader.IsFiring;
            bool isFiringMissile = inputReader.IsMissileTriggered;

            if (isFiringBullets && !isFiringMissile)
            {
                if (Time.time >= _nextFireTime)
                {
                    FireBullet();
                }
            }
            else if (isFiringMissile && !isFiringBullets)
            {
                if (Time.time >= _nextMissileTime)
                {
                    FireMissile();
                }
                inputReader.ResetMissileTrigger();
            }
            else if (isFiringBullets && isFiringMissile)
            {
                if (Time.time >= _nextFireTime)
                {
                    FireBullet();
                }
                inputReader.ResetMissileTrigger();
            }
        }

        private void UpdateMultishotTimer()
        {
            if (!_isMultishotActive) return;

            _multishotTimer -= Time.deltaTime;

            if (hudManager != null)
            {
                hudManager.UpdateTripleShotTimer(_multishotTimer);
            }

            if (_multishotTimer <= 0f)
            {
                _isMultishotActive = false;
                _multishotTimer = 0f;

                if (hudManager != null)
                {
                    hudManager.UpdateTripleShotTimer(0f);
                }
            }
        }

        private void FireBullet()
        {
            _nextFireTime = Time.time + fireRate;

            if (firePoint == null || _bulletPool == null) return;

            Quaternion centerRotation = Quaternion.LookRotation(transform.root.forward);

            SpawnBulletFromPool(firePoint.position, centerRotation);

            if (_isMultishotActive)
            {
                Quaternion leftRotation = centerRotation * Quaternion.Euler(0f, -15f, 0f);
                Quaternion rightRotation = centerRotation * Quaternion.Euler(0f, 15f, 0f);

                SpawnBulletFromPool(firePoint.position, leftRotation);
                SpawnBulletFromPool(firePoint.position, rightRotation);
            }
        }

        private void FireMissile()
        {
            if (playerInventory != null && !playerInventory.HasMissiles()) return;

            _nextMissileTime = Time.time + missileCooldown;

            if (firePoint == null || _missilePool == null) return;

            if (playerInventory != null && !playerInventory.UseMissile()) return;

            Quaternion missileRotation = Quaternion.LookRotation(transform.root.forward);
            SpawnMissileFromPool(firePoint.position, missileRotation);
        }

        private void SpawnBulletFromPool(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            for (int i = 0; i < _bulletPool.Length; i++)
            {
                if (!_bulletPool[i].activeSelf) 
                {
                    _bulletPool[i].transform.position = spawnPosition;
                    _bulletPool[i].transform.rotation = spawnRotation;
                    _bulletPool[i].SetActive(true);
                    return;
                }
            }
        }

        private void SpawnMissileFromPool(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            for (int i = 0; i < _missilePool.Length; i++)
            {
                if (!_missilePool[i].activeSelf) 
                {
                    _missilePool[i].transform.position = spawnPosition;
                    _missilePool[i].transform.rotation = spawnRotation;
                    _missilePool[i].SetActive(true);
                    return;
                }
            }
        }

        public void ActivateMultishot(float duration)
        {
            if (_isMultishotActive)
            {
                _multishotTimer += duration;
            }
            else
            {
                _multishotTimer = duration;
                _isMultishotActive = true;
            }

            if (hudManager != null)
            {
                hudManager.UpdateTripleShotTimer(_multishotTimer);
            }
        }
    }
}