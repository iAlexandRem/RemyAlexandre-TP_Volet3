using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionCoccinelle : MonoBehaviour
{
    Animator anim;

    public EventTrigger eventTrigger;

    public float delaiSec;

    public bool estRouge; // estRouge, true ou false sur la coccinelle par l'Inspector

    public static bool couleurEstChoisie = false; // Passe de false à true (static pour partager le bool aux autres scripts)

    public PartieConnect4 partie; // Référence au script PartieConnect4

    public GameObject panelCoccinelles; // Groupe des deux coccinelles dans le canvas


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        eventTrigger.enabled = false;

        Invoke(nameof(ActiverTrigger), delaiSec);
    }

    void ActiverTrigger()
    {
        eventTrigger.enabled = true; // Délai pour activer le Event Trigger
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
            if (estRouge)
            {
                partie.couleurChoisie = 1;
                Debug.Log("Rouge choisi");
            }
            else
            {
                partie.couleurChoisie = 2;
                Debug.Log("Jaune choisi");
            }
        }
    }
}
