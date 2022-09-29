using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphTween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(gameObject, new Vector3(0.62f, 0.62f, 0.62f), 1f).setEase(LeanTweenType.easeOutBounce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
