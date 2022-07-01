using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UndoButton : MonoBehaviour
{
    public bool buttonPressed;
    Grid grid;
    ClickController clickController;

    // Update is called once per frame

    void Start()
        {
            grid = GameObject.FindGameObjectWithTag("TagGrid").GetComponent<Grid>();
            clickController = grid.GetComponent<ClickController>();
        }
    void Update()
    {
        if (buttonPressed)
        {
            Debug.Log("Undo Button Pressed");
            clickController.removeTile(new Vector3Int(1, 1, 0), true);
            buttonPressed = false;
        }
    }
    public void setPressed(bool pressed)
    {
        buttonPressed = pressed;
        Debug.Log($"Button pressed is {buttonPressed}");
    }
    public void printIt()
    {
        Debug.Log("Button pressed is " + buttonPressed);
    }
}
