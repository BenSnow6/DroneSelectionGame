
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] Image image;
    private InputScheme inputScheme;

    private void Awake()
    {
        inputScheme = new InputScheme();
        Debug.Log("InputManager Awake");
    }

    private void OnEnable()
    {
        inputScheme.Enable();
        Debug.Log("InputManager enabled");
    }

    private void OnDisable()
    {
        inputScheme.Disable();
    }

    private void Start() {
        inputScheme.MouseInputs.Touch.performed += ctx => StartTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        Debug.Log($"Touch Down at {ctx}");
        image.transform.position = ctx.ReadValue<Vector2>();
    }
}
