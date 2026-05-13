using UnityEngine;

public class PartieConnect4 : MonoBehaviour
{

    private int[,] plateau = new int[6, 7]; // Un array avec 6 rangées et 7 colonnes
    private bool tourRouge = true; // Les rouges commencent



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    // 0 = vide, 1 = rouge, 2 = jaune
    public void JouerCoup(int rangee, int colonne) // On inscrit la coordonnée du trou comme étant celle du joueur
    {
        int joueur = tourRouge ? 1 : 2; // Si c'est le tourRouge, joueur = 1, sinon 2

        plateau[rangee - 1, colonne - 1] = joueur; // Selon (rangee, colonne), je mets un 1 ou 2 en index dans l'array plateau

        Debug.Log((joueur == 1 ? "Rouge" : "Jaune") + " joue en " + rangee + "," + colonne); 

        tourRouge = !tourRouge; // C'est le tour de l'adversaire
    }
}
