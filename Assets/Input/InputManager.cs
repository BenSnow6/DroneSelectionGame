
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] Highlight highlight;
    [SerializeField] TooltipManager tooltipManager;
    [SerializeField] ClickController clickController;
    private InputScheme inputScheme;

    private void Awake()
    {
        inputScheme = new InputScheme();
        // Debug.Log("InputManager Awake");
    }

    private void OnEnable()
    {
        inputScheme.Enable();
        // Debug.Log("InputManager enabled");
    }

    private void OnDisable()
    {
        inputScheme.Disable();
    }

    private void Start() {
        inputScheme.MouseInputs.Touch.performed += ctx => StartTouch(ctx);
        inputScheme.MouseInputs.TapHoldSelect.performed += ctx => TapSelect(ctx);
           
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        highlight.HoverLocation(ctx);
        tooltipManager.HoverLocation(ctx);
    }
    
    private void TapSelect(InputAction.CallbackContext ctx)
    {
        Vector2 movementInput = ctx.ReadValue<Vector2>();
        Vector3 mousePos = new Vector3(movementInput.x, movementInput.y, 0);
        Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int tileLocalPos = clickController.TilePosition(mouseLocation);
        Debug.Log($"Mouse position is {tileLocalPos}");
        // Debug.Log($"Tile position is {ctx.ReadValue<Vector2>()}");
        clickController.TapSelect(ctx);
        clickController.HoverLocation(ctx);
    }
}
