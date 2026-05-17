using UnityEngine;

public class PlateformeInvisibleTrigger : MonoBehaviour
{
    public Animator plateformeAnimator; // Selon la bonne plateforme montante

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            plateformeAnimator.SetTrigger("Activate");
        }
    }
}
