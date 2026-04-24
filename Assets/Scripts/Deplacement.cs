using UnityEngine;
using UnityEngine.InputSystem;

public class Deplacement : MonoBehaviour
{
    public InputAction actionDeplacement;
    
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    public float vitesseDeplacement;
    float directionDeplacement;

    void OnEnable()
    {
        actionDeplacement.Enable();
    }
    void OnDisable()
    {
        actionDeplacement.Disable();
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
        if (directionDeplacement < 0)
        {
            sr.flipX = true;
        }
        else if (directionDeplacement > 0)
        {
            sr.flipX = false;
        }
    }
    void FixedUpdate()
    {
        directionDeplacement = actionDeplacement.ReadValue<float>();

        if (directionDeplacement != 0)
        {
            rb.linearVelocityX = directionDeplacement * vitesseDeplacement;
        }
    }
}
