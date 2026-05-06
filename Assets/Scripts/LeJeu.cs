using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LeJeu : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip vocalMessageDebut;
    public AudioClip vocalInstructionsDebut;
    bool premierClickDetecte = false;
    bool instructionsJouees = false;
    public HoverBoutons hoverBoutons; // Pour éviter la cacophonie avec le bool hoverUI
    bool wasHovering = false; // True si on arrête de hover une fois qu'on a hover
    bool wasPlaying = false; // True si l'AudioClip finit de jouer une fois joué
    public bool vocalInstructionsTerminees = false;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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
    }


    void Update()
    {
        // Au premier clic durant le premier mini-jeu
        if (!premierClickDetecte && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            premierClickDetecte = true;
            audioSource.Stop(); // J'arrête le premier message

            if (!instructionsJouees)
            {
                instructionsJouees = true;

                audioSource.loop = false;
                audioSource.clip = vocalInstructionsDebut;
                audioSource.Play(); // On passe aux instructions de la souris ou du clavier

                wasPlaying = true;
            }
        }
        else if (wasPlaying && audioSource.clip == vocalInstructionsDebut && !audioSource.isPlaying && !vocalInstructionsTerminees)
        {
            vocalInstructionsTerminees = true;
            wasPlaying = false;
            // Debug.Log("Instructions terminées");
        }


        else if (SceneManager.GetActiveScene().name == "Selecteur de jeux")
        {
            bool isHovering = (hoverBoutons != null && hoverBoutons.hoverUI);

            if (isHovering)
            {
                audioSource.Stop();
            }
            else
            {
                if (wasHovering)
                {
                    audioSource.time = 0f;
                    audioSource.Play(); // Le message ("Sélectionne un mini-jeu parmi les trois suivants") rejoue seulement si on sort du hover
                }
            }
            wasHovering = isHovering; // Le bool du passé devient à chaque fois le bool du présent
        }
    }
}
