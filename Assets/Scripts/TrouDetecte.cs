using UnityEngine;

public class TrouDetecte : MonoBehaviour
{
    public SucreBrun sucre; // Référence au script SucreBrun

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision) // Si le sucre tombe dans le trou avec la fourmi
    {
        if (collision.CompareTag("Player") && sucre.estPorte)
        {
            sucre.LacheLeSucre(); // La fourmi perd le sucre si ça tombe avec elle dans le trou
        }
    }
}
