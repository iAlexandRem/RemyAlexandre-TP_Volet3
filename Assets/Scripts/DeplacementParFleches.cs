using UnityEngine;
using UnityEngine.InputSystem;

public class DeplacementParFleches : MonoBehaviour
{
    float directionDeplacement = 0;
    float directionRotation = 0;

    public float vitesseDeplacement;
    public float vitesseRotation;

    public InputAction onDeplacement;
    public InputAction onRotation;

    void OnEnable()
    {
        onDeplacement.Enable();
        onRotation.Enable();
    }
    void OnDisable()
    {
        onDeplacement.Disable();
        onRotation.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {
        directionDeplacement = onDeplacement.ReadValue<float>();
        directionRotation = onRotation.ReadValue<float>();

        transform.Rotate(0, 0, directionRotation * vitesseRotation * Time.deltaTime);
        transform.Translate(Vector2.left * directionDeplacement * vitesseDeplacement * Time.deltaTime);
    }
}
