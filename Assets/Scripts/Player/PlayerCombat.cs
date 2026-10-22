using System.Collections;
using System.Collections.Generic;
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

        // Utilisation de Listes
        private List<GameObject> _bulletPool;
        private List<GameObject> _missilePool;

        // Index de parcours circulaire (Ring Buffer)
        private int _bulletIndex;
        private int _missileIndex;

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

            if (playerInventory == null)
            {
                playerInventory = GetComponent<PlayerInventory>();
            }

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

            _bulletPool = new List<GameObject>(bulletPoolSize);
            for (int i = 0; i < bulletPoolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                _bulletPool.Add(bullet);
            }
        }

        private void InitializeMissilePool()
        {
            if (missilePrefab == null) return;

            _missilePool = new List<GameObject>(missilePoolSize);
            for (int i = 0; i < missilePoolSize; i++)
            {
                GameObject missile = Instantiate(missilePrefab);
                missile.SetActive(false);
                _missilePool.Add(missile);
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

            if (firePoint == null || _bulletPool == null || _bulletPool.Count == 0) return;

            Quaternion centerRotation = Quaternion.LookRotation(transform.root.forward);

            // Tir central
            SpawnBulletFromPool(firePoint.position, centerRotation);

            // Tirs latéraux si Multishot actif (-15° et +15°)
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
            if (playerInventory != null && !playerInventory.HasMissiles())
            {
                return;
            }

            _nextMissileTime = Time.time + missileCooldown;

            if (firePoint == null || _missilePool == null || _missilePool.Count == 0) return;

            if (playerInventory != null && !playerInventory.UseMissile())
            {
                return;
            }

            Quaternion missileRotation = Quaternion.LookRotation(transform.root.forward);
            SpawnMissileFromPool(firePoint.position, missileRotation);
        }

        private void SpawnBulletFromPool(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            // Récupération directe par index
            GameObject bullet = _bulletPool[_bulletIndex];

            // Réinitialisation si la balle était déjà active
            bullet.SetActive(false);
            bullet.transform.position = spawnPosition;
            bullet.transform.rotation = spawnRotation;
            bullet.SetActive(true);

            // Incrémentation circulaire de l'index
            _bulletIndex = (_bulletIndex + 1) % _bulletPool.Count;
        }

        private void SpawnMissileFromPool(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            // Récupération directe par index
            GameObject missile = _missilePool[_missileIndex];

            // Réinitialisation si le missile était déjà actif
            missile.SetActive(false);
            missile.transform.position = spawnPosition;
            missile.transform.rotation = spawnRotation;
            missile.SetActive(true);

            // Incrémentation circulaire de l'index
            _missileIndex = (_missileIndex + 1) % _missilePool.Count;
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