using UnityEngine;

public class TrouConnect4 : MonoBehaviour
{
    public int rangee; // Pour identifier la rangée du trou
    public int colonne; // Pour identifier la colonne du trou
    public TrouConnect4[] trousColonne; // 6 trous de la colonne à inverser l'ordre des trouOccupe qui deviennet true à chaque rangée
    public bool trouOccupe = false; // Le trou n'est pas occupé au début

    public void OccuperTrouLePlusBas()
    {
        // trousColonne.Length = 6
        for (int i = trousColonne.Length - 1; i >= 0; i--) // Du trou 5 bas, jusqu'au trou 0 haut
        {
            if (!trousColonne[i].trouOccupe) // Si le trou le plus bas qui a détecté une collision n'est pas occupé
            {
                trousColonne[i].trouOccupe = true; // Il devient occupé
                return; // Fin de la boucle
            }
        }
    }
}
