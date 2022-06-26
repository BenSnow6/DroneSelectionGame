using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMapSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Map size: " + FindSize());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 FindSize()
    {
        Vector3 size = GetComponent<Renderer>().bounds.size;
        return size;
    }
}
