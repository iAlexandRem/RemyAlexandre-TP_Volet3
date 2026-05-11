using UnityEngine;

public class MouvementBillesEnnemies : MonoBehaviour
{
    public float vitesse;
    public float acceleration;
    public float forcePoussee;
    public float distanceActivation; // Jusqu'à où la bille détecte le joueur
    public float tempsRebond; // Temps avant de changer de direction après collision

    Rigidbody2D rb;
    private Transform joueur;
    private float timerRebond = 0f; // Pour un countdown à chaque rebond

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joueur = GameObject.FindGameObjectWithTag("Player").transform; // Trouve la fourmi
    }

    void FixedUpdate()
    {
        if (timerRebond > 0)
        {
            timerRebond -= Time.fixedDeltaTime; // Décrémente timer
        }

        Vector2 dir; // Direction de déplacement

        // Si enfin pas de rebond ET si le joueur est proche 
        if (timerRebond <= 0 && Vector2.Distance(transform.position, joueur.position) <= distanceActivation)
        {
            dir = ((Vector2)joueur.position - (Vector2)transform.position).normalized; // La dir est vers le joueur
        }
        else
        {
            dir = rb.linearVelocity.normalized; // Sinon on garde la directon actuelle

            if (rb.linearVelocity.magnitude < 0.1f) // Si la bille ne bouge presque plus
            {
                float angle = Random.Range(0f, 360f); // Angle aléatoire en degrés
                dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)); // Direction aléatoire 
            }
        }

        rb.linearVelocity = dir * vitesse; // Vitesse constante
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // À chaque fois que la bille rentre en contact avec le joueur
        {
            GameObject[] trous = GameObject.FindGameObjectsWithTag("Trou"); // On met tous les gameobjects Trous dans un array [trous]

            GameObject plusProche = trous[0]; // Je veux repérer le trou le plus proche AU MOMENT DE LA COLLISION, on commence par un premier
            float minDist = Vector2.Distance(collision.transform.position, plusProche.transform.position); // Distance minimale entre le joueur touché et ce trou (qu'on considère proche)
            foreach (GameObject t in trous) // Pour CHAQUE trou (t) dans l'array
            {
                float d = Vector2.Distance(collision.transform.position, t.transform.position); // Calcul distance entre joueur et ce trou
                if (d < minDist) // Si cette distance est plus petite jusqu'à présent
                {
                    plusProche = t; // On met à jour que ce trou t est le plus proche
                    minDist = d; // Nouvelle distance minimale pour comparer avec les autres trous de l'array
                }
            }

            Vector2 dir = ((Vector2)plusProche.transform.position - (Vector2)collision.transform.position).normalized; // Une dir allant du joueur vers le trou
            collision.rigidbody.linearVelocity = dir * forcePoussee; // La bille pousse le joueur vers le trou!
        }


        if (collision.gameObject.CompareTag("Bois")) // Collision rebond sur les blocs de bois
        {
            timerRebond = tempsRebond; // Reset du countdown

            float angle = Random.Range(0f, 360f); // Angle aléatoire
            Vector2 dirAleatoire = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)); // Direction aléatoire

            rb.linearVelocity = dirAleatoire * vitesse; // Vitesse constante selon direction aléatoire
        }
    }
}
