using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    // Grab the screenshot from the main manager
    Texture2D screenshot = MainManager.Instance.screenshot;
    public Image minimapDisplay;
    void Start()
    {
        minimapDisplay.sprite = Sprite.Create(screenshot, new Rect(screenshot.width/10, screenshot.height/10, screenshot.width*0.9f, screenshot.height*0.9f), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
