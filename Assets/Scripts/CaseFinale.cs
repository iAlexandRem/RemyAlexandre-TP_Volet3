using UnityEngine;

public class CaseFinale : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sucre"))
        {
            Debug.Log("VICT0IRE");
        }
    }
}
