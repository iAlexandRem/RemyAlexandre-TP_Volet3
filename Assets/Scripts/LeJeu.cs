using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEditor;

public class LeJeu : MonoBehaviour
{
    AudioSource audioSource;
    public AudioSource autreAudioSource;
    public AudioClip vocalMessageDebut;
    public AudioClip vocalInstructionsDebut;
    public AudioClip vocalPasLaSouris;
    bool premierClickDetecte = false;
    bool instructionsJouees = false;
    public HoverBoutons[] hoverBoutons; // Pour éviter la cacophonie grâce au bool hoverUI
    bool wasHovering = false; // True si on arrête de hover une fois qu'on a hover
    bool isHovering = false; // True si on hover au moins un des boutons
    bool wasPlaying = false; // True si l'AudioClip finit de jouer une fois joué

    public bool MessageDebutFini = false;
    private bool flecheDejaPressee = false;
    public bool vocalInstructionsTerminees = false;
    public static bool autreVocalQuiJoue = true; // Si une autre narration est en train de jouer
    public Animator anim;



    void Start()
    {
        SelectionCoccinelle.couleurEstChoisie = false;

        audioSource = GetComponent<AudioSource>();

        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        if (SceneManager.GetActiveScene().name == "Selecteur de jeux")
        {
            audioSource.loop = true;
        }
        else
        {
            audioSource.loop = false;
        }
        audioSource.clip = vocalMessageDebut;
        audioSource.Play();
        MessageDebutFini = false;
        autreVocalQuiJoue = true;
    }


    void Update()
    {
        if (FlechePressee())
        {
            flecheDejaPressee = true;
        }

        // Debug.Log(autreVocalQuiJoue);

        if (!audioSource.isPlaying && audioSource.clip == vocalMessageDebut && !MessageDebutFini)
        {
            MessageDebutFini = true;
            autreVocalQuiJoue = false;
            if (!flecheDejaPressee)
            {
                Invoke("JouerInstructions", 1.5f);
                autreVocalQuiJoue = true;
            }
        }

        // Au premier clic durant Mini-Jeu1 et Mini-Jeu2
        if (!premierClickDetecte && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && SceneManager.GetActiveScene().name != "Menu" && SceneManager.GetActiveScene().name != "Selecteur de jeux" && SceneManager.GetActiveScene().name != "Mini-Jeu3")
        {
            premierClickDetecte = true;

            audioSource.Stop(); // J'arrête le premier message par souris
            MessageDebutFini = true;

            if (flecheDejaPressee) // S'il y a clic alors que les flèches ont été pressées
            {
                audioSource.PlayOneShot(vocalPasLaSouris);
                vocalInstructionsTerminees = true;
                autreVocalQuiJoue = true;
                Invoke("SonEstLibre", 3f);
            }
            else
            {
                JouerInstructions();
            }
        }

        // Pour le Mini-Jeu3, on attend la sélection de couleur
        if (!premierClickDetecte &&
            SceneManager.GetActiveScene().name == "Mini-Jeu3" &&
            SelectionCoccinelle.couleurEstChoisie)
        {
            premierClickDetecte = true;

            audioSource.Stop(); // J'arrête le premier message quand la couleur est choisie

            if (!instructionsJouees)
            {
                instructionsJouees = true;

                audioSource.loop = false;
                audioSource.clip = vocalInstructionsDebut;
                audioSource.Play(); // On passe aux INSTRUCTIONS de Drag&Drop

                wasPlaying = true;
            }
        }


        if (wasPlaying && audioSource.clip == vocalInstructionsDebut && !audioSource.isPlaying && !vocalInstructionsTerminees)
        {
            vocalInstructionsTerminees = true;
            wasPlaying = false;
            autreVocalQuiJoue = false;
            // Debug.Log("Instructions terminées");
        }


        if (SceneManager.GetActiveScene().name == "Selecteur de jeux")
        {
            isHovering = false; // À réinitialiser

            foreach (HoverBoutons hb in hoverBoutons) // Pour chacun des trois boutons ayant le script HoverBoutons
            {
                if (hb != null && hb.hoverUI)
                {
                    isHovering = true;
                }
            }

            if (isHovering)
            {
                audioSource.Stop(); // Le message qui joue s'arrête si on hover un bouton de mini-jeu

                autreAudioSource.volume = 0f; // Pas de cacophonie accidentelle
            }
            else
            {
                if (wasHovering)
                {
                    audioSource.time = 21f; // À f secondes dans l'audio
                    audioSource.Play(); // Le message ("Sélectionne un mini-jeu parmi les trois suivants") rejoue seulement si on sort du hover

                    autreAudioSource.volume = 0f; // Pas de cacophonie accidentelle
                    Invoke("RemettreVolume", 5f);
                }
            }
            wasHovering = isHovering; // Le bool du passé devient à chaque fois le bool du présent
        }
    }


    void JouerInstructions()
    {
        instructionsJouees = true;

        audioSource.loop = false;
        audioSource.clip = vocalInstructionsDebut;


        if (SceneManager.GetActiveScene().name == "Mini-Jeu3")
        {
            if (SelectionCoccinelle.couleurEstChoisie)
            {
                audioSource.Play(); // On passe aux INSTRUCTIONS de Drag&Drop
            }
        }
        else if (SceneManager.GetActiveScene().name == "Mini-Jeu2")
        {
            audioSource.Play(); // On passe aux INSTRUCTIONS des flèches clavier
            anim.SetTrigger("FlechesInstructions");
        }

        else if (SceneManager.GetActiveScene().name == "Mini-Jeu1")
        {
            audioSource.Play(); // On passe aux INSTRUCTIONS de la souris
            anim.SetTrigger("Souris-Demonstration");
        }
        wasPlaying = true; // Fin des instructions
    }




    bool FlechePressee() // True si le joueur a déjà pressé les flèches du clavier 
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return false;

        return Keyboard.current.upArrowKey.isPressed ||
           Keyboard.current.downArrowKey.isPressed ||
           Keyboard.current.leftArrowKey.isPressed ||
           Keyboard.current.rightArrowKey.isPressed;
    }



    void RemettreVolume()
    {
        autreAudioSource.volume = 0.67f;
    }

    public void SonEstLibre()
    {
        autreVocalQuiJoue = false;
    }
}
