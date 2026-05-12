using UnityEngine;
using UnityEngine.InputSystem;

public class SucreBrun : MonoBehaviour
{
    Rigidbody2D rb;
    Transform joueur;

    bool estPorte = false;
    Vector2 offset = new Vector2(-1f, 0f);

    public float distancePrise = 1.5f; // distance pour pouvoir ramasser

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joueur = GameObject.FindGameObjectWithTag("Player").transform;
        Invoke("PetitePoussee", 3f); // Délai
    }


    void PetitePoussee()
    {
        rb.AddForce(Vector2.down * 17f, ForceMode2D.Impulse); // Poussée automatique vers le bas
    }


    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("SPACE détecté");
            float dist = Vector2.Distance(transform.position, joueur.position);

            // 👉 ON PEUT RAMASSER SEULEMENT SI PROCHE
            if (dist <= distancePrise)
            {
                estPorte = !estPorte;

                if (estPorte)
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (estPorte && joueur != null)
        {
            Vector2 target = (Vector2)joueur.position + offset;
            rb.MovePosition(target);
        }
    }
}
