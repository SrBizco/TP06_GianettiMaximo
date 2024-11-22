using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private int ammoAmount = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddAmmo(ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}