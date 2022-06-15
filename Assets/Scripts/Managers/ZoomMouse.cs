using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    Vector3 newPosition;
    int zoom;
    bool canZoom;
    bool isMoving;

    void Update () 
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom > 49)
        {
            zoom -= 1;
            canZoom = true;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom < 101)
        {
            zoom += 1;
            canZoom = true;
        }

        if (canZoom) 
        {
            isMoving = true;
            Camera.main.orthographicSize = zoom;
            newPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        }
        if (isMoving)
        {
            transform.position = Vector3.Lerp (transform.position,  newPosition, Time.deltaTime);
        }

        if(transform.position == newPosition)
        {
            isMoving = false;
        }
    }
}