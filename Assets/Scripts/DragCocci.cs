using UnityEngine;
using UnityEngine.EventSystems;

public class DragCocci : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    private Camera cam;
    private Vector3 offset;
    private bool peutDrag = true;
    public bool dropDepuisHautGrille = false; // La collision trigger avec DepuisHaut le fait passer à true

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cam = Camera.main;

        rb.gravityScale = 0f; // Zéro gravité

        int currentOrder = sr.sortingOrder;
        sr.sortingOrder = 5; // Devant grille
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // DRAG & DROP

    public void AuDebutGlisser(BaseEventData eventData)
    {
        if (!peutDrag) return; // Empêche le redrag

        dropDepuisHautGrille = false;

        PointerEventData pointerEventData = eventData as PointerEventData;

        GetComponent<Collider2D>().enabled = false; // Désactiver le collider

        Vector3 mousePos = new Vector3(pointerEventData.position.x, pointerEventData.position.y, Mathf.Abs(cam.transform.position.z));

        mousePos = cam.ScreenToWorldPoint(mousePos); // Un point écran à une position dans le monde

        mousePos.z = 0f;

        offset = transform.position - mousePos; // Décalage entre souris et cocci
    }

    public void AuGlisser(BaseEventData eventData)
    {
        if (!peutDrag) return; // Empêche le redrag

        PointerEventData pointerEventData = eventData as PointerEventData;

        Vector3 mousePos = new Vector3(pointerEventData.position.x, pointerEventData.position.y, Mathf.Abs(cam.transform.position.z));

        mousePos = cam.ScreenToWorldPoint(mousePos);

        mousePos.z = 0f;

        transform.position = mousePos + offset; // Cocci suit la position de souris au drag
    }

    public void AuFinGlisser(BaseEventData eventData)
    {
        GetComponent<Collider2D>().enabled = true; // De base au drop

        peutDrag = false; // On ne peut plus rattraper Cocci lors du drop

        rb.gravityScale = 1f; // Cocci tombe dans le néant

        Invoke("VerifierDrop", 0.05f); // Petit délai de vérification trigger
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DepuisHaut")) // Si on échappe Cocci depuis le haut de la grille
        {
            sr.sortingOrder = 2; // Derrière grille
            dropDepuisHautGrille = true; // Le bool enclenche la détection des colliders triggers des Trous dans ColliderConnect4
        }
    }

     void VerifierDrop() // Désactiver le Collider2D si le drop n'est pas depuis le haut de la grille
    {
        if (!dropDepuisHautGrille)
        {
            GetComponent<Collider2D>().enabled = false; // Cocci ne reste pas pris au fond de la grille, et tombe à travers
        }
    }
}



