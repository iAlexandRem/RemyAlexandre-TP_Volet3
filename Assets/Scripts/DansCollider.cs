using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DansCollider : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    public Animator camAnimator; // Pour glisser l'Animator de la CinemachineCamera 
    public GameObject LimitesPositionCamera; // Pour modifier son scale lorsqu'on est hors labyrinthe
    public Vector3 hausseScaleLimites; // Par l'Inspector
    private Vector3 initialScaleLimites;
    private Vector3 targetScaleLimites; // Facteur d'aggrandissement cible
    private float smoothSpeed = 0.5f; // Vitesse de transition du scale pour le lerp dans Update


    Vector2 positionDepart;
    public DeplacementParFleches deplacement; // Pour gérer l'arrêt de déplacement par flèches, avec le bool peutBouger
    public Transform pointRetour; // Un point de retour où le joueur revient
    bool ignoreProchainTrou = false; // Pour ignorer le trigger du trou de sortie

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

        if (LimitesPositionCamera != null)
        {
            initialScaleLimites = LimitesPositionCamera.transform.localScale;
            targetScaleLimites = initialScaleLimites;
        }
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Mini-Jeu2") // Juste pour essayer une transition "fluide" des limites de position caméra quand on sort du périmètre du labyrinthe
        {
            LimitesPositionCamera.transform.localScale = Vector3.Lerp(LimitesPositionCamera.transform.localScale, targetScaleLimites, Time.deltaTime * smoothSpeed);
        }
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
                if (ignoreProchainTrou)
                {
                    ignoreProchainTrou = false;
                    return; // Si on vient juste d'être téléporté, je dois éviter le reste de la fonction en-dessous, pour pas de tombée double
                }
                ignoreProchainTrou = true;

                anim.SetTrigger("estTombee"); // La fourmi tombe dans un trou  

                deplacement.peutBouger = false; // Déplacement bloqué

                Vector2 dir = ((Vector2)collision.transform.position - (Vector2)transform.position).normalized; // Direction d'un vecteur qui pointe de la fourmi vers le trou
                rb.AddForce(dir * 10f, ForceMode2D.Impulse); // Une force vers le centre du trou

                //Debug.Log("Dans Trou");
                Invoke("ReviensSurface", 0.5f);
            }


            if (collision.CompareTag("HorsLabyrinthe") && camAnimator != null) // Quand on s'aventure hors labyrinthe
            {
                camAnimator.SetTrigger("CamZoom"); // Plan rapproché

                targetScaleLimites = Vector3.Scale(initialScaleLimites, hausseScaleLimites); // + grande zone pour les limites de caméra

                rb.angularDamping = 0f;
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
                //Debug.Log("Hors Trou");
            }

            if (collision.CompareTag("HorsLabyrinthe") && camAnimator != null) // Quand on revient dedans le labyrinthe
            {
                camAnimator.SetTrigger("CamDezoom"); // Plan moins rapproché

                targetScaleLimites = initialScaleLimites; // Scale initial pour les limites de caméra

                rb.angularDamping = 42f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (audioSource.isPlaying) return; // Éviter le spam sonore

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
        transform.position = pointRetour.position; // On revient au trou zéro en tombant dans les trous

        transform.rotation = Quaternion.Euler(0f, 0f, 45f); // Différent angle de rotation à la remontée

        anim.SetTrigger("vaRemonter"); // La fourmi remonte du trou

        rb.AddForce(new Vector2(-1f, -1f).normalized * 15f, ForceMode2D.Impulse); // Petite force en diagonale sud-ouest

        Invoke("PostRemontee", 0.7f);
    }

    void PostRemontee()
    {
        deplacement.peutBouger = true; // Déplacement permis
        anim.speed = 5f; // Stress de la fourmi sur le idle
        ignoreProchainTrou = false; // Je réactive le trigger du trou ici, le délai est important, j'ai testé
    }

}
