using UnityEngine;

public class SucreBrun : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("PetitePoussee", 3f); // Délai
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PetitePoussee()
    {
        rb.AddForce(Vector2.down * 17f, ForceMode2D.Impulse); // Poussée automatique vers le bas
    }
}
