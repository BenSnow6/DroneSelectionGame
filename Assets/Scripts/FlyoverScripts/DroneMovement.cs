using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneMovement : MonoBehaviour
{
    Vector3Int[] path = new Vector3Int[] {new Vector3Int(0,0,0), new Vector3Int(0,50,0), new Vector3Int(1000,50,1000), new Vector3Int(2000,50,1000), new Vector3Int(2000,0,1000)};
    List<Vector3Int> selectedPath = MainManager.Instance.clickedLocations;
    List<Vector3> scaledPath = new List<Vector3>();
    int pathIndex = 0;
    public float moveSpeed = 1f;
    private float x_scale = 0; // This is the width of the map divided by the number of tiles in the x direction (2244.6/10)
    private float z_scale = 0; // This is the length of the map divided by the number of tiles in the z direction (1746.7/8)
    private int flyHeight = 10;
    public Image DroneTracker;
    public FindMapSize mapSizeFinder;
    public GameObject miniMapCanvas;
    private float parentWidth;
    private float parentHeight;
    private Vector3 parentPosition;
    private Vector2 mapSize;
    private Vector2 miniMapSize;

    // Start is called before the first frame update
    void Start()
    {
        /// <summary>
        /// Scale the path to fit the grid.
        /// </summary>
        mapSize = mapSizeFinder.mapSize;
        x_scale = mapSize.x/10f; // Dividing by 10 because there are 10 tiles in the x direction in the selection scene
        z_scale = mapSize.y/8f; // It says y here because we're storing the z value in the y value of the Vector2
        miniMapSize = miniMapCanvas.GetComponent<RectTransform>().rect.size;

        generateRoute();


    }

    // Update is called once per frame
    void LateUpdate()
    {
     Move3D();
     Move2D();
     UpdateIndex();
     Debug.Log("Drone position: " + transform.position);
     Debug.Log("Drone tracker position: " + DroneTracker.transform.position);
     Debug.Log($"Move speed: {moveSpeed}");
    }

    void Move3D()
    {
        /// <summary>
        /// Moves the drone along the path from one waypoint to the next.
        /// </summary>

        transform.position = Vector3.MoveTowards(transform.position, scaledPath[pathIndex], moveSpeed * Time.deltaTime);
        // Set rotation of the drone
        transform.LookAt(scaledPath[pathIndex]);
        transform.Rotate(270, 0, 270);
    }
    private void Move2D()
    {
        // Move the drone tracker on the minimap
        float x = (transform.position.x / mapSize.x);
        float y = (transform.position.z / mapSize.y);
        DroneTracker.transform.position = new Vector3(50, 0, 0);
        // Set the local position of the drone tracker
        DroneTracker.transform.localPosition = new Vector3(x * miniMapSize.x - miniMapSize.x/2f, y * miniMapSize.y-miniMapSize.y/2f, 0);
        Debug.Log("Parent position: " + parentPosition);
        Debug.Log($"x and y: {x}, {y}");
    }
    private void UpdateIndex()
    {
        /// <summary>
        /// Updates the index of the path to move to the next waypoint.
        /// </summary>
        if (transform.position == scaledPath[pathIndex])
        {
            pathIndex++;
        }
        if (pathIndex == scaledPath.Count)
        {
            return;
        }
    }

    private Vector3 FindSize()
    {
        Vector3 size = GetComponent<Renderer>().bounds.size;
        return size;
    }

    private void generateRoute()
    {
        /// <summary>
        /// Scales the given location array to the 3D map size.
        /// </summary>
        int index = 0;
        foreach (Vector3Int item in MainManager.Instance.clickedLocations)
        {
            if (index == 0)
            {
                scaledPath.Add(new Vector3(item.x*x_scale, 0, item.y*z_scale));
                index ++;
            }
            else
            {
                scaledPath.Add(new Vector3(item.x*x_scale, flyHeight, item.y*z_scale));
                index ++;
            }
        }
        // Set the last point to the ground.
        scaledPath.Add(new Vector3(scaledPath[scaledPath.Count-1].x, 0, scaledPath[scaledPath.Count-1].z));
        /// Set the location of the drone to be the first location in the path.
        transform.position = scaledPath[pathIndex];
    }
    public void applySliderValue(float value)
    {
        moveSpeed = value;
        Debug.Log($"Move speed: {moveSpeed}");
    }
}
