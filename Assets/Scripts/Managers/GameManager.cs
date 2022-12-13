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
    private Vector3 camStartPos;
    private float camStartZoom;

    private void Start()
    {
        // Get the location and zoom of the main camera when the scene loads
        // wait for 2 seconds to make sure the camera has been initialized
        camStartPos = new Vector3(6.1f, 4.2f,-10f);
        camStartZoom = 4.3f;
        Debug.Log($"camStartPos: {camStartPos}");
    }
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
            // Set the camera back to the original position and zoom
            //setCameraForScreenShot();
            // Get the bounds of the tilemap and multiply it to get the size of the tilemap in world space
            Vector3 size = screenShotTilemap.GetComponent<Renderer>().bounds.size;
            // Debug.Log($"Size of screenShotTilemap: {size}");
            // Get size of tiles in pixels
            

            // Debug.Log($"Size of screenShotTilemap: {screenShotTilemap.GetComponent<Renderer>().bounds}");
            takeScreenshot = false;
            int width = Screen.width;
            int height = Screen.height;
            float a = 420/2560;
            float b = 85/1600;
            Debug.Log($"Height {height}, width {width}");
            Texture2D screenshotTexture = new Texture2D((int) Mathf.Round(width*(1-2*a)),(int) Mathf.Round(height*(1-2*b)), TextureFormat.RGB24, false);
            Rect rect = new Rect(a*width, b*height, width*(1-2*a), height*(1-2*b));
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
    private void setCameraForScreenShot()
    {
        // Move camera to the position and zoom it was at when the scene started
        Camera.main.transform.position = camStartPos;
        Debug.Log($"camStartPos: {camStartPos}");
        Camera.main.orthographicSize = camStartZoom;
        Debug.Log($"camStartZoom: {camStartZoom}");
        // Pause the game
        Time.timeScale = 0;
    }
}