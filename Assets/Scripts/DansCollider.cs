using UnityEngine;

public class DansCollider : MonoBehaviour
{
    Rigidbody2D rb;
    AudioSource audioSource;
    public AudioClip vocalMauvaiseDirectionDroite;
    public AudioClip vocalMauvaiseDirectionGauche;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision) // En RENTRANT dans le collider trigger 
    {
        CaseScript caseScript = collision.GetComponent<CaseScript>(); // Référence au script CaseScript

        if (caseScript != null)
        {
            caseScript.ActiverCase(); // Déclenchement de la fonction du script CaseScript
        }
    }

    void OnTriggerExit2D(Collider2D collision) // En SORTANT du collider trigger
    {
        CaseScript caseScript = collision.GetComponent<CaseScript>();

        if (caseScript != null)
        {
            caseScript.DesactiverCase();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LimiteDroite"))
        {
            audioSource.PlayOneShot(vocalMauvaiseDirectionDroite); // Je pense qu'on devrait aller à gauche
        }
        else if (collision.gameObject.CompareTag("LimiteGauche"))
        {
            audioSource.PlayOneShot(vocalMauvaiseDirectionGauche); // Je pense qu'on devrait aller à droite
        }
    }
}
