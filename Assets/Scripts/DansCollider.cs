using UnityEngine;
using UnityEngine.SceneManagement;

public class DansCollider : MonoBehaviour
{
    Rigidbody2D rb;
    AudioSource audioSource;
    public AudioClip vocalMauvaiseDirectionDroite;
    public AudioClip vocalMauvaiseDirectionGauche;

    public LeJeu leJeu; // Pour utiliser le bool vocalInstructionsTerminees

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

        if (SceneManager.GetActiveScene().name == "Mini-Jeu1")
        {
            CaseScript caseScript = collision.GetComponent<CaseScript>(); // Référence au script CaseScript

            if (caseScript != null)
            {
                caseScript.ActiverCase(); // Déclenchement de la fonction du script CaseScript
            }
        }


        else if (SceneManager.GetActiveScene().name == "Mini-Jeu2")
        {
            if (collision.CompareTag("Trou"))
            {
                Debug.Log("Dans Trou");
            }

        }

    }

    void OnTriggerExit2D(Collider2D collision) // En SORTANT du collider trigger
    {
        if (SceneManager.GetActiveScene().name == "Mini-Jeu1")
        {
            CaseScript caseScript = collision.GetComponent<CaseScript>();

            if (caseScript != null)
            {
                caseScript.DesactiverCase();
            }
        }

        else if (SceneManager.GetActiveScene().name == "Mini-Jeu2")
        {
            if (collision.CompareTag("Trou"))
            {
                Debug.Log("Hors Trou");
            }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LimiteDroite"))
        {
            if (leJeu != null && leJeu.vocalInstructionsTerminees) // Attendre que les instructions soient finies
            {
                audioSource.PlayOneShot(vocalMauvaiseDirectionDroite); // Je pense qu'on devrait aller à gauche
            }
        }
        else if (collision.gameObject.CompareTag("LimiteGauche"))
        {
            audioSource.PlayOneShot(vocalMauvaiseDirectionGauche); // Je pense qu'on devrait aller à droite

        }
    }
}
