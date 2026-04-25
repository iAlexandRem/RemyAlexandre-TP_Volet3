using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CurseurUI : MonoBehaviour
{
    public RectTransform cursorTransform;
    public Image img;

    public Sprite sprite1; // Feuille
    public Sprite sprite2; // Feuille clic

    void Start()
    {
        img.sprite = sprite1;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue(); // Position souris

        cursorTransform.position = mousePos + new Vector2(14f, -23f);; // Position du sprite qui suit la position souris (centré sur la tige)

        if (Mouse.current.leftButton.isPressed)
        { 
            img.sprite = sprite2; // Au clic, on change de sprite
        }
        else
        {
            img.sprite = sprite1;
        }
    }
}