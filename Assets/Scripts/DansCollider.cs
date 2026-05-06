using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DansCollider : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    Vector2 positionDepart;
    public DeplacementParFleches deplacement; // Pour gérer l'arrêt de déplacement par flèches
    public Transform pointRetour; // Un point de retour où le joueur revient
    AudioSource audioSource;
    public AudioClip vocalMauvaiseDirectionDroite;
    public AudioClip vocalMauvaiseDirectionGauche;

    public LeJeu leJeu; // Pour utiliser le bool vocalInstructionsTerminees


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        positionDepart = transform.position;
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
                anim.SetTrigger("estTombee"); // La fourmi tombe dans un trou
                deplacement.peutBouger = false;

                Vector2 dir = ((Vector2)collision.transform.position - (Vector2)transform.position).normalized; // Direction d'un vecteur qui pointe de la fourmi vers le trou
                rb.AddForce(dir * 10f, ForceMode2D.Impulse); // Une force vers le trou
                rb.linearDamping = 5f; // Force moins brusque
                //Debug.Log("Dans Trou");

                Invoke("ReviensSurface", 1f);
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
                rb.linearDamping = 25f; // Pour éviter toute flottaison en physique par après
                anim.speed = 1f;
                //Debug.Log("Hors Trou");
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



    void ReviensSurface()
    {
        anim.SetTrigger("vaRemonter"); // La fourmi remonte du trou
        deplacement.peutBouger = true;

        transform.position = pointRetour.position; // On revient au trou zéro en tombant dans les trous
        rb.AddForce(Vector2.down * 10f, ForceMode2D.Impulse); // Petite force vers le bas

        Invoke("StressAnim", 1f);
    }

    void StressAnim()
    {
        anim.speed = 5f; // Stress de la fourmi sur le idle
    }

}
