using UnityEngine;
using UnityEngine.InputSystem;

public class SucreBrun : MonoBehaviour
{
    MouvementBillesEnnemies[] billes; // Référence au script MouvementBillesEnnemies des billes

    Transform joueur;
    Rigidbody2D rb;

    bool estPorte = false; // On ne porte pas le sucre au début
    public float distancePrise; // 4f


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        joueur = playerObj.transform; // Transform du joueur
        billes = FindObjectsByType<MouvementBillesEnnemies>(FindObjectsSortMode.None);

        Invoke("PetitePoussee", 3f); // Délai
    }

    void PetitePoussee()
    {
        rb.AddForce(Vector2.down * 17f, ForceMode2D.Impulse); // Poussée automatique du sucre vers le bas au début du jeu
    }


    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) // On équipe le sucre quand on appuie Espace
        {
            float dist = Vector2.Distance(transform.position, joueur.position); // Distance entre le sucre et joueur

            if (!estPorte && dist <= distancePrise) // Quand on est près du sucre
            {
                estPorte = true;
                rb.simulated = false; // Physique est désactivée, donc suit celle de la fourmi
            }
            else if (estPorte) // Quand on lâche le sucre
            {
                estPorte = false;
                rb.simulated = true;
            }
        }

        if (estPorte)
        {
            transform.position = (Vector2)joueur.position + (Vector2)joueur.right * -2f; // 2 unités en décalage avec la tête de la fourmi


            foreach (MouvementBillesEnnemies bille in billes) // Pour chaque bille...
            {
                if (bille.collisionBilleFourmi)
                {
                    estPorte = false; // La fourmi échappe le sucre, lors d'une collision avec bille
                }
            }
        }
    }
}