using UnityEngine;
using UnityEngine.EventSystems;

public class DragCocci : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    private Animator anim;
    public PartieConnect4 partie; // Référence au script PartieConnect4
    public SelectionCoccinelle selection; // Référence au script SelectionCoccinelle
    public EventTrigger eventTrigger;
    private Camera cam;
    private Vector3 offset;

    public GameObject prefabCocci; // Le prefab de la coccinelle
    public GameObject prefabRouge;
    public GameObject prefabJaune;
    public Transform spawnPointActuel; // Où je veux que ça spawn
    public Transform spawnPointAdversaire; // Au-dessus de la grille
    public static bool RespawnTime = false; // Pour contrôler le respawn par unité à volonté

    private bool peutDrag = true;
    public bool dropDepuisHautGrille = false; // La collision trigger avec DepuisHaut le fait passer à true


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        cam = Camera.main;

        rb.gravityScale = 0f; // Zéro gravité

        int currentOrder = sr.sortingOrder;
        sr.sortingOrder = 5; // Devant grille

        peutDrag = true;
        eventTrigger.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn() // Instantiation de cocci, voir script RespawnAuBonTour
    {
        GameObject nouvelleCocci = Instantiate(prefabCocci, spawnPointActuel.position, Quaternion.identity);

        nouvelleCocci.GetComponent<Animator>().enabled = true;
        nouvelleCocci.GetComponent<Animator>().SetTrigger("Spawn"); // Animation spawn rotation de la cocci

        nouvelleCocci.transform.localScale = Vector3.one; // Il y a un bug, il faut forcer le scale normal pour spawn l'équipe inverse

        if (spawnPointActuel == spawnPointAdversaire) // Spawn au-dessus si c'est le tour adverse
        {
            nouvelleCocci.GetComponent<DragCocci>().ForceDrop(); // Cocci tombe toute seule dans la grille
        }
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
        if (!peutDrag) return;

        GetComponent<Collider2D>().enabled = true; // De base au drop

        anim.enabled = false; // Si je ne le fais pas, je perds bizarrement leur rotation en physique

        peutDrag = false; // On ne peut plus rattraper Cocci lors du drop

        rb.gravityScale = 1f; // Cocci tombe 

        // Spawn(); Spawn d'une nouvelleCocci au drop

        RespawnTime = true; // Devient true pour toutes les prochaines instances (et non le PremierSpawn)

        Invoke("VerifierDrop", 0.05f); // Petit délai de vérification trigger
    }


    public void ForceDrop() // Comme AuFinGlisser, mais seulement pour le camp adverse qui tombe automatiquement dans la grille
    {
        GetComponent<Collider2D>().enabled = true;
    
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        peutDrag = false;
        eventTrigger.enabled = false;

        rb.gravityScale = 1f; // Gravité

        this.enabled = false; // Pour que le script n'interfère pas avec la physique 

        RespawnTime = true;
    }






    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DepuisHaut")) // Si on échappe Cocci depuis le haut de la grille
        {
            if (sr == null)
            {
                sr = GetComponentInChildren<SpriteRenderer>();
            }

            if (sr != null)
            {
                sr.sortingOrder = 2; // Derrière grille
            }

            dropDepuisHautGrille = true; // Le bool enclenche la détection des colliders triggers des Trous dans ColliderConnect4
        }
    }

    void VerifierDrop() // Désactiver le Collider2D si le drop n'est pas depuis le haut de la grille
    {
        if (!dropDepuisHautGrille)
        {
            GetComponent<Collider2D>().enabled = false; // Cocci ne reste pas pris au fond de la grille, et tombe à travers
        }

        peutDrag = false;
        eventTrigger.enabled = false; // Pour ne pas désactiver le Collider2D par accident, s'il doit rester activé
    }
}



