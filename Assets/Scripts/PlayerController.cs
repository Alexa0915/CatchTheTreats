using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float keySpeed = 8f;
    float minX, maxX;

    void Start()
    {
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;
        float margin = 0.5f;
        minX = -halfWidth + margin;
        maxX = halfWidth - margin;
    }

    void Update()
    {
        float x = transform.position.x;

        var touch = Touchscreen.current;
        var mouse = Mouse.current;
        var keys = Keyboard.current;

        if (touch != null && touch.primaryTouch.press.isPressed)
        {
            Vector2 p = touch.primaryTouch.position.ReadValue();
            x = Camera.main.ScreenToWorldPoint(new Vector3(p.x, p.y, 10f)).x;
        }
        else if (mouse != null && mouse.leftButton.isPressed)
        {
            Vector2 p = mouse.position.ReadValue();
            x = Camera.main.ScreenToWorldPoint(new Vector3(p.x, p.y, 10f)).x;
        }
        else if (keys != null)
        {
            float dir = 0f;
            if (keys.leftArrowKey.isPressed) dir -= 1f;
            if (keys.rightArrowKey.isPressed) dir += 1f;
            x += dir * keySpeed * Time.deltaTime;
        }

        x = Mathf.Clamp(x, minX, maxX);
        transform.position = new Vector3(x, transform.position.y, 0f);
    }
}