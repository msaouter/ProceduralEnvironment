using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PoissonWindow : EditorWindow
{
    /* Poisson disk arguments */

    private float x = 250, y, z = 250;
    public Vector3 regionSize;
    public float yMin = (float)0.2;

    public float radiusBetweenObject = 10;

    public float samplesBeforeRejection = 30;

    public bool spaceDimension = false; /* Allow to make 3D distribution */

    GameObject parent = null;
    GameObject prefab = null;

    public List<Vector3> list3D;



    [MenuItem("Window/EnvironmentGen")]
    public static void ShowWindow()
    {
        GetWindow<PoissonWindow>("Environment Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Parameters", EditorStyles.boldLabel);

        //GUILayout.Label
        radiusBetweenObject = EditorGUILayout.FloatField("Radius between objects", radiusBetweenObject);
        samplesBeforeRejection = EditorGUILayout.FloatField("Samples before rejection", samplesBeforeRejection);
        GUILayout.Label("The greater this number is, the longer the execution will take.", EditorStyles.miniLabel);
        GUILayout.Space(10);

        spaceDimension = EditorGUILayout.Toggle("3D placement ?", spaceDimension);
        GUILayout.Label("Toggle this if you wish to place objects in the air. \nExample : fireflies", EditorStyles.miniLabel);

        GUILayout.Space(10);
        GUILayout.Label("Region size");
        EditorGUI.indentLevel++;
        x = EditorGUILayout.FloatField("X", x);
        
        if (spaceDimension)
        {
            y = EditorGUILayout.FloatField("Y", y);
            yMin = EditorGUILayout.FloatField("Y min", yMin);
        }
        else
        {
            y = 0;
        }

        z = EditorGUILayout.FloatField("Z", z);
        regionSize = new Vector3(x, y, z);


        EditorGUI.indentLevel--;

        GUILayout.Space(10);

        parent = (GameObject)EditorGUILayout.ObjectField("Parent", parent, typeof(GameObject), true);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        GUILayout.Space(20);

        if(GUILayout.Button("Start generation"))
        {
            if (spaceDimension)
            {
                Debug.Log("3D generation started");
                list3D = PoissonDistribution.PointGeneration3D(radiusBetweenObject, regionSize, samplesBeforeRejection);

                /*foreach (Vector3 value in list3D)
                {
                    Instantiate(prefab, value + parent.GetComponent<Transform>().position, Quaternion.identity, parent.GetComponent<Transform>());
                }

                Debug.Log("Generation finished, Number of object generated :" + list3D.Count);*/
            }

            else
            {
                Debug.Log("2D generation started");
                list3D = PoissonDistribution.PointGeneration2D(radiusBetweenObject, regionSize, samplesBeforeRejection);
            } 

            foreach (Vector3 value in list3D)
            {
                Instantiate(prefab, value + parent.GetComponent<Transform>().position, Quaternion.identity, parent.GetComponent<Transform>());
            }
            Debug.Log("Generation finished, Number of object generated :" + list3D.Count);
        }
    }
}
