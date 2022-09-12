using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour
{

    public Vector2 movementInput;
    public Vector3 mousePos;
    public Vector3 mouseLocation;
    public static TooltipManager _instance;
    public TextMeshProUGUI textHeader;
    public TextMeshProUGUI textContent;
    private Image toolTipImage;

    private void Awake() {
        // Only want one TooltipManager instance in the game. If one is found, delete this. If not, set the instance to this one.
        if (_instance !=null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
        toolTipImage = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Input.mousePosition;
        transform.position = mousePos;
    }

    public void SetAndShowToolTip(string Header, string Content, Color toolTipColor)
    {
        gameObject.SetActive(true);
        textHeader.text = Header;
        textContent.text = Content;
        toolTipImage.color = toolTipColor;

        
    }
    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textHeader.text = string.Empty;
        textContent.text = string.Empty;
    }

    public void HoverLocation(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        mousePos = new Vector3(movementInput.x, movementInput.y, 0);
        mouseLocation = Camera.main.ScreenToWorldPoint(mousePos);
    }
}
