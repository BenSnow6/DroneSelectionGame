using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputTouchTest : MonoBehaviour
{
    [SerializeField] Image image; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTouchDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            image.color = Color.red;
        }
        else if (context.canceled)
            {
                Debug.Log("Touch Up");
            }
    }
}
