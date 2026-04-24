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


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = false;
        audioSource.clip = vocalMessageDebut;
        audioSource.Play();
    }


    void Update()
    {
        if (!premierClickDetecte &&
            SceneManager.GetActiveScene().name == "Mini-Jeu1" && // Au premier clic durant le premier mini-jeu
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            premierClickDetecte = true; // Il ne sera plus false
            audioSource.Stop(); // J'arrête le premier message

            if (!instructionsJouees)
            {
                instructionsJouees = true;
                
                audioSource.loop = false;
                audioSource.clip = vocalInstructionsDebut;
                audioSource.Play(); // On passe aux instructions de la souris
            }
        }
    }
}
