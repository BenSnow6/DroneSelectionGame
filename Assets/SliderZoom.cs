using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderZoom : MonoBehaviour
{
    Camera mainCamera;
    public float ZoomAmount = 4.5f;
    public Joystick joystick;
    public float moveSpeed = 100f;
    private Vector3 camStartPos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        camStartPos = mainCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.orthographicSize = ZoomAmount;
        ClampCamera();
        mainCamera.transform.position += new Vector3(joystick.Horizontal/moveSpeed, joystick.Vertical/moveSpeed, 0);

    }

    public void applySliderValue(float value)
    {
        ZoomAmount = value;
    }
    
    private void ClampCamera()
    {
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float mapHeight = 10f;
        float mapWidth = 14f;
        float xMin = camWidth / 2f - 2.5f;
        float xMax = mapWidth - camWidth/2 - 1f;
        float yMin = camHeight / 2f - 0.5f;
        float yMax = mapHeight - camHeight / 2f;


        Vector3 pos = mainCamera.transform.position;
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);
        mainCamera.transform.position = pos;
    }
}
