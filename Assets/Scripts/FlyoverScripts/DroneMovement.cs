using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    private bool routeFinished = false;
    public Image DroneTracker;
    public FindMapSize mapSizeFinder;
    public GameObject miniMapCanvas;
    private float parentWidth;
    private float parentHeight;
    private Vector3 parentPosition;
    private Vector2 mapSize;
    private Vector2 miniMapSize;
    private bool showFinishedBox = true;
    [SerializeField] public float x_offset_minimap = 2.5f;
    [SerializeField] public float y_offset_minimap = 7.7f;
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
    void Update()
    {
        if(!routeFinished)
        {
            Move3D();
            Move2D();
            UpdateIndex();
        }
        else
        {
            if(showFinishedBox)
            {
                showFinishedBox = false;
                GetComponent<Renderer>().enabled = false;
                transform.localScale = new Vector3(0,0,0);
                QuestionDialogUI.Instance.ShowQuestion("Mission complete! Return to selection scene?", () => {
                Restart();
                // EditorApplication.ExitPlaymode(); # removed so build can take place
            }, () => {
                    Exit();// Do nothing
            });
            }
        }
        // Debug.Log("Drone position: " + transform.position);
        // Debug.Log("Drone tracker position: " + DroneTracker.transform.position);
        // Debug.Log($"Move speed: {moveSpeed}");
    }

    void Move3D()
    {
        /// <summary>
        /// Moves the drone along the path from one waypoint to the next.
        /// </summary>

        transform.position = Vector3.MoveTowards(transform.position, scaledPath[pathIndex], moveSpeed * 0.01f); //* Time.deltaTime);
        // Set rotation of the drone
        transform.LookAt(scaledPath[pathIndex]);
        transform.Rotate(270,0,270);
    }
    private void Move2D()
    {
        // Move the drone tracker on the minimap
        float x = (transform.position.x / mapSize.x);
        float y = (transform.position.z / mapSize.y);
        // DroneTracker.transform.position = new Vector3(50, 0, 0);
        // Set the local position of the drone tracker
        DroneTracker.transform.localPosition = new Vector3(x * miniMapSize.x - miniMapSize.x/2f+3f+x_offset_minimap, y * miniMapSize.y-miniMapSize.y/2f+8f+y_offset_minimap, 0);

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
            // set bool to stop movement, set rotation of craft, and display dialog box.
            routeFinished = true;
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

    // private void generateRoute()
    // {
    //     /// <summary>
    //     /// Scales the given location array to the 3D map size.
    //     /// </summary>
    //     float x_scalefactor = 0.06f;
    //     float z_scalefactor = 0.22f;
    //     int index = 0;
    //     foreach (Vector3Int item in MainManager.Instance.clickedLocations) 
    //     {
    //         if (index == 0)
    //         {
    //             scaledPath.Add(new Vector3(item.x*x_scale*(1+x_scalefactor), 0, item.y*z_scale*(1+z_scalefactor)));
    //             Debug.Log($"Index is {index} and location is {scaledPath[index]}");
    //             index ++;
    //         }
    //         else
    //         {
    //             if(index == selectedPath.Count)
    //             {
    //                 scaledPath.Add(new Vector3(item.x*x_scale*(1+x_scalefactor), 0, item.y*z_scale*(1+z_scalefactor)));
    //                 Debug.Log($"Last index with locations {scaledPath[index]}");
    //                 break;
    //             }
    //             scaledPath.Add(new Vector3(item.x*x_scale*(1+x_scalefactor), flyHeight, item.y*z_scale*(1+z_scalefactor)));
    //             Debug.Log($"Index is {index} and location is {scaledPath[index]}");
    //             index ++;
    //         }
    //     }
    //     // Set the last point to the ground.
    //     // scaledPath.Add(new Vector3(scaledPath[scaledPath.Count].x, 0, scaledPath[scaledPath.Count+1].z));
    //     /// Set the location of the drone to be the first location in the path.
    //     transform.position = scaledPath[pathIndex];
    // }
    public void applySliderValue(float value)
    {
        moveSpeed = value;
        Debug.Log($"Move speed: {moveSpeed}");
    }
    public void Restart()
    {
        SceneManager.LoadScene("SelectionScene");
        // Debug.Log("Game is restarting");
        MainManager.Instance.DestroyScreenshot();
    }
    public void Exit()
    {
        Debug.Log("Exiting this game");
        // // save any game data here
        // #if UNITY_EDITOR
        //     // Application.Quit() does not work in the editor so
        //     // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        //     UnityEditor.EditorApplication.isPlaying = false;
        // #else
        //     Application.Quit();
        // #endif
    }
}
