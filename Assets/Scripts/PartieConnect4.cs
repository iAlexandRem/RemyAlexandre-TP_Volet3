using UnityEngine;

public class PartieConnect4 : MonoBehaviour // Avec recherches de théorie sur les tableaux (à 2 integers) et boucles sur grille...
{

    // index : 0 → 5 (rangées; r)
    // index : 0 → 6 (colonnes; c)
    private int[,] plateau = new int[6, 7]; // Un array 2D vide avec 6 rangées et 7 colonnes : plateau[rangee, colonne]
    // (0,0) = l'origine est en haut à gauche (C# standard)
    // L'index des colonnes augmentent vers la droite, l'index des rangées augmentent vers le bas

    public bool tourRouge = true; // Les rouges commencent 



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
        if (plateau[0, colonne] != 0) // Une colonne est pleine si le haut est occupé
        {
            Debug.Log("Colonne pleine");
            return; // Stop de la fonction en entier
        }


        for (int rangee = 5; rangee >= 0; rangee--) // Vérifier avec une boucle (du bas vers le haut) pour toutes les 6 rangées de 5 à 0
        {
            if (plateau[rangee, colonne] == 0) // Si le trou à telle rangée et colonne est vide
            {
                // On inscrit alors que rangée, colonne = joueur
                plateau[rangee, colonne] = joueur; // Ex. plateau[0, 3] = 1 ; 1ère rangée du haut & 4e colonne = Rouge
                Debug.Log((joueur == 1 ? "Rouge" : "Jaune") + " joue en " + rangee + "," + colonne);


                if (VerifierVictoire(joueur)) // Si la fonction bool est true, victoire
                {
                    Debug.Log("Les " + (joueur == 1 ? "Rouges" : "Jaunes") + " ont GAGNÉ !");
                    return; // Stop de la fonction en entier
                }

                tourRouge = !tourRouge; // C'est le tour de l'adversaire, ça inverse le bool à chaque fois

                if (tourRouge)
                {
                    Debug.Log("Les Rouges, c'est votre tour!");
                }
                else
                {
                    Debug.Log("Les Jaunes, c'est votre tour!");
                }

                break; // Arrêt de la boucle
            }
        }
    }


    // La logique est de scanner à chaque fois sur tout le plateau 6x7 : 4 trous consécutifs, si [r, c] == joueur
    bool VerifierVictoire(int joueur)
    {
        // Horizontal
        for (int r = 0; r < 6; r++) // Pour chacune des 6 rangées (r de 0 à 5)
        {
            for (int c = 0; c < 4; c++) // c = 3 maximum pour ne pas aller hors grille (c de 0 à 3)
            {
                if (plateau[r, c] == joueur && // Trou de base, appartient-il au joueur?
                    plateau[r, c + 1] == joueur &&
                    plateau[r, c + 2] == joueur &&
                    plateau[r, c + 3] == joueur) // Ex. [r, 3+3]; c = 6, donc jusqu'à la 7e dernière colonne à droite
                {
                    return true; // 4 trous consécutifs
                }
            }
        }

        // Vertical
        for (int c = 0; c < 7; c++) // Pour chacune des 7 colonnes (c de 0 à 6)
        {
            for (int r = 0; r < 3; r++) // r = 2 maximum pour ne pas aller hors grille (r de 0 à 2)
            {
                if (plateau[r, c] == joueur && // Trou de base, appartient-il au joueur?
                    plateau[r + 1, c] == joueur &&
                    plateau[r + 2, c] == joueur &&
                    plateau[r + 3, c] == joueur) // Ex. [2+3, c]; r = 5, donc jusqu'à la 6e dernière rangée en bas
                {
                    return true; // 4 trous consécutifs
                }
            }
        }

        // Diagonale descendante (\)
        for (int r = 0; r < 3; r++) // (r de 0 à 2)
        {
            for (int c = 0; c < 4; c++) // (c de 0 à 3)
            {
                if (plateau[r, c] == joueur && // Trou de base, appartient-il au joueur? Ex. [0, 0] 
                    plateau[r + 1, c + 1] == joueur && // Ex. [0+1, 0+1]; r = 1, c = 1 
                    plateau[r + 2, c + 2] == joueur && // Ex. [0+2, 0+2]; r = 2, c = 2 
                    plateau[r + 3, c + 3] == joueur) // Ex. [0+3, 0+3]; r = 3, c = 3 
                {
                    return true; // 4 trous consécutifs
                }
            }
        }

        // Diagonale montante (/)
        for (int r = 3; r < 6; r++) // (r de 3 à 5)
        {
            for (int c = 0; c < 4; c++) // (c de 0 à 3)
            {
                if (plateau[r, c] == joueur && // Trou de base, appartient-il au joueur? Ex. [5, 0] 
                    plateau[r - 1, c + 1] == joueur && // Ex. [5-1, 0+1]; r = 4, c = 1 
                    plateau[r - 2, c + 2] == joueur && // Ex. [5-2, 0+2]; r = 3, c = 2 
                    plateau[r - 3, c + 3] == joueur) // Ex. [5-3, 0+3]; r = 2, c = 3 
                {
                    return true; // 4 trous consécutifs
                }
            }
        }

        return false; // Si aucune victoire n'est trouvée
    }
}





