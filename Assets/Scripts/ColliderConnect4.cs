using UnityEngine;

public class ColliderConnect4 : MonoBehaviour
{
    private TrouConnect4 trou;

    public PartieConnect4 partie; // Référence au script PartieConnect4


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trou = GetComponent<TrouConnect4>(); // Recherche du script TrouConnect4 sur le même component
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (trou.trouOccupe) // Si le trou est occupé, on ne fait rien
        {
            return;
        }

        if (collision.CompareTag("CocciRouge"))
        {
            if (!partie.tourRouge)
            {
                return; // Aucune détection des rouges si c'est le tour des Jaunes
            }

            trou.trouOccupe = true;

            partie.JouerCoup(trou.colonne); // Retenir la colonne du trou où la coccinelle est tombée
        }

        if (collision.CompareTag("CocciJaune"))
        {
            if (partie.tourRouge)
            {
                return; // Aucune détection des jaunes si c'est le tour des Rouges
            }

            trou.trouOccupe = true;

            partie.JouerCoup(trou.colonne); // Retenir la colonne du trou où la coccinelle est tombée
        }
    }
}
