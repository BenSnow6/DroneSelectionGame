using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapFollow : MonoBehaviour
{

    public Image DroneTracker;
    // Start is called before the first frame update
    void Start()
    {
        // Set drone tracker loaction to the first location in the path.
        DroneTracker.transform.position = new Vector3(MainManager.Instance.clickedLocations[0].x, MainManager.Instance.clickedLocations[0].y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Drone location: " + DroneTracker.transform.position);
    }
}
