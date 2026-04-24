using UnityEngine;

public class CaseScript : MonoBehaviour
{
    public SpriteRenderer chiffre0; // Premier chiffre par l'inspecteur
    public SpriteRenderer chiffre00; // Deuxième chiffre, s'il y en a un
    public SpriteRenderer globe;
    public int nombre;

    float chiffreScaling = 1.3f; // Pour AGGRANDIR le chiffre
    private Vector3 scaleInitial0;
    private Vector3 scaleInitial00;
    private Color couleurInitiale; // Couleur du globe

    AudioSource audioSource;
    public AudioClip vocalNombreCompter;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (chiffre0 != null)
        {
            scaleInitial0 = chiffre0.transform.localScale;
        }
        if (chiffre00 != null)
        {
            scaleInitial00 = chiffre00.transform.localScale;
        }

        if (globe != null)
        {
            couleurInitiale = globe.color;
        }
    }

    void Update()
    {

    }



    public void ActiverCase() // Activée par OnTriggerEnter2D dans le script DansCollider du joueur
    {
        if (chiffre0 != null)
            chiffre0.transform.localScale = scaleInitial0 * chiffreScaling; //Chiffre + gros

        if (chiffre00 != null)
            chiffre00.transform.localScale = scaleInitial00 * chiffreScaling; //Chiffre + gros


        SetAlpha(1f); // Couleur flat blanche ajouté sur le chiffre
        globe.color = new Color(1f, 1f, 1f, globe.color.a); // Couleur globe + claire

        //Debug.Log("J'ai atteint la case " + nombre);
        audioSource.PlayOneShot(vocalNombreCompter); // On entend moi qui dit le nombre avec echo
    }


    public void DesactiverCase() // Activée par OnTriggerExit2D dans le script DansCollider du joueur
    {
        if (chiffre0 != null)
        {
            chiffre0.transform.localScale = scaleInitial0; //Chiffre taille normale
        }
        if (chiffre00 != null)
        {
            chiffre00.transform.localScale = scaleInitial00; //Chiffre taille normale
        }

        SetAlpha(0f); // Couleur flat blanche enlevée du chiffre
        globe.color = couleurInitiale;
    }






    void SetAlpha(float a) //Pour ajouter ou enlever la couleur flat blanche (par l'alpha) du chiffre
    {
        if (chiffre0 != null)
        {
            Color c = chiffre0.color;
            c.a = a; // 0f ou 1f
            chiffre0.color = c; // La couleur appliquée
        }

        if (chiffre00 != null)
        {
            Color c = chiffre00.color;
            c.a = a; // 0f ou 1f
            chiffre00.color = c; // La couleur appliquée
        }
    }

}
