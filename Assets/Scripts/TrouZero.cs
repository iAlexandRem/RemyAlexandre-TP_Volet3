using UnityEngine;

public class TrouZero : MonoBehaviour
{
    public DansCollider dansCollider; // Référence au script DansCollider
    public int delaiResetChutes;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && dansCollider.vientDEtreTeleporte)
        {
            dansCollider.vientDEtreTeleporte = false;
            Invoke("DelaiResetChutes", delaiResetChutes);
        }
    }

    void DelaiResetChutes()
    {
        dansCollider.vientDEtreTeleporte = true; // Juste pour reset NombreDeChutesFourmi si on tombe par exprès dans le trou 0 
    }
}
