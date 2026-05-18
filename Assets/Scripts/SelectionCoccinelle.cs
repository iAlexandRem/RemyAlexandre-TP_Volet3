using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionCoccinelle : MonoBehaviour
{
    Animator anim;
    public Animator animRouge;
    public Animator animJaune;
    public EventTrigger eventTrigger;

    public float delaiSec;

    public bool estRouge; // estRouge, true ou false sur la coccinelle par l'Inspector

    public static bool couleurEstChoisie = false; // Passe de false à true (static pour partager le bool aux autres scripts)
    public static DragCocci dragSelectionne; // Pour s'assurer de spawn la couleur choisie avec SelectionCoccinelle.dragSelectionne.Spawn();
    public DragCocci dragRouge; // Prefab rouge
    public DragCocci dragJaune; // Prefab jaune

    public PartieConnect4 partie; // Référence au script PartieConnect4
    public GameObject panelCoccinelles; // Groupe des deux coccinelles dans le canvas
    public bool peutHoverCocci = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if (animRouge == null)
        {
            animRouge = GetComponent<Animator>();
        }
        if (animJaune == null)
        {
            animJaune = GetComponent<Animator>();
        }

        eventTrigger.enabled = false;
        Invoke(nameof(ActiverTrigger), delaiSec);
    }

    void ActiverTrigger() // Pour pas de clic trop hâtif
    {
        eventTrigger.enabled = true; // Délai pour activer le Event Trigger
        peutHoverCocci = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Quand le joueur choisit la couleur de coccinelle de son choix dans le canvas
    public void visuelClicCoccinelle() // Que j'active grâce au Event Trigger du parent
    {
        couleurEstChoisie = true;

        panelCoccinelles.SetActive(false);

        if (partie.couleurChoisie == 0) // Une seule fois le choix de couleur
        {
            Invoke("CouleurChoisie", 0.5f);
        }
    }

    public void CouleurChoisie() // Rouge ou jaune, à transmettre au script PartieConnect4
    {
        if (estRouge)
        {
            partie.couleurChoisie = 1;
            Debug.Log("Rouge choisi");
            dragSelectionne = dragRouge;
        }
        else
        {
            partie.couleurChoisie = 2;
            Debug.Log("Jaune choisi");
            dragSelectionne = dragJaune;
        }

        PremierSpawn();
    }

    public void PremierSpawn() // Le joueur a accès aux coccinelles prefabs de la couleur de son choix
    {
        if (partie.couleurChoisie == 1 && animRouge != null)
        {
            animRouge.SetTrigger("Spawn");
        }
        else if (partie.couleurChoisie == 2 && animJaune != null)
        {
            animJaune.SetTrigger("Spawn");
        }
    }
}
