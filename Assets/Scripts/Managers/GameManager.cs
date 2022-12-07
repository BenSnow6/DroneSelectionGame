using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    public bool startFlyover = false;
    private bool takeScreenshot = false;
    public MainManager mainManager;
    public Tilemap[] tilemaps;
    public Tilemap screenShotTilemap;
    private void OnEnable()
    {
        // Subscribe to the render pipeline event.
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    private void OnDisable()
    {
        // Subscribe to the render pipeline event.
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }

    IEnumerator LoadFlyoverScene()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2);
        Debug.Log("Loading Scene");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("FlyoverScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    public void StartFlyoverButton()
    {
        // startFlyover = true;
        StartCoroutine(LoadFlyoverScene());
    }
    public void captureScreenshot()
    {
            Debug.Log("Screenshot taken");
            // StartCoroutine(CoroutineScreenShot());
            TurnOffTilemaps();
            takeScreenshot = true;
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (takeScreenshot)
        {
            // Get the bounds of the tilemap and multiply it to get the size of the tilemap in world space
            Vector3 size = screenShotTilemap.GetComponent<Renderer>().bounds.size;
            // Debug.Log($"Size of screenShotTilemap: {size}");
            // Get size of tiles in pixels
            

            // Debug.Log($"Size of screenShotTilemap: {screenShotTilemap.GetComponent<Renderer>().bounds}");
            takeScreenshot = false;
            int width = Screen.width;
            int height = Screen.height;
            Debug.Log($"Height {height}, width {width}");
            Texture2D screenshotTexture = new Texture2D(2140-420, 1455-80, TextureFormat.RGB24, false);
            Rect rect = new Rect(420, 85, 1730, 1370);
            screenshotTexture.ReadPixels(rect, 0, 0);
            screenshotTexture.Apply();
            // Debug.Log($"{screenshotTexture.width} , {screenshotTexture.height} look @ me");
            // pass screenshot to main manager
            mainManager.SetScreenshot(screenshotTexture);
            // Save the screenshot as a png
            byte[] bytes = screenshotTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Screenshot.png", bytes);
            Debug.Log($"Screenshot is at {Application.persistentDataPath + "/Screenshot.png"}");
        }
    }
    
    private void TurnOffTilemaps()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.gameObject.SetActive(false);
        }
    }

}