using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;
 
    void Update()
    {
        Zoom();
    }

    void Zoom()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit point;
        Physics.Raycast(ray, out point, 1000);
        Vector3 Scrolldirection = ray.GetPoint(5);

        float step = zoomSpeed * Time.deltaTime;

        // Allows zooming in and out via the mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Scrolldirection.y > minZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, Scrolldirection, Input.GetAxis("Mouse ScrollWheel") * step);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Scrolldirection.y < maxZoom)
        {
            transform.position = Vector3.MoveTowards(transform.position, Scrolldirection, Input.GetAxis("Mouse ScrollWheel") * step);
            
        }
    }
}