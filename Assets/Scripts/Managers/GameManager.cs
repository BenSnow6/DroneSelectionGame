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
            takeScreenshot = false;
            int width = Screen.width;
            int height = Screen.height;
            Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
            Rect rect = new Rect(0, 0, width, height);
            screenshotTexture.ReadPixels(rect, 0, 0);
            screenshotTexture.Apply();
            // pass screenshot to main manager
            mainManager.SetScreenshot(screenshotTexture);
            // Save the screenshot as a png
            byte[] bytes = screenshotTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/ScreenshotNoUI.png", bytes);
            Debug.Log("Screenshot written");
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