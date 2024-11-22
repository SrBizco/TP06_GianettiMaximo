using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float enemyDamage = 1f;
    [SerializeField] private float playerDamage = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(enemyDamage);
                Destroy(gameObject);
            }
        }
        
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(playerDamage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}