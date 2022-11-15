using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolSpawner : MonoBehaviour
{
    [SerializeField] GameObject schoolMarker;
    private Vector3 offset; // offset position of the tip of the marker prefab
    public TextAsset SchoolNameData; // csv file with location data
    public Renderer map; // map renderer

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"MAPSIZE {map.bounds.size}");
        showSchools();
        Instantiate(schoolMarker, offset+new Vector3(0,0,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void spawnSchool(Vector3 position, Vector3 offset, string schoolName)
    {
        GameObject schoolObject = Instantiate(schoolMarker, offset+position, Quaternion.identity) as GameObject;  // instatiate the object
        schoolObject.transform.localScale = new Vector3(300f, 300f, 300f); // set size of the prefabs
        // set rotation of the prefabs
        schoolObject.transform.Rotate(270, 0, 0);
        schoolObject.tag = "School"; // set tag for the object
        // Set child text object to school name
        schoolObject.transform.GetChild(0).GetComponent<TextMesh>().text = schoolName;
        // LeanTween.moveX(schoolObject, schoolObject.transform.position.x, 5f).setEase(LeanTweenType.easeOutBounce);
        // LeanTween.scale(schoolObject, new Vector3(0.24f, 0.24f, 2.0f), 2.0f).setEase(LeanTweenType.easeInBounce);
        
        // schoolObject.schoolnamestring = 
    }

    void showSchools()
    {
        float tipPosition_y = 2.5f;
        Vector3 offset = new Vector3(0, tipPosition_y, 0); // location offset of the marker's tip


        string[] data = SchoolNameData.text.Split(new string[] {",", "\n"}, System.StringSplitOptions.None); //read the csv file
        for (int i = 0; i < data.Length; i++)
        {
            float x_f = 0;
            float x_g = 2224.6f;
            float x_d = -1.6f;
            float x_e = -1.28f;
            float y_f = 0f;
            float y_g = map.bounds.size.z;
            float y_d = 50.863096f;
            float y_e = 51.02f;

            // Scale the data to the grid bounds
            float x_pos = (x_f-x_g)/(x_d-x_e)*(float.Parse(data[3*i+1])-x_d)+x_f;

            float y_pos = (y_f-y_g)/(y_d-y_e)*(float.Parse(data[3*i+2])-y_d)+y_f;

            spawnSchool(new Vector3(x_pos, 0, y_pos), offset, data[i*3]); // instantiate prefabs at each of the locations
        }
        /*
        read in csv
        for every line in csv
            instantiate a school object
            set the schoolNameString to the name of the school
        -1.6, 50.863096, -1.28, 51.02
        */
    }
}
