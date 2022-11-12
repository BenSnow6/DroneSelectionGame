using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Script to make the school markers face the camera
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
        // Check which camera is active and make the school markers face that camera
        if (mainCamera.activeSelf)
        {
            // Lerping the rotation of the school markers to face the camera
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - mainCamera.transform.position), 0.02f);
        }
        else if (birdsEyeCamera.activeSelf)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - birdsEyeCamera.transform.position), 0.02f);
        }
        else if (zoomCamera.activeSelf)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - zoomCamera.transform.position), 0.02f);
        }
    }
}
