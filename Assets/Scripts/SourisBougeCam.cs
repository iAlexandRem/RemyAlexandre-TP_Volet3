using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SourisBougeCam : MonoBehaviour
{
    public float initialMouvementX;
    public float initialMouvementY;
    public float vitesse;
    private float quantiteMouvementX; // Pour ne pas écraser les valeurs quand je les change
    private float quantiteMouvementY;

    private Vector3 posDepart;
    private Vector3 posCible;

    private CinemachineVirtualCamera cam;
    public float zoomMin;
    public float zoomMax;
    public float vitesseZoom;
    private float zoomCible;

    public GameObject LimitesPositionCamera; // J'aimerais moins de limites lorsqu'on zoom
    public Vector3 hausseScaleLimites; // Par l'Inspector
    private Vector3 initialScaleLimites;
    private Vector3 targetScaleLimites; // Facteur d'aggrandissement cible
    public float smoothSpeed; // Vitesse de transition du scale pour le lerp

    void Start()
    {
        // Position de départ de la CinemachineCamera
        posDepart = new Vector3(11f, 56f, transform.localPosition.z);
        transform.localPosition = posDepart;

        quantiteMouvementX = initialMouvementX;
        quantiteMouvementY = initialMouvementY;

        cam = GetComponent<CinemachineVirtualCamera>();
        zoomCible = zoomMin;

        if (LimitesPositionCamera != null)
        {
            initialScaleLimites = new Vector3(100f, 48f, 1f);
            targetScaleLimites = initialScaleLimites;
        }
    }

    void Update()
    {
        if (PartieConnect4.partieCommence)
        {
            // TRANSLATION sur scène par souris
            Vector2 mouse = Mouse.current.position.ReadValue(); // 1920 x 1080 px

            // Normaliser position entre -1 et 1
            float mouseX = (mouse.x / Screen.width - 0.5f) * 2f; // -1 à 1
            float mouseY = (mouse.y / Screen.height - 0.5f) * 2f; // -1 à 1

            // Offset
            float x = posDepart.x + mouseX * quantiteMouvementX;
            float y = posDepart.y + mouseY * quantiteMouvementY;

            posCible = new Vector3(x, y, posDepart.z);

            // Lerp
            transform.localPosition = Vector3.Lerp(transform.localPosition, posCible, vitesse * Time.deltaTime);
        }


        // ZOOM molette souris
        float scroll = Mouse.current.scroll.ReadValue().y; // Molette vers le haut ou bas

        if (scroll != 0)
        {
            zoomCible += scroll * vitesseZoom;
            zoomCible = Mathf.Clamp(zoomCible, zoomMin, zoomMax);
        }
        // Transition 
        cam.m_Lens.OrthographicSize =
            Mathf.Lerp(cam.m_Lens.OrthographicSize, zoomCible, vitesseZoom * Time.deltaTime);


        if (zoomCible >= zoomMax)
        {
            quantiteMouvementX = initialMouvementX * 5f; // + de mouvement
            quantiteMouvementY = initialMouvementY * 5f;

            targetScaleLimites = initialScaleLimites + hausseScaleLimites; // + vaste pour les limites de position caméra quand on zoom
        }
        else if (zoomCible <= zoomMin)
        {
            quantiteMouvementX = initialMouvementX;
            quantiteMouvementY = initialMouvementY;

            targetScaleLimites = initialScaleLimites;
        }
        // Transition 
        LimitesPositionCamera.transform.localScale = Vector3.Lerp(LimitesPositionCamera.transform.localScale, targetScaleLimites, smoothSpeed * Time.deltaTime);
    }
}