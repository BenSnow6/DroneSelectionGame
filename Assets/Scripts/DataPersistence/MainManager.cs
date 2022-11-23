using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    /// <summary>
    /// Allows for data persistence to be used in the game.
    /// </summary>

    public static MainManager Instance;
    public List<Vector3Int> clickedLocations;  // Save the locations of the clicked tiles to pass to flyover scene.
    public Texture2D screenshot; // Save the screenshot to pass to flyover scene.
    public Image screenshotDisplay; // Display the screenshot on the UI.

    private void Awake()
    {
        // Only allow one instance of the MainManger to exist.
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // Set the instance to this object.
        // Do not allow the MainManager to be destroyed.
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetScreenshot(Texture2D screenshot)
    {
        this.screenshot = screenshot;
        // Apply the screenshot to the UI.
        screenshotDisplay.sprite = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height), new Vector2(0.5f, 0.5f));
        Debug.Log("Screenshot set");
    }
}


/// Used with help from: https://learn.unity.com/tutorial/implement-data-persistence-between-scenes#

/// This method is called a Singleton pattern. Only one instance of the MainManager can exist at a time.