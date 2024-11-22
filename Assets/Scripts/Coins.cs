using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                
                player.AddCoin(coinValue);
            }

            
            Destroy(gameObject);
        }
    }
}