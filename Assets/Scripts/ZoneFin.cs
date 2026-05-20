using UnityEngine;

public class ZoneFin : MonoBehaviour
{
    public LeJeu jeu; // Référence au script LeJeu
    public SucreBrun sucre; // Référence au script SucreBrun
    AudioSource audioSource;
    public AudioClip vocalOubliSucreBrun;
    public AudioClip vocalRappelTrouverSucreBrun;
    bool premierRappelJoue = false;
    bool deuxiemeRappelJoue = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        premierRappelJoue = false;
        deuxiemeRappelJoue = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D entree)
    {
        if (entree.CompareTag("Player") && !sucre.estPorte && !LeJeu.autreVocalQuiJoue) // Si la fourmi atteint la Fin, sans rapporter le sucre
        {
            if (!premierRappelJoue)
            {
                audioSource.PlayOneShot(vocalOubliSucreBrun);
                premierRappelJoue = true;
                Invoke("DeuxiemeRappel", 10f);
                LeJeu.autreVocalQuiJoue = true;
                Invoke("AucunVocalJoue", 4f);
            }
            else if (!deuxiemeRappelJoue)
            {
                Invoke("DeuxiemeRappel", 1f);
            }
        }
    }

    void DeuxiemeRappel()
    {
        audioSource.PlayOneShot(vocalRappelTrouverSucreBrun);
        deuxiemeRappelJoue = true;
        LeJeu.autreVocalQuiJoue = true;
        Invoke("AucunVocalJoue", 4f);
    }

    void AucunVocalJoue()
    {
        jeu.SonEstLibre();
    }
}
