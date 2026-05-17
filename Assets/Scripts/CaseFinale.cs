using UnityEngine;
using UnityEngine.SceneManagement;

public class CaseFinale : MonoBehaviour
{
    public bool aGagne;
    Animator anim;

    void Start()
    {
        if (GetComponentInChildren<Animator>() != null)
        {
            anim = GetComponentInChildren<Animator>();
        }

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
                GetComponent<Collider2D>().enabled = false; // Une seule victoire
            }
        }

        else if (SceneManager.GetActiveScene().name == "Mini-Jeu2")
        {
            if (collision.gameObject.CompareTag("Sucre")) // La Fourmi gagne en rapportant le sucre brun dans la case Fin
            {
                aGagne = true;
                Debug.Log("VICT0IRE");
                GetComponent<Collider2D>().enabled = false; // Une seule victoire
            }
        }
    }
}
