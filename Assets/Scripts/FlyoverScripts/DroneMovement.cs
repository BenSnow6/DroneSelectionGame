using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    Vector3Int[] path = new Vector3Int[] {new Vector3Int(0,0,0), new Vector3Int(0,50,0), new Vector3Int(1000,50,1000), new Vector3Int(2000,50,1000), new Vector3Int(2000,0,1000)};
    int pathIndex = 0;
    [SerializeField] float moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = path[pathIndex];
    }

    // Update is called once per frame
    void Update()
    {
     Move();   
    }

    void Move()
    {
        /// <summary>
        /// Moves the drone along the path from one waypoint to the next.
        /// </summary>

        transform.position = Vector3.MoveTowards(transform.position, path[pathIndex], moveSpeed * Time.deltaTime);

        if (transform.position == path[pathIndex])
        {
            pathIndex++;
        }
        if (pathIndex == path.Length)
        {
            return;
        }
    }
}
