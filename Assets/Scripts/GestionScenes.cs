using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionScenes : MonoBehaviour
{
    public void ChangerScene() // Que j'active grâce au Event Trigger du parent
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Invoke("LoadScene1", 1f);
        }
        else if (SceneManager.GetActiveScene().name == "Selecteur de jeux")
        {
            Invoke("LoadScene2", 0.5f);
        }
    }


    public void LoadScene1()
    {
        Scene sceneCourante = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Selecteur de jeux");
    }

    public void LoadScene2() // jeu : Le Chemin de la Chenille
    {
        Scene sceneCourante = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Mini-Jeu1");
    }

     public void LoadScene3() // jeu : La Fourmi contre le Labyrinthe
    {
        Scene sceneCourante = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Mini-Jeu2");
    }

    public void LoadScene0() // Pour retourner au menu
    {
        Scene sceneCourante = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Menu");
    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
