using UnityEngine;
using UnityEngine.InputSystem;

public class DeplacementParFleches : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    [Header("Déplacement")]
    float directionDeplacement = 0;
    float directionRotation = 0;
    public float vitesseDeplacement;
    public float vitesseRotation;
    public bool peutBouger = true;
    public bool enDeplacement = false;


    [Header("Gestion inputs")]
    public InputAction actionDeplacement;
    public InputAction actionRotation;

    void OnEnable()
    {
        actionDeplacement.Enable();
        actionRotation.Enable();
    }
    void OnDisable()
    {
        actionDeplacement.Disable();
        actionRotation.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // FixedUpdate is called at a constant interval and is very good for physics
    void FixedUpdate()
    {
        directionDeplacement = actionDeplacement.ReadValue<float>();
        directionRotation = actionRotation.ReadValue<float>();

        if (peutBouger)
        {
            transform.Translate(Vector2.left * directionDeplacement * vitesseDeplacement * Time.fixedDeltaTime);
            transform.Rotate(0, 0, directionRotation * vitesseRotation * Time.fixedDeltaTime);

            enDeplacement = directionDeplacement != 0 || directionRotation != 0; // La translation ou la rotation active l'animation Fourmi@Marche
            anim.SetFloat("vitesse", enDeplacement ? 1f : 0f); // dans l'Animator, vitesse de 1f si enDeplacement est true, sinon 0f
        }

    }
}
