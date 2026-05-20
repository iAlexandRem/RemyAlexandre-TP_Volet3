using UnityEngine;

public class MouvementBillesEnnemies : MonoBehaviour
{
    public float vitesse;
    public float acceleration;
    public float forcePoussee;
    public float distanceActivation; // Jusqu'à où la bille détecte le joueur
    public float tempsRebond; // Temps avant de changer de direction après collision
    private float vitesseActuelle; // Pour les transitions de vitesse

    public static bool BilleToucheFourmi = false; // Je réutilise dans le script SucreBrun
    public bool collisionBilleFourmi = false;

    Rigidbody2D rb;
    private Transform joueur;
    private float timerRebond = 0f; // Pour un countdown à chaque rebond
    public LeJeu jeu; // Pour utiliser des bools comme vocalInstructionsTerminees ou autreVocalQuiJoue
    AudioSource audioSource;
    public AudioClip vocalEviteBillesEnMetal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        joueur = GameObject.FindGameObjectWithTag("Player").transform; // Trouve la fourmi
        vitesseActuelle = vitesse;
        BilleToucheFourmi = false;
    }

    void FixedUpdate()
    {
        if (timerRebond > 0)
        {
            timerRebond -= Time.fixedDeltaTime; // Décrémente timer
        }

        Vector2 dir; // Direction de déplacement

        Vector2 dirJoueur = ((Vector2)joueur.position - (Vector2)transform.position).normalized; // Direction vers le joueur
        float dist = Vector2.Distance(transform.position, joueur.position); // Distance au joueur

        // Si enfin pas de rebond ET si le joueur est proche 
        if (timerRebond <= 0 && dist <= distanceActivation)
        {
            dir = Vector2.Lerp(rb.linearVelocity.normalized, dirJoueur, acceleration * Time.fixedDeltaTime); // Virage progressif vers le joueur
        }
        else
        {
            dir = rb.linearVelocity.normalized; // Sinon on garde la direction actuelle

            if (rb.linearVelocity.magnitude < 0.1f) // Si la bille ne bouge presque plus
            {
                float angle = Random.Range(0f, 360f); // Angle aléatoire en degrés
                dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)); // Direction aléatoire 
            }
        }

        vitesseActuelle *= 0.999f; // Perte progressive de vitesse
        vitesseActuelle = Mathf.Min(vitesseActuelle + acceleration * Time.fixedDeltaTime, vitesse); // Augmente progressivement la vitesseActuelle (accélération), mais la bloque pour ne jamais dépasser la valeur max "vitesse"
        rb.linearVelocity = dir.normalized * vitesseActuelle; // Application de la vitesse
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // À chaque fois que la bille rentre en contact avec le joueur
        {
            if (!BilleToucheFourmi)
            {
                if (jeu != null && jeu.MessageDebutFini && !LeJeu.autreVocalQuiJoue) // Attendre que les instructions soient finies
                {
                    BilleToucheFourmi = true;
                    audioSource.PlayOneShot(vocalEviteBillesEnMetal, 1.5f); // Avertissement des billes au premier contact
                    LeJeu.autreVocalQuiJoue = true;
                    Invoke("AucunVocalJoue", 6f);
                }
            }
            collisionBilleFourmi = true;

            GameObject[] trous = GameObject.FindGameObjectsWithTag("Trou"); // Ça met tous les gameobjects Trous dans un array [trous]

            GameObject plusProche = trous[1]; // Je veux repérer le trou le plus proche au moment de la collision, on commence par un premier
            float minDist = Vector2.Distance(collision.transform.position, plusProche.transform.position); // Distance minimale entre le joueur touché et ce trou (qu'on considère proche)
            foreach (GameObject t in trous) // Pour CHAQUE trou (t) dans l'array
            {
                float d = Vector2.Distance(collision.transform.position, t.transform.position); // Calcul distance entre joueur et ce trou
                if (d < minDist) // Si cette distance est la plus petite jusqu'à présent
                {
                    plusProche = t; // Mise à jour que ce trou t est le plus proche
                    minDist = d; // Nouvelle distance minimale pour comparer avec les autres trous de l'array
                }
            }

            Vector2 dir = ((Vector2)plusProche.transform.position - (Vector2)collision.transform.position).normalized; // Une dir allant du joueur vers le trou
            collision.rigidbody.linearVelocity = dir * forcePoussee; // LA BILLE POUSSE LE JOUEUR VERS LE TROU!
        }


        if (collision.gameObject.CompareTag("Bois")) // Collision rebond sur les blocs de bois
        {
            timerRebond = tempsRebond; // Reset du countdown

            vitesseActuelle *= 0.3f; // Perte temporaire de vitesse 

            float angle = Random.Range(0f, 360f); // Angle aléatoire
            Vector2 dirAleatoire = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)); // Direction aléatoire

            rb.linearVelocity = dirAleatoire * rb.linearVelocity.magnitude * 0.3f; // Rebond selon direction aléatoire
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collisionBilleFourmi = false;
        }
    }

    void OnTriggerStay2D(Collider2D fuite) // Tant que la bille est dedans
    {
        if (fuite.CompareTag("ZoneProtegee")) // Pour que la bille fuit certaines zones dites protégées (DÉBUT, FIN)
        {
            Vector2 dir = ((Vector2)transform.position - fuite.ClosestPoint(transform.position)).normalized; // Une dir allant du point le plus près du collider vers la bille
            rb.linearVelocity = dir * rb.linearVelocity.magnitude * 1.5f; // Vitesse de poussée constante selon cette direction de fuite
        }
    }


    void AucunVocalJoue()
    {
        jeu.SonEstLibre();
    }
}
