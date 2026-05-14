using UnityEngine;

public class PartieConnect4 : MonoBehaviour
{

    private int[,] plateau = new int[6, 7]; // Un array 2D avec 6 rangées et 7 colonnes : plateau[ligne, colonne]
    private bool tourRouge = true; // Les rouges commencent vraiment



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    // 0 = VIDE, 1 = ROUGE, 2 = JAUNE
    public void JouerCoup(int colonne) // À l'aide de la colonne du trou joué dans la partie
    {
        int joueur = tourRouge ? 1 : 2; // Si c'est le tourRouge, joueur = 1, sinon 2

        // Si la colonne est pleine jusqu'en haut
        if (plateau[5, colonne - 1] != 0) // 6e rangée, et colonne, vérifier si ce n'est pas vide
        {
            Debug.Log("Colonne pleine");
            return; // On arrête le reste de la fonction
        }


        for (int rangee = 1; rangee <= 6; rangee++) // Vérifier avec une boucle pour toutes les rangées de 1 à 6
        {
            if (plateau[rangee - 1, colonne - 1] == 0) // Si le trou à telle rangée et colonne est vide
            {
                plateau[rangee - 1, colonne - 1] = joueur; // Exemple: plateau[2, 3] = 1
                Debug.Log((joueur == 1 ? "Rouge" : "Jaune") + " joue en " + rangee + "," + colonne);

                tourRouge = !tourRouge; // C'est le tour de l'adversaire

                if (tourRouge)
                {
                    Debug.Log("Les Rouges, c'est votre tour!");
                }
                else
                {
                    Debug.Log("Les Jaunes, c'est votre tour!");
                }
            }
        }
    }
}

