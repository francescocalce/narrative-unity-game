using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float velocita = 4f;
    public float sensibilita = 0.1f;
    public float gravita = -20f;
    public float forzaSalto = 6f;

    private CharacterController cc;
    private Transform cam;
    private float rotX = 0f;
    private float velY = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        // Movimento
        float x = 0f, z = 0f;
        if (Keyboard.current.wKey.isPressed) z = 1f;
        if (Keyboard.current.sKey.isPressed) z = -1f;
        if (Keyboard.current.aKey.isPressed) x = -1f;
        if (Keyboard.current.dKey.isPressed) x = 1f;

        Vector3 mov = transform.right * x + transform.forward * z;
        cc.Move(mov * velocita * Time.deltaTime);

        // Gravita sempre attiva
        velY += gravita * Time.deltaTime;
        cc.Move(new Vector3(0, velY, 0) * Time.deltaTime);

        // Salto - dopo aver applicato gravita
        if (cc.isGrounded)
        {
            velY = -2f;
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                velY = forzaSalto;
            }
        }

        // Mouse
        Vector2 mouse = Mouse.current.delta.ReadValue();
        rotX -= mouse.y * sensibilita;
        rotX = Mathf.Clamp(rotX, -80f, 80f);
        cam.localRotation = Quaternion.Euler(rotX, 0f, 0f);
        transform.Rotate(0f, mouse.x * sensibilita, 0f);

        // ESC
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}