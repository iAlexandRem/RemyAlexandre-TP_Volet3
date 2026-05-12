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
    public Transform[] pointsRetour; // Un point de retour (trou) où le joueur revient (et les autres seront aléatoires pour les billes et le sucre)
    bool ignoreProchainTrou = false; // Pour ignorer temporairement le trigger du trou de sortie

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
        if (SceneManager.GetActiveScene().name == "Mini-Jeu2" && LimitesPositionCamera != null) // Juste pour essayer une transition "fluide" des limites de position caméra quand on sort du périmètre du labyrinthe
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
                    return; // Si on vient juste d'être téléporté, je dois éviter le reste de la fonction en-dessous, pour pas de tombée double au trou de sortie
                }
                ignoreProchainTrou = true;

                anim.SetTrigger("estTombee"); // La fourmi tombe dans un trou  

                if (CompareTag("Player"))
                {
                    deplacement.peutBouger = false; // Déplacement bloqué du joueur
                }

                Vector2 dir = ((Vector2)collision.transform.position - (Vector2)transform.position).normalized; // Direction d'un vecteur qui pointe du sujet vers le trou
                if (CompareTag("Sucre"))
                {
                    rb.AddForce(dir * 10f, ForceMode2D.Impulse); // Une force vers le centre du trou
                }
                else
                {
                    rb.AddForce(dir * 40f, ForceMode2D.Impulse); // Une force vers le centre du trou
                }

                //Debug.Log("Dans Trou");
                Invoke("ReviensSurface", 0.5f);
            }


            if (collision.CompareTag("HorsLabyrinthe") && camAnimator != null && CompareTag("Player")) // Quand on s'aventure hors labyrinthe
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

            if (collision.CompareTag("HorsLabyrinthe") && camAnimator != null && CompareTag("Player")) // Quand on revient dedans le labyrinthe
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

        if (collision.gameObject.CompareTag("LimiteDroite") && CompareTag("Player"))
        {
            if (leJeu != null && leJeu.vocalInstructionsTerminees) // Attendre que les instructions soient finies
            {
                audioSource.PlayOneShot(vocalMauvaiseDirectionDroite); // Je pense qu'on devrait aller à gauche
            }
        }
        else if (collision.gameObject.CompareTag("LimiteGauche") && CompareTag("Player"))
        {
            audioSource.PlayOneShot(vocalMauvaiseDirectionGauche); // Je pense qu'on devrait aller à droite
        }
    }



    void ReviensSurface() // Téléportation
    {
        if (pointsRetour == null || pointsRetour.Length == 0) // Si le tableau de trous n'existe pas ou est vide
        {
            return; // Pas de téléportation
        }

        if (CompareTag("Player"))
        {
            transform.position = pointsRetour[0].position; // Le joueur revient au trou 0 en tombant dans les trous

            transform.rotation = Quaternion.Euler(0f, 0f, 45f); // Différent angle de rotation à la remontée

            rb.AddForce(new Vector2(-1f, -1f).normalized * 60f, ForceMode2D.Impulse); // Petite force en diagonale sud-ouest
        }
        else
        {
            int index = Random.Range(1, pointsRetour.Length); // Un trou aléatoire (excepté le 0) parmi ceux de l'array
            transform.position = pointsRetour[index].position; // La chose est transportée à ce trou
            if (CompareTag("Sucre"))
            {
                rb.AddForce(new Vector2(-1f, -1f).normalized * 67f, ForceMode2D.Impulse); // Grosse force hehe
            }
        }

        anim.SetTrigger("vaRemonter"); // Le sujet remonte du trou

        Invoke("PostRemontee", 0.7f);
    }

    void PostRemontee()
    {
        if (CompareTag("Player"))
        {
            deplacement.peutBouger = true; // Déplacement permis
            anim.speed = 5f; // Stress de la fourmi sur le idle
        }
        ignoreProchainTrou = false; // Je réactive le trigger du trou ici, le délai est important, j'ai testé
    }

}
