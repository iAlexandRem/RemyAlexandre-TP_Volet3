using UnityEngine;

public class ColliderConnect4 : MonoBehaviour
{
    private TrouConnect4 trou;

    public PartieConnect4 partie; // Référence au script PartieConnect4
    public RespawnAuBonTour respawn; // Référence au script RespawnAuBonTour
    Collider2D col;
    private bool colEstTrue = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trou = GetComponent<TrouConnect4>(); // Recherche du script TrouConnect4 sur le même component
        col = GetComponent<Collider2D>();
        col.enabled = true;
        colEstTrue = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (partie.aGagne && colEstTrue)
        {
            col.enabled = false; // Collider2D désactivé
            colEstTrue = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // Détection des coccis par les trous de la grille
    {
        DragCocci drag = collision.GetComponent<DragCocci>(); // Référence au script de la coccinelle prefab

        if (!drag.dropDepuisHautGrille) return; // Si la coccinelle n'est pas lancée depuis le haut, les trous ne détectent rien quand on les survole avec la coccinelle en drag


        if (collision.CompareTag("CocciRouge"))
        {
            trou.OccuperTrouLePlusBas(); // Dans la colonne, le trou le plus haut est détecté par collision trigger, cette fonction indique que c'est le trou le plus bas qui doit être occupé!

            drag.dropDepuisHautGrille = false; // Le drop dans la grille est terminé

            partie.JouerCoup(trou.colonne); // Retenir la colonne du trou où la coccinelle est tombée 
            drag.coupEnregistre = true;

            respawn.JoueurCocciDroppedDansGrille(); // Le tour de l'adversaire de drop
        }

        if (collision.CompareTag("CocciJaune"))
        {
            trou.OccuperTrouLePlusBas(); // Dans la colonne, le trou le plus haut est détecté par collision trigger, cette fonction indique que c'est le trou le plus bas qui doit être occupé!

            drag.dropDepuisHautGrille = false; // Le drop dans la grille est terminé


            partie.JouerCoup(trou.colonne); // Retenir la colonne du trou où la coccinelle est tombée
            drag.coupEnregistre = true;

            respawn.JoueurCocciDroppedDansGrille(); // Le tour de l'adversaire de drop
        }
    }
}
