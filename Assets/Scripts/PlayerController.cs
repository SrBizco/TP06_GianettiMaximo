using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform arm;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float currentHealth;
    [SerializeField] private int coinCount = 0;
    [SerializeField] private int ammoCount;
    [SerializeField] private int maxAmmo;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Slider healthBar;

    private int jumpCount;
    private bool tripleJumpActive = false;
    private float tripleJumpTimer = 0f;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    
    public float maxHealth = 10f;

    private Rigidbody2D rb;
    private Animator animator;
    private UIManager uiManager;

    
    private int facingDirection = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
        
        currentHealth = maxHealth;
        ammoCount = maxAmmo;
        jumpCount = maxJumpCount;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            Move();
            Jump();
            AimAtMouse();

            if (Input.GetButtonDown("Fire1") && ammoCount > 0)
            {
                Shoot();
            }
        }

        if (tripleJumpActive)
        {
            tripleJumpTimer -= Time.deltaTime;
            if (tripleJumpTimer <= 0)
            {
                DeactivateTripleJump();
            }
        }

        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                DeactivateInvincibility();
            }
        }
        UpdateHealthBar();
        UpdateCoinText();

        animator.SetFloat("XVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("YVelocity", rb.velocity.y);
        
        ammoText.text = ammoCount.ToString();
        
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput > 0)
        {
            facingDirection = 1;
            transform.localScale = new Vector3(5, 5, 5);
        }
        else if (moveInput < 0)
        {
            facingDirection = -1;
            transform.localScale = new Vector3(-5, 5, 5);
        }
    }

    private void Jump()
    {
        if (jumpCount > 0 && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioManager.instance.PlaySFX(AudioManager.instance.jumpSFX);

            animator.SetBool("isJumping", true);

            jumpCount--;
        }
    }

    private void AimAtMouse()
    {
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        
        Vector2 direction = (mousePosition - arm.position).normalized;

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        
        if (facingDirection == 1)
        {
            arm.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (facingDirection == -1)
        {
            
            arm.rotation = Quaternion.Euler(0, 0, angle + 180);
        }
    }
    private void Shoot()
    {
        
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();

        if (facingDirection == 1)
        {
            rbProjectile.velocity = firePoint.right * projectileSpeed;
        }
        else if (facingDirection == -1)
        {
            rbProjectile.velocity = -firePoint.right * projectileSpeed;
        }

        
        ammoCount--;
        AudioManager.instance.PlaySFX(AudioManager.instance.shootSFX);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            
            animator.SetBool("isJumping", false);
            jumpCount = maxJumpCount;
        }
    }

    public void ActivateInvincibility(float duration)
    {
        isInvincible = true;
        invincibilityTimer = duration;

        
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0.5f);
        AudioManager.instance.PlaySFX(AudioManager.instance.InvencibilitySFX);
    }

    private void DeactivateInvincibility()
    {
        isInvincible = false;

        
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void TakeDamage(float damage)
    {
        if (isInvincible) return;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
           
            AudioManager.instance.PlaySFX(AudioManager.instance.defeatMusic);
            GameOver();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        AudioManager.instance.PlaySFX(AudioManager.instance.healItemSFX);
    }
    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth / maxHealth;
    }
    private void UpdateCoinText()
    {
        coinText.text = coinCount.ToString();
    }

    public void AddAmmo(int amount)
    {
        ammoCount += amount;
        ammoCount = Mathf.Clamp(ammoCount, 0, maxAmmo);
        Debug.Log("Munición añadida: " + amount);
        AudioManager.instance.PlaySFX(AudioManager.instance.ammoItemSFX);
    }
    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinText();
        AudioManager.instance.PlaySFX(AudioManager.instance.CoinSFX);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Fall"))
        {
            GameOver();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Victory"))
        {
            Victory();
        }
    }

    public void ActivateTripleJump(float duration)
    {
        tripleJumpActive = true;
        tripleJumpTimer = duration;

        maxJumpCount = 3;
        jumpCount = maxJumpCount;
        AudioManager.instance.PlaySFX(AudioManager.instance.TripleJumpSFX);
    }

    private void DeactivateTripleJump()
    {
        tripleJumpActive = false;
        maxJumpCount = 2;
        jumpCount = maxJumpCount;
    }
    private void GameOver()
    {
        Time.timeScale = 0;
        uiManager.ToggleDefeat();
    }
    private void Victory()
    {
        Time.timeScale = 0;
        uiManager.ToggleVictory();
    }
}