using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform background1;
    [SerializeField] private Transform background2;
    [SerializeField] private float repositionOffset = 1f;
    
    private float backgroundWidth; 
    private Transform cameraTransform; 
    

    void Start()
    {
        cameraTransform = Camera.main.transform;

        SpriteRenderer sr = background1.GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
        {
            backgroundWidth = sr.bounds.size.x;
        }
        else
        {
            Debug.LogError("No se encontró SpriteRenderer en el fondo 1.");
        }
    }

    void Update()
    {
        if (cameraTransform.position.x >= background2.position.x - repositionOffset)
        {
            background1.position = background2.position + new Vector3(backgroundWidth, 0, 0);

            Transform temp = background1;
            background1 = background2;
            background2 = temp;
        }
    }
}