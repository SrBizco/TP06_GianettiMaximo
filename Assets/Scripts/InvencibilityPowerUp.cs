using UnityEngine;

public class InvencibilityPowerUp : MonoBehaviour
{
    [SerializeField] private float invincibilityDuration = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivateInvincibility(invincibilityDuration);
            }

            
            Destroy(gameObject);
        }
    }
}