using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGem : MonoBehaviour
{
    public Transform child;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the gem without rotating the child object
        transform.Rotate(new Vector3(0, 0, 100f)* Time.deltaTime);
        child.transform.rotation = Quaternion.Euler (0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
    }
}
