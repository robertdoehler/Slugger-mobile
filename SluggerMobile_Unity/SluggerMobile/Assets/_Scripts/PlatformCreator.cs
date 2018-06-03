using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlatformCreator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void createMesh()
    {

    }
}

[CustomEditor(typeof(PlatformCreator))]
public class PlatformCreatorInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlatformCreator platformCreator = (PlatformCreator)target;
        if (GUILayout.Button("Create Mesh"))
        {
            platformCreator.createMesh();
        }
    }
}
