using UnityEngine;

public class MessagesAutresZones : MonoBehaviour
{
    public LeJeu jeu; // Référence au script LeJeu
    AudioSource audioSource;
    public AudioClip zoneMessage; // Différents messages par l'inspecteur
    public float delaiPourSonLibre; // Délai que ça prend pour activer un autre message
    bool messageAJoue = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        messageAJoue = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && jeu.MessageDebutFini && !LeJeu.autreVocalQuiJoue && !messageAJoue)
        {
            messageAJoue = true;
            audioSource.PlayOneShot(zoneMessage);
            LeJeu.autreVocalQuiJoue = true;
            Invoke("AucunVocalJoue", delaiPourSonLibre);
        }
    }

    void AucunVocalJoue()
    {
        jeu.SonEstLibre();
    }

}
