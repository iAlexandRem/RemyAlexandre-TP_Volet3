using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnAuBonTour : MonoBehaviour // Au cas-où que les coccis ne respawn pas
{
    public DragCocci drag; // Référence au script DragCocci
    public PartieConnect4 partie; // Référence au script PartieConnect4
    public Transform spawnPointRouge;
    public Transform spawnPointJaune;
    public Transform spawnPointAdversaire;
    public bool aSpawnCeTour = true;
    public bool dropJoueurDuHautGrilleDetecte = false;
    private bool bordInverse;
    public AudioClip sfxCartoonFall;

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

        if (partie.aGagne) return;
        if (EssaieDeBloquerChuteCocciDuCiel && !blocageAEteNecessaire)
        {
            bordInverse = !bordInverse;
            blocageAEteNecessaire = true;

            if (bordInverse)
            {
                cOLLISIOnInViSiBle.transform.rotation = Quaternion.Euler(0, 0, 30); // De chaque bord
            }
            else
            {
                cOLLISIOnInViSiBle.transform.rotation = Quaternion.Euler(0, 0, -30);
            }
            cOLLISIOnInViSiBle.SetActive(true); // Méthode non orthodoxe, pour bloquer le fall
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


            if (drag.coupEnregistre) // Juste pour éviter un double spawn du joueur
            {
                drag.JoueurADejaRespawn = true;
            }
        }

        if (!aSpawnCeTour && !drag.dropDepuisHautGrille) // Il faut que cocci tombe dedans
        {
            anim.speed = 0.05f;
            Invoke("RespawnJoueur", 6f);
        }
        else if (partie.coupRetire)
        {
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

        if (Random.value < 0.70f) // Vitesse normale
        {
            value = Random.Range(0.01f, 0.07f);

            Invoke("StopTranslationX", Random.Range(6f, 7f));
        }
        else if (Random.value < 0.91f) // Plus rapide
        {
            value = Random.Range(0.07f, 0.21f);

            Invoke("LegereTranslationX", Random.Range(0.4f, 4f));
        }
        else // Rare
        {
            value = Random.Range(0.18f, 0.28f);
        }
        anim.speed = value;



        if (!partie.aGagne)
        {
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
        anim.speed = 0.05f;
        Invoke("StopTranslationX", 4f);
    }

    public void TranslationX()
    {
        anim.speed = 1f;
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
        audioSource.PlayOneShot(sfxCartoonFall); // On peut voir une cocci tomber à ce moment-là

        cOLLISIOnInViSiBle.SetActive(false);
        EssaieDeBloquerChuteCocciDuCiel = false;
        blocageAEteNecessaire = false;
    }
}
