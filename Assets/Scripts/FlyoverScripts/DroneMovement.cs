using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneMovement : MonoBehaviour
{
    Vector3Int[] path = new Vector3Int[] {new Vector3Int(0,0,0), new Vector3Int(0,50,0), new Vector3Int(1000,50,1000), new Vector3Int(2000,50,1000), new Vector3Int(2000,0,1000)};
    List<Vector3Int> selectedPath = MainManager.Instance.clickedLocations;
    List<Vector3Int> scaledPath = new List<Vector3Int>();
    int pathIndex = 0;
    [SerializeField] float moveSpeed = 1f;
    private int x_scale = 2245/10; // use renderer to get map size instead of hardcoding and divide by the grid size which wil be stored in the main manager
    private int z_scale = 1747/8;
    private int flyHeight = 10;
    public Image DroneTracker;

    // Start is called before the first frame update
    void Start()
    {
        /// <summary>
        /// Scale the path to fit the grid.
        /// </summary>
        int index = 0;
        foreach (Vector3Int item in MainManager.Instance.clickedLocations)
        {
            if (index == 0)
            {
                scaledPath.Add(new Vector3Int(item.x*x_scale, 0, item.y*z_scale));
                index ++;
            }
            else
            {
                scaledPath.Add(new Vector3Int(item.x*x_scale, flyHeight, item.y*z_scale));
                index ++;
            }
        }
        // Set the last point to the ground.
        scaledPath.Add(new Vector3Int(scaledPath[scaledPath.Count-1].x, 0, scaledPath[scaledPath.Count-1].z));
        /// Set the location of the drone to be the first location in the path.
        transform.position = scaledPath[pathIndex];
    }

    // Update is called once per frame
    void LateUpdate()
    {
     Move3D();
     Move2D();
     UpdateIndex();
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
        DroneTracker.transform.position = new Vector3(transform.position.x, transform.position.z, DroneTracker.transform.position.z);
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
}
