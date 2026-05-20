using UnityEngine;

public class DetectionSucre : MonoBehaviour
{
    public LeJeu jeu; // Référence au script LeJeu
    public CaseFinale finale; // Référence au script CaseFinale
    AudioSource audioSource;
    public AudioClip vocalSucreBrunTrouve;
    public AudioClip vocalRappelTrouverSucreBrun;
    public bool premiereRencontreSucre = false; // La première fois que tu trouves le sucre brun
    public bool deuxiemeRencontreSucre = false;
    public GameObject SucreBrun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        premiereRencontreSucre = false;
        deuxiemeRencontreSucre = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SucreBrun != null)
        {
            transform.position = SucreBrun.transform.position; // La détection trigger suit le sucre brun, c'est séparé avec cet objet nul, car avant j'avais testé de mettre ce script comment enfant et ça avait corrompu la physique
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !premiereRencontreSucre && jeu.MessageDebutFini && !LeJeu.autreVocalQuiJoue && !finale.aGagne)
        {
            premiereRencontreSucre = true;
            audioSource.PlayOneShot(vocalSucreBrunTrouve); // Message qui joue quand tu trouves le sucre brun pour la 1ère fois
            LeJeu.autreVocalQuiJoue = true;
            Invoke("AucunVocalJoue", 11f);
            enabled = false;
        }
        if (collision.CompareTag("Player") && !deuxiemeRencontreSucre && jeu.MessageDebutFini && !LeJeu.autreVocalQuiJoue && !finale.aGagne)
        {
            deuxiemeRencontreSucre = true;
            audioSource.PlayOneShot(vocalRappelTrouverSucreBrun); // Rappel de ramener le sucre brun vers la Fin
            LeJeu.autreVocalQuiJoue = true;
            Invoke("AucunVocalJoue", 4f);
            enabled = false;
        }
    }

    void AucunVocalJoue()
    {
        jeu.SonEstLibre();
    }
}
