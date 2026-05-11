using UnityEngine;

public class DansTerrain : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Si le joueur est dans le labyrinthe
        {

        }
    }

    void OnTriggerExit2D(Collider2D collision) // Si le joueur passe hors labyrinthe
    {

    }

}
