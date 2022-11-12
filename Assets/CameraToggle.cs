using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    // Script to toggle between three cinemachine cameras
    // 1. Main Camera
    // 2. Camera 1
    // 3. Camera 2
    public GameObject mainCamera;
    public GameObject birdsEyeCamera;
    public GameObject zoomCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void toggleCamera()
    {
        if (mainCamera.activeSelf)
        {
            mainCamera.SetActive(false);
            birdsEyeCamera.SetActive(true);
        }
        else if (birdsEyeCamera.activeSelf)
        {
            birdsEyeCamera.SetActive(false);
            zoomCamera.SetActive(true);
        }
        else if (zoomCamera.activeSelf)
        {
            zoomCamera.SetActive(false);
            mainCamera.SetActive(true);
        }
    }
}
