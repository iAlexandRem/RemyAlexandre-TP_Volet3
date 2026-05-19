using UnityEngine;

public class DetectionSucre : MonoBehaviour
{
    public LeJeu jeu; // Référence au script LeJeu
    AudioSource audioSource;
    public AudioClip vocalSucreBrunTrouve;
    public bool decouverteSucreBrun = false; // La première fois que tu trouves le sucre brun

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        decouverteSucreBrun = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !decouverteSucreBrun && jeu.vocalInstructionsTerminees && !LeJeu.autreVocalQuiJoue)
        {
            decouverteSucreBrun = true;
            audioSource.PlayOneShot(vocalSucreBrunTrouve);
            LeJeu.autreVocalQuiJoue = true;
            Invoke("VersSonLibre", 10f);
        }
    }

    void VersSonLibre()
    {
        jeu.SonEstLibre();
    }
}
