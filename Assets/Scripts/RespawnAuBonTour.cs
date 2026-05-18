using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnAuBonTour : MonoBehaviour // Au cas-où que les coccis ne respawn pas
{
    public DragCocci drag; // Référence au script DragCocci
    public PartieConnect4 partie; // Référence au script PartieConnect4
    public Transform spawnPointRouge;
    public Transform spawnPointJaune;
    public Transform spawnPointAdversaire;
    private bool aSpawnCeTour = true;
    private bool dropJoueurDuHautGrilleDetecte = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (partie.aGagne) // Coccis tombent de partout avec victoire
        {
            if (partie.couleurGagnante == 1)
            {
                drag.prefabCocci = drag.prefabRouge;
            }
            else if (partie.couleurGagnante == 2)
            {
                drag.prefabCocci = drag.prefabJaune;
            }
            drag.spawnPointActuel = spawnPointAdversaire;
            drag.Spawn(); // Chaos
        }
    }

    public void JoueurCocciDroppedDansGrille() // Dès que le joueur échappe sa cocci dans la grille, voir les collisions triggers dans ColliderConnect4
    {
        if (!aSpawnCeTour) return;

        dropJoueurDuHautGrilleDetecte = true;

        aSpawnCeTour = false; // Bloque les spams, un adversaire ne va pas spawn tant que c'est le joueur qui n'aura pas drop sa cocci dans la grille

        if (!drag.dropDepuisHautGrille) // Il faut que cocci tombe dedans
        {
            Invoke("SpawnAdversaire", 2f);
        }
    }


    // La tombée de l'adversaire 
    void SpawnAdversaire()
    {
        if (partie.couleurChoisie == 1)
        {
            drag.prefabCocci = drag.prefabJaune; // Couleur inverse
        }
        else if (partie.couleurChoisie == 2)
        {
            drag.prefabCocci = drag.prefabRouge; // Couleur inverse
        }

        drag.spawnPointActuel = spawnPointAdversaire; // À un pointX aléatoire en animation au-dessus de la grille
        if (dropJoueurDuHautGrilleDetecte)
        {
            drag.Spawn(); // Clone adverse qui est en chute libre
        }

        if (!aSpawnCeTour && !drag.dropDepuisHautGrille) // Il faut que cocci tombe dedans
        {
            Invoke("RespawnJoueur", 6f);
        }
    }


    public void RespawnJoueur()
    {
        if (partie.couleurChoisie == 1)
        {
            drag.prefabCocci = drag.prefabRouge;
            drag.spawnPointActuel = spawnPointRouge;
        }
        else if (partie.couleurChoisie == 2)
        {
            drag.prefabCocci = drag.prefabJaune;
            drag.spawnPointActuel = spawnPointJaune;
        }

        drag.Spawn(); // Clone pour le joueur

        aSpawnCeTour = true; // Reset
        dropJoueurDuHautGrilleDetecte = false; // Reset
    }
}
