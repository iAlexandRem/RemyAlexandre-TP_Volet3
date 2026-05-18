using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnAuBonTour : MonoBehaviour // Au cas-où que les coccis ne respawn pas
{
    public DragCocci drag; // Référence au script DragCocci
    public Transform spawnPointRouge;
    public Transform spawnPointJaune;
    public Transform spawnPointAdversaire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DragCocci.RespawnTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && DragCocci.RespawnTime)
        {
            drag.prefabCocci = drag.prefabJaune;
            drag.spawnPointActuel = spawnPointAdversaire;
            drag.Spawn();
        }
    }
}