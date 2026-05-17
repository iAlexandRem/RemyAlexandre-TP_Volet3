using UnityEngine;

public class Nuages : MonoBehaviour
{
    public float vitesse = 2f;
    float yInitial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        yInitial = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * vitesse * Time.deltaTime); // Direction gauche (-1, 0) avec vitesse

        if (transform.position.x < -25f) // Si trop à gauche
        {
            transform.position = new Vector3(142f, yInitial + Random.Range(1f, 4f), 0f); // À droite + positionY aléatoire
        }
    }
}
