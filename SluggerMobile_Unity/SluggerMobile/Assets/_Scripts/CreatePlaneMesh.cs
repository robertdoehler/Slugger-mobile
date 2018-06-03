using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CreatePlaneMesh : MonoBehaviour {

    public float width;
    public float height;

	// Use this for initialization
	void Start () {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        //Vertices
        Vector3[] verticies = new Vector3[4]
        {
            new Vector3(0,0,0), new Vector3(width, 0,0), new Vector3(0, height, 0), new Vector3(width, height, 0)
        };

        //Triangles
        int[] tris = new int[6];
        tris[0] = 0;
        tris[1] = 2;
        tris[2] = 1;
        tris[3] = 2;
        tris[4] = 3;
        tris[5] = 1;

        //Normals
        Vector3[] normals = new Vector3[4];
        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;

        //UVs (How Texture is displayed)
        Vector2[] uvs = new Vector2[4];

        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);
        uvs[2] = new Vector2(0, 1);
        uvs[3] = new Vector2(1, 1);

        mesh.vertices = verticies;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uvs;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
