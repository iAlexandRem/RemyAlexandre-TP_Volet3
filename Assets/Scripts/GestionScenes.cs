using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionScenes : MonoBehaviour // Que j'active grâce au Event Trigger du parent
{
    public void AllerScene1() // Un délai pour laisser place à l'animation
    {
        Invoke("LoadScene1", 1f);
    }

      public void AllerJeu1() 
    {
        Invoke("LoadJeu1", 1f);
    }

     public void AllerJeu2() 
    {
        Invoke("LoadJeu2", 1f);
    }

     public void AllerJeu3() 
    {
        Invoke("LoadJeu3", 1f);
    }



    public void LoadScene1()
    {
        Scene sceneCourante = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Selecteur de jeux");
    }


    public void LoadJeu1() // jeu : Le Chemin de la Chenille
    {
        Scene sceneCourante = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Mini-Jeu1");
    }

    public void LoadJeu2() // jeu : La Fourmi contre le Labyrinthe
    {
        Scene sceneCourante = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Mini-Jeu2");
    }

    public void LoadJeu3() // jeu : Cocci-Connect4
    {
        Scene sceneCourante = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Mini-Jeu3");
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
