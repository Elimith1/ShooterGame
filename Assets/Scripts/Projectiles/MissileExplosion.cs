using System.Collections;
using Core;
using Player;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private GameObject missileBody;
    
    [Header("Sécurité Spawn")]
    [SerializeField] private float spawnImmunityTime = 0.08f; // Immunité de 0.08s au lancement

    private SphereCollider sphereCollider;
    private bool isExploding;
    private int damage;
    private float enableTimer;

    private void Awake()
    {
        isExploding = false;
    }

    private void Start()
    {
        if (explosion != null)
        {
            sphereCollider = explosion.gameObject.GetComponent<SphereCollider>();
        }

        Missile parentMissile = GetComponentInParent<Missile>();
        if (parentMissile != null)
        {
            damage = parentMissile.GetDamage();
        }
    }

    private void OnEnable()
    {
        isExploding = false;
        enableTimer = 0f; // Réinitialise le timer à l'activation

        if (missileBody != null)
        {
            missileBody.SetActive(true);
        }

        if (sphereCollider != null)
        {
            sphereCollider.enabled = false;
        }
    }

    private void Update()
    {
        // Incrémente le timer d'immunité du spawn
        enableTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore toute collision pendant la première fraction de seconde après le tir
        if (enableTimer < spawnImmunityTime) return;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (!isExploding)
        {
            ExecuteExplosion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore les triggers du joueur ou les contacts trop proches du spawn
        if (other.CompareTag("Player")) return;

        if (!isExploding)
        {
            if (enableTimer < spawnImmunityTime) return;

            ExecuteExplosion();
            return;
        }

        // Application des dégâts de zone si l'explosion est déjà active
        if ((other.gameObject.CompareTag("Alien") || other.gameObject.CompareTag("AlienSpawn")) && isExploding)
        {
            Health healthComponent = other.gameObject.GetComponent<Health>();
            if (healthComponent != null)
            {
                DamageInfo damageInfo = new DamageInfo(damage, transform.position, Vector3.up, transform.root.gameObject);
                healthComponent.TakeDamage(damageInfo);
            }
        }
    }

    private void ExecuteExplosion()
    {
        isExploding = true;

        Rigidbody rb = GetComponentInParent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        if (sphereCollider != null)
        {
            sphereCollider.enabled = true;
        }

        if (missileBody != null)
        {
            missileBody.SetActive(false);
        }

        StartCoroutine(PlayExplosionParticules());
    }

    private IEnumerator PlayExplosionParticules()
    {
        if (explosion != null)
        {
            explosion.Play();
            yield return new WaitForSeconds(explosion.main.duration);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        if (sphereCollider != null)
        {
            sphereCollider.enabled = false;
        }

        transform.root.gameObject.SetActive(false);
    }
}