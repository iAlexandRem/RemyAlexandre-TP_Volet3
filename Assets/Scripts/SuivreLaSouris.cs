using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class SuivreLaSouris : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer[] sprites;
    Animator anim;
    AudioSource audioSource;
    public AudioClip sfxFlip;
    float dernierFlip;
    bool sonFlip = false;


    public float vitesseDeplacement; // 3f
    float directionX;
    bool enDeplacementX;
    GameObject dernierTrigger; // Dernier trigger de PlateformeInvisible

    public HoverBoutons hoverBoutons; // Pour empêcher le déplacement quand on hover des boutons


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        vitesseDeplacement = 3f;
    }


    void Update()
    {
        if (Mouse.current.leftButton.isPressed) // Tant que le CLIC gauche reste APPUYÉ
        {
            float zoneMorte = 1f; // Zone ajustable autour du centre du sujet, pour essayer d'éviter les alternations trop rapides gauches-droites si la souris clique près de celui-ci

            Vector2 PosSouris = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            float diffX = PosSouris.x - transform.position.x; // Différence positive ou négative

            if (Mathf.Abs(diffX) > zoneMorte) // La direction (gauche ou droite) est obtenue seulement si la différence est assez grande, donc si le clic est assez éloigné du centre du sujet
            {
                if (diffX > 0) //Clic à droite 
                {
                    directionX = 1; //Direction du sujet vers la droite

                    sonFlip = true; // Le premier clic ne doit pas sonner comme un flip, alors je mets cette ligne ici
                }
                else if (diffX < 0) //Clic à gauche
                {
                    directionX = -1; //Direction du sujet vers la gauche
                }
            }

            enDeplacementX = true; //Déplacement
            anim.SetBool("enDeplacement", true); // Pour set l'Anim


            if (sonFlip == true)
            {
                float dir = Mathf.Sign(directionX);
                if (dir != 0 && dir != dernierFlip)
                    audioSource.PlayOneShot(sfxFlip, 2f); // Juste pour éviter le bug du son
                dernierFlip = dir;
            }
        }

        else
        {
            enDeplacementX = false; //Aucun déplacement
            anim.SetBool("enDeplacement", false); // Retour à l'idle
        }


        if (directionX < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Flip vers la gauche
        }
        else if (directionX > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip vers la droite
        }


        if (hoverBoutons != null && hoverBoutons.hoverUI)
        {
            enDeplacementX = false; //Aucun déplacement si on survole le UI
        }

    }



    void FixedUpdate()
    {
        Vector2 vel = rb.linearVelocity; // Pour manipuler le .x comme vecteur

        if (enDeplacementX)
        {
            vel.x = directionX * vitesseDeplacement; // De la vitesse dans la directionX de la souris
        }
        else
        {
            vel.x = 0f;
        }

        rb.linearVelocity = vel; // Ligne obligatoire, pour appliquer les changements au Rigidbody
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeclencheurPlateforme"))
        {
            // Si c'est un nouveau trigger
            if (collision.gameObject != dernierTrigger)
            {
                vitesseDeplacement += 0.42f; // La chenille va un peu plus rapidement à chaque étage plus haut

                // On mémorise ce trigger
                dernierTrigger = collision.gameObject;
            }
        }
    }
}
