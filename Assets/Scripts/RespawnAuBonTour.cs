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
    public bool dropJoueurDuHautGrilleDetecte = false;
    Animator anim;
    AudioSource audioSource;
    public AudioClip sfxNouvelleCocci;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        anim.speed = 1f; // Vitesse normale du spawnPointAdversaire en translation
    }

    // Update is called once per frame
    void Update()
    {
        if (partie.aGagne && partie.autoriseInfestation) // Coccis tombent de partout avec victoire
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
            drag.Spawn(); // Infestation de coccinelles
        }
    }

    public void JoueurCocciDroppedDansGrille() // Dès que le joueur échappe sa cocci dans la grille, voir les collisions triggers dans ColliderConnect4
    {
        if (!aSpawnCeTour) return;

        dropJoueurDuHautGrilleDetecte = true;

        aSpawnCeTour = false; // Bloque les spams, un adversaire ne va pas spawn tant que c'est le joueur qui n'aura pas drop sa cocci dans la grille

        if (!drag.dropDepuisHautGrille) // Il faut que cocci tombe dedans
        {
            if (partie.aGagne) return;
            Invoke("SpawnAdversaire", 2f); // Tour adverse
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

        anim.SetTrigger("Translation"); // Point de tombée en mouvement
        drag.spawnPointActuel = spawnPointAdversaire; // À un pointX aléatoire qui est parfois en animation au-dessus de la grille
        if (dropJoueurDuHautGrilleDetecte && !PartieConnect4.tourJoueur && !partie.coupRetire)
        {
            drag.Spawn(); // Clone adverse qui est en chute libre
        }

        if (!aSpawnCeTour && !drag.dropDepuisHautGrille) // Il faut que cocci tombe dedans
        {
            anim.speed = 0f;
            Invoke("RespawnJoueur", 6f);
        }
    }


    public void RespawnJoueur() // Une cocci de plus pour le joueur
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


        float value;
        if (Random.value < 0.75f) // + de chance que spawnPointAdversaire se déplace lentement que rapidement
        {
            value = Random.Range(0.01f, 0.15f);

            Invoke("StopTranslationX", 1f);
        }
        else
        {
            value = Random.Range(0.2f, 0.6f);

            Invoke("LegereTranslationX", 4f);
        }

        anim.speed = value;



        if (!partie.aGagne)
        {
            SpawnCocciSFX();
            drag.Spawn(); // Clone pour le joueur
            aSpawnCeTour = true; // Reset
        }
        else
        {
            anim.SetTrigger("Translation");
            anim.speed = 4f;
        }
        dropJoueurDuHautGrilleDetecte = false; // Reset
    }

    public void SpawnCocciSFX()
    {
        if (partie.aGagne)
        {
            audioSource.PlayOneShot(sfxNouvelleCocci, 0.4f);
        }
        else
        {
            audioSource.PlayOneShot(sfxNouvelleCocci);
        }
    }

    public void StopTranslationX()
    {
        anim.SetTrigger("Stop"); // Point d'adversaire s'arrête
        anim.speed = 0;
    }

    public void LegereTranslationX()
    {
        anim.SetTrigger("Translation"); // Point d'adversaire est en translation
        anim.speed = 0.4f;
        Invoke("StopTranslationX", 1f);
    }




    public void RespawnAdversaire() // Au cas-où que l'adversaire ne veut pas spawn du tout
    {
        if (partie.couleurChoisie == 1)
        {
            drag.prefabCocci = drag.prefabJaune;
        }
        else if (partie.couleurChoisie == 2)
        {
            drag.prefabCocci = drag.prefabRouge;
        }

        drag.spawnPointActuel = spawnPointAdversaire;
        drag.Spawn(); // Sans respawn le joueur
    }
}
