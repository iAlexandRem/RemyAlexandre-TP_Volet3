using UnityEngine;

public class ColliderConnect4 : MonoBehaviour
{
    private TrouConnect4 trou;

    public PartieConnect4 partie; // Référence au script PartieConnect4


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trou = GetComponent<TrouConnect4>(); // Cela cherche le script TrouConnect4 sur le même component
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
            trou.trouOccupe = true;

            Debug.Log("Cocci rouge dans rangée " + trou.rangee + " colonne " + trou.colonne);

            partie.JouerCoup(trou.rangee, trou.colonne); // On inscrit la coordonnée du trou comme étant celle du joueur
        }

        if (collision.CompareTag("CocciJaune"))
        {
            trou.trouOccupe = true;

            Debug.Log("Cocci jaune dans rangée " + trou.rangee + " colonne " + trou.colonne);

            partie.JouerCoup(trou.rangee, trou.colonne); // On inscrit la coordonnée du trou comme étant celle du joueur
        }
    }
}
