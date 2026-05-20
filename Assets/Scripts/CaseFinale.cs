using UnityEngine;
using UnityEngine.SceneManagement;

public class CaseFinale : MonoBehaviour
{
    public bool aGagne;
    Animator anim;
    public Animator boutonAnim;
    public Animator cinemachine;
    AudioSource audioSource;
    public AudioSource compter42;
    public AudioClip sfxChenilleAGagne;
    public AudioClip vocalOuiBravoPapillon;
    public AudioSource tuAsGagne;
    public AudioSource musique;
    public AudioClip vocalFourmiAGagne;
    public AudioClip sfxFourmiAGagne;

    void Start()
    {
        if (GetComponentInChildren<Animator>() != null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        audioSource = GetComponent<AudioSource>();

        aGagne = false;
        
        if (SceneManager.GetActiveScene().name == "Mini-Jeu1")
        {
            compter42.volume = 0f;
        }
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
                Invoke("SoundEffectVictoireChenille", 1f);
                Invoke("ZoomCinemachine", 3f);
                musique.volume = 4f;
                compter42.volume = 1f;
                Invoke("InciteRetournerMenu", 7f);
                GetComponent<Collider2D>().enabled = false; // Une seule victoire
                Invoke("TransformationPapillon", 21f);
            }
        }

        else if (SceneManager.GetActiveScene().name == "Mini-Jeu2")
        {
            if (collision.gameObject.CompareTag("Sucre")) // La Fourmi gagne en DÉPOSANT le sucre brun dans la case Fin
            {
                aGagne = true;
                Debug.Log("VICT0IRE");
                audioSource.PlayOneShot(vocalFourmiAGagne);
                Invoke("SoundEffectVictoireFourmi", 1f);
                Invoke("ZoomCinemachine", 3f);
                musique.volume = 0.67f;
                tuAsGagne.volume = 1f;
                Invoke("InciteRetournerMenu", 7f);
                GetComponent<Collider2D>().enabled = false; // Une seule victoire
            }
        }
    }

    void SoundEffectVictoireChenille()
    {
        audioSource.PlayOneShot(sfxChenilleAGagne);
    }

    void SoundEffectVictoireFourmi()
    {
        audioSource.PlayOneShot(sfxFourmiAGagne);
    }

    void ZoomCinemachine()
    {
        cinemachine.SetTrigger("CamZoom");
    }

    void InciteRetournerMenu()
    {
        boutonAnim.SetTrigger("TempsDeQuitter");
    }

    void TransformationPapillon()
    {
        audioSource.PlayOneShot(vocalOuiBravoPapillon);
    }

}
