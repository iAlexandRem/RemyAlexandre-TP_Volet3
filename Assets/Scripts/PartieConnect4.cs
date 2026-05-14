using UnityEngine;

public class PartieConnect4 : MonoBehaviour // Avec recherches de théorie sur les tableaux (à 2 integers) et boucles...
{

    private int[,] plateau = new int[6, 7]; // Un array 2D vide avec 6 rangées et 7 colonnes : plateau[rangee, colonne]
    private bool tourRouge = true; // Les rouges commencent 



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
        if (plateau[5, colonne - 1] != 0) // 6e rangée, et selon colonne, vérifier si ce n'est pas vide
        {
            Debug.Log("Colonne pleine");
            return; // Pas besoin du reste de la fonction
        }


        for (int rangee = 1; rangee <= 6; rangee++) // Vérifier avec une boucle pour toutes les rangées de 1 à 6
        {
            if (plateau[rangee - 1, colonne - 1] == 0) // Si le trou à telle rangée et colonne est vide
            {
                // On inscrit alors que rangée, colonne = joueur (et -1 car l'index d'un tableau commence par 0)
                plateau[rangee - 1, colonne - 1] = joueur; // Exemple: plateau[2, 3] = 1
                Debug.Log((joueur == 1 ? "Rouge" : "Jaune") + " joue en " + rangee + "," + colonne);

                tourRouge = !tourRouge; // C'est le tour de l'adversaire, ça inverse le bool à chaque fois

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

        if (VerifierVictoire(joueur)) // Si la fonction bool est true, victoire
        {
            Debug.Log("Les " + (joueur == 1 ? "Rouges" : "Jaunes") + " ont GAGNÉ !");
        }
    }


    bool VerifierVictoire(int joueur)
    {
        
        return true;
    }



}

