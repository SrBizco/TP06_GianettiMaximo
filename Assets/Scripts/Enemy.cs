using UnityEngine;
using UnityEngine.UI; 

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f; 
    private float currentHealth; 
    private Slider healthBar; 
    [SerializeField] private GameObject particleSystemPrefab; 
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private Transform shootPoint; 
    [SerializeField] private float detectionRange = 10f; 
    [SerializeField] private float shootInterval = 1f; 
    [SerializeField] private float projectileSpeed = 5f; 

    private float shootTimer;
    private int facingDirection = 1; 

    void Start()
    {
        currentHealth = maxHealth; 
        healthBar = GetComponentInChildren<Slider>(); 
        UpdateHealthBar(); 
    }

    void Update()
    {
        DetectPlayerAndShoot();
    }
    private void DetectPlayerAndShoot()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));
        if (playerCollider != null)
        {
            FacePlayer(playerCollider.transform);

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot(playerCollider.transform);
                shootTimer = shootInterval;
            }
        }
    }
    
    private void FacePlayer(Transform playerTransform)
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        if (direction.x > 0 && facingDirection != 1)
        {
            facingDirection = 1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0 && facingDirection != -1)
        {
            facingDirection = -1;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    private void Shoot(Transform playerTransform)
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        Vector2 direction = (playerTransform.position - shootPoint.position).normalized;

        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
        rbProjectile.velocity = direction * projectileSpeed;
        AudioManager.instance.PlaySFX(AudioManager.instance.shootSFX); 
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar(); 
        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.enemyDeathSFX);
            Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth / maxHealth;
    }
}