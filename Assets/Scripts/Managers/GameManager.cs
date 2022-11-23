using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public bool startFlyover = false;
    private bool screenshotOnce = false;
    public MainManager mainManager;
    public Tilemap[] tilemaps;
    void Update()
    {
        // Commented out to ignore error, this seems to be used
        
        // Press the P key to start coroutine
        // if (startFlyover) // change with new input system
        // {
        //     // Use a coroutine to load the Scene in the background
        //     StartCoroutine(LoadYourAsyncScene());
        // }
        // startFlyover = false;

    }

    IEnumerator LoadFlyoverScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        Debug.Log("Loading Scene");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("FlyoverScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // public void StartFlyover(InputAction.CallbackContext context)
    // {
    //     if (context.started)
    //     {
    //         // clickUndo = !clickUndo;
    //     }
    //     else if (context.canceled)
    //     {
    //         startFlyover = true;
    //     }
    // }
    public void StartFlyoverButton()
    {
        // startFlyover = true;
        StartCoroutine(LoadFlyoverScene());
    }

    private IEnumerator CoroutineScreenShot()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect rect = new Rect(width/10, height/10, width*0.9f, height*0.9f);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();
        // pass screenshot to main manager
        mainManager.SetScreenshot(screenshotTexture);
        // Save the screenshot as a png
        byte[] bytes = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshot.png", bytes);
        Debug.Log("Screenshot written");
    }
    public void captureScreenshot()
    {
            Debug.Log("Screenshot taken");
            StartCoroutine(CoroutineScreenShot());
    }
}