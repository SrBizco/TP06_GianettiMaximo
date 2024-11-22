using UnityEngine;

public class TripleJump : MonoBehaviour
{
    [SerializeField] private float duration = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivateTripleJump(duration);
            }

            Destroy(gameObject);
        }
    }
}