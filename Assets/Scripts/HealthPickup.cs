using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healthAmount = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Heal(healthAmount);
                Destroy(gameObject);
            }
        }
    }
}