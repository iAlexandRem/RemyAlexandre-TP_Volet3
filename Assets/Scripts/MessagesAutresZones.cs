using UnityEngine;

public class MessagesAutresZones : MonoBehaviour
{
    public LeJeu jeu; // Référence au script LeJeu
    public SucreBrun sucre; // Référence au script SucreBrun
    public DansCollider dansCollider; // Référence au script DansCollider
    AudioSource audioSource;
    public AudioClip zoneMessage; // Différents messages par l'inspecteur
    public AudioClip zoneMessageAvecSucre;
    public AudioClip zoneMessage1ApresTomberTrou;
    public AudioClip zoneMessage2ApresTomberTrou;
    public AudioClip zoneMessage3ApresTomberTrou;
    public float delaiPourSonLibre; // Délai que ça prend pour activer un autre message
    bool messageAJoue = false;
    bool message1JouerApresTrou = false;
    bool message2JouerApresTrou = false;
    bool message3JouerApresTrou = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        messageAJoue = false;
        message1JouerApresTrou = false;
        message2JouerApresTrou = false;
        message3JouerApresTrou = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && jeu.MessageDebutFini && !LeJeu.autreVocalQuiJoue && !messageAJoue && zoneMessage != null)
        {
            messageAJoue = true;
            audioSource.PlayOneShot(zoneMessage); // Sortie hors labyrinthe par le haut
            LeJeu.autreVocalQuiJoue = true;
            Invoke("AucunVocalJoue", delaiPourSonLibre);
        }

        if (sucre.estPorte && collision.CompareTag("Player") && jeu.MessageDebutFini && !LeJeu.autreVocalQuiJoue && !messageAJoue && zoneMessageAvecSucre != null)
        {
            messageAJoue = true;
            audioSource.PlayOneShot(zoneMessageAvecSucre); // Tu es sur le bon chemin, ou tu y es presque
            LeJeu.autreVocalQuiJoue = true;
            Invoke("AucunVocalJoue", delaiPourSonLibre);
        }


        if (collision.CompareTag("Player") && jeu.MessageDebutFini && !LeJeu.autreVocalQuiJoue) // Quelques mots après chutes dans trou
        {
            if (dansCollider.NombreDeChutesFourmi == 2 || dansCollider.NombreDeChutesFourmi >= 8)
            {
                if (zoneMessage1ApresTomberTrou != null && !message1JouerApresTrou)
                {
                    message1JouerApresTrou = true;
                    Invoke("MessagePeutSeRejouer", 10f);
                    audioSource.PlayOneShot(zoneMessage1ApresTomberTrou); // Variation1
                    LeJeu.autreVocalQuiJoue = true;
                    Invoke("AucunVocalJoue", 6f);
                }
            }
            else if (dansCollider.NombreDeChutesFourmi == 3)
            {
                if (zoneMessage2ApresTomberTrou != null && !message2JouerApresTrou)
                {
                    message2JouerApresTrou = true;
                    audioSource.PlayOneShot(zoneMessage2ApresTomberTrou); // Variation2
                    LeJeu.autreVocalQuiJoue = true;
                    Invoke("AucunVocalJoue", 6f);
                }

            }
            else if (dansCollider.NombreDeChutesFourmi == 6)
            {
                if (zoneMessage3ApresTomberTrou != null && !message3JouerApresTrou)
                {
                    message3JouerApresTrou = true;
                    audioSource.PlayOneShot(zoneMessage3ApresTomberTrou); // Variation3
                    LeJeu.autreVocalQuiJoue = true;
                    Invoke("AucunVocalJoue", 6f);
                }
            }
            LeJeu.autreVocalQuiJoue = true;
            Invoke("AucunVocalJoue", delaiPourSonLibre);
        }
    }

    void AucunVocalJoue()
    {
        jeu.SonEstLibre();
    }


    void MessagePeutSeRejouer()
    {
        message1JouerApresTrou = false;
    }
}
