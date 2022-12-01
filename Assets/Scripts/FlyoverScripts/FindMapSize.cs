using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMapSize : MonoBehaviour
{
    public Vector2 mapSize;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Map size: " + FindSize());
        mapSize = FindSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 FindSize()
    {
        Vector3 size = GetComponent<Renderer>().bounds.size;
        return new Vector2(size.x, size.z);
    }
}
