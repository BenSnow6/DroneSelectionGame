using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SchoolMarker : MonoBehaviour

{
    [SerializeField] GameObject schoolMarker;
    private Vector3 offset; // offset position of the tip of the marker prefab
    public TextAsset SchoolNameData; // csv file with location data

    // Start is called before the first frame update
    void Start()
    {
        float tipPosition_y = 0.256f-(256-222)/256; // 256-222 = 34 pixels from the bottom of the image
        Vector3 offset = new Vector3(0, tipPosition_y, 2.73958f); // location offset of the marker's tip


        string[] data = SchoolNameData.text.Split(new string[] {",", "\n"}, System.StringSplitOptions.None); //read the csv file
        for (int i = 0; i < data.Length; i++)
        {
            float x_f = 0.0f;
            float x_g = 10.0f;
            float x_d = -1.6f;
            float x_e = -1.28f;
            float y_f = 0.1f;
            float y_g = 8.0f;
            float y_d = 50.863096f;
            float y_e = 51.02f;

            // Scale the data to the grid bounds
            float x_pos = (x_f-x_g)/(x_d-x_e)*(float.Parse(data[3*i+1])-x_d)+x_f;

            float y_pos = (y_f-y_g)/(y_d-y_e)*(float.Parse(data[3*i+2])-y_d)+y_f;

            spawnSchool(new Vector3(x_pos, y_pos, 0), data[i*3]); // instantiate prefabs at each of the locations
        }
        /*
        read in csv
        for every line in csv
            instantiate a school object
            set the schoolNameString to the name of the school
         -1.6, 50.863096, -1.28, 51.02
        */
        
    }


    void spawnSchool(Vector3 position, string schoolName)
    {
        GameObject schoolObject = Instantiate(schoolMarker, offset+position, Quaternion.identity) as GameObject;  // instatiate the object
        schoolObject.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f); // set size of the prefabs
        // LeanTween.moveX(schoolObject, schoolObject.transform.position.x, 5f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.scale(schoolObject, new Vector3(0.24f, 0.24f, 2.0f), 2.0f).setEase(LeanTweenType.easeInBounce);
        
        // schoolObject.schoolnamestring = 
    }
}


