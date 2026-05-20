using UnityEngine;
using UnityEngine.SceneManagement;

public class CaseFinale : MonoBehaviour
{
    public bool aGagne;
    Animator anim;
    AudioSource audioSource;
    public AudioSource tuAsGagne;
    public AudioSource musique;
    public AudioClip vocalFourmiAGagne;

    void Start()
    {
        if (GetComponentInChildren<Animator>() != null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        audioSource = GetComponent<AudioSource>();

        aGagne = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneManager.GetActiveScene().name == "Mini-Jeu1")
        {
            if (collision.gameObject.CompareTag("Player")) // La Chenille gagne en se rendant à la case 42
            {
                aGagne = true;
                Debug.Log("VICT0IRE");
                anim.SetTrigger("RayonsBlancs");
                musique.volume = 0.67f;
                GetComponent<Collider2D>().enabled = false; // Une seule victoire
            }
        }

        else if (SceneManager.GetActiveScene().name == "Mini-Jeu2")
        {
            if (collision.gameObject.CompareTag("Sucre")) // La Fourmi gagne en DÉPOSANT le sucre brun dans la case Fin
            {
                aGagne = true;
                Debug.Log("VICT0IRE");
                audioSource.PlayOneShot(vocalFourmiAGagne);
                musique.volume = 0.67f;
                tuAsGagne.volume = 1f;
                GetComponent<Collider2D>().enabled = false; // Une seule victoire
            }
        }
    }
}
