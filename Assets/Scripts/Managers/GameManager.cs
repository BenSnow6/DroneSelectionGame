using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public bool startFlyover = false;
    void Update()
    {
        // Commented out to ignore error, this seems to be used
        
        // Press the P key to start coroutine
        if (startFlyover) // change with new input system
        {
            // Use a coroutine to load the Scene in the background
            StartCoroutine(LoadYourAsyncScene());
        }
        startFlyover = false;

    }

    IEnumerator LoadYourAsyncScene()
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

    public void StartFlyover(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // clickUndo = !clickUndo;
        }
        else if (context.canceled)
        {
            startFlyover = true;
        }
    }
    public void StartFlyoverButton()
    {
        startFlyover = true;
    }
}