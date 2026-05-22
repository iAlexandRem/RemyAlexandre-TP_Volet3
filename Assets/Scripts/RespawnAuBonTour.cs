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

    public bool EssaieDeBloquerChuteCocciDuCiel = false; // Je dois bloquer l'adversaire s'il vient, si ta cocci n'est pas restée dans la grille au drop (partie.coupRetire = true), sinon la partie se ruine carrément, et je n'ai pas su encore comment empêcher le spawn de la cocci en chute libre à ce moment-là
    public GameObject cOLLISIOnInViSiBle;
    bool blocageAEteNecessaire = false;

    Animator anim;
    AudioSource audioSource;
    public AudioClip sfxNouvelleCocci;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cOLLISIOnInViSiBle.SetActive(false);
        EssaieDeBloquerChuteCocciDuCiel = false;
        blocageAEteNecessaire = false;
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        anim.speed = 1f; // Vitesse normale du spawnPointAdversaire en translation
    }

    // Update is called once per frame
    void Update()
    {
        if (partie.aGagne && partie.autoriseInfestation) // CHAOS DE COCCINELLES, après victoire
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


        if (EssaieDeBloquerChuteCocciDuCiel && !blocageAEteNecessaire)
        {
            blocageAEteNecessaire = true;
            cOLLISIOnInViSiBle.SetActive(true); // Méthode non orthodoxe
            Invoke("ResetBlocage", 3f);
        }
    }

    public void JoueurCocciDroppedDansGrille() // Dès que le joueur échappe sa cocci dans la grille, voir les collisions triggers dans ColliderConnect4
    {
        if (!aSpawnCeTour) return;

        dropJoueurDuHautGrilleDetecte = true;

        if (!drag.dropDepuisHautGrille) // Il faut que cocci tombe dedans
        {
            if (partie.aGagne) return;
            aSpawnCeTour = false; // Bloque les spams je crois, un adversaire ne va pas spawn tant que c'est le joueur qui n'aura pas drop sa cocci dans la grille
            Invoke("SpawnAdversaire", 2f); // Tour adverse
        }
    }


    // La tombée de l'adversaire 
    public void SpawnAdversaire()
    {
        if (partie.couleurChoisie == 1)
        {
            drag.prefabCocci = drag.prefabJaune; // Couleur inverse
        }
        else if (partie.couleurChoisie == 2)
        {
            drag.prefabCocci = drag.prefabRouge; // Couleur inverse
        }

        drag.spawnPointActuel = spawnPointAdversaire; // À un pointX aléatoire qui est parfois en animation au-dessus de la grille
        if (dropJoueurDuHautGrilleDetecte)
        {
            drag.Spawn(); // Clone adverse qui est en chute libre
        }

        if (!aSpawnCeTour && !drag.dropDepuisHautGrille) // Il faut que cocci tombe dedans
        {
            anim.speed = 0f;
            Invoke("RespawnJoueur", 6f);
        }
        else if (partie.coupRetire)
        {
            Invoke("RespawnJoueur", 3f);
            partie.coupRetire = false;
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

        
        // Randomness avec le point de spawn pour les coccis adverses
        float value; 

        if (Random.value < 0.77f) // + de chance que spawnPointAdversaire se déplace lentement que rapidement
        {
            value = Random.Range(0.01f, 0.06f);

            value *= Random.Range(0.8f, 1.1f);

            Invoke("StopTranslationX", Random.Range(0.8f, 1.6f));
        }
        else
        {
            value = Random.Range(0.08f, 0.15f); // un peu de boost

            value *= Random.Range(0.9f, 1.1f);

            Invoke("LegereTranslationX", Random.Range(0.1f, 1f));
        }



        if (!partie.aGagne)
        {
            if (partie.coupRetire) return;
            SpawnCocciSFX();
            drag.Spawn(); // Clone pour le joueur
            aSpawnCeTour = true; // Reset de bool
        }
        else
        {
            CancelInvoke();
            anim.speed = 1f;
        }

        dropJoueurDuHautGrilleDetecte = false; // Reset de bool
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
        anim.speed = 0;
    }

    public void LegereTranslationX() // Point d'adversaire est en translation
    {
        anim.speed = 0.1f;
        Invoke("StopTranslationX", 1f);
    }




    public void RespawnAdversaire()
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
        drag.Spawn(); // Une cocci d'en haut sans respawn le joueur
    }


    void ResetBlocage()
    {
        cOLLISIOnInViSiBle.SetActive(false);
        EssaieDeBloquerChuteCocciDuCiel = false;
        blocageAEteNecessaire = false;
    }
}
