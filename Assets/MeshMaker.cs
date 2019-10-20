using System.Collections.Generic;
using UnityEngine;

public class MeshMaker : MonoBehaviour {
    public GameObject[] sphere;
    private Vector3[] point; 
    private Mesh mesh;
    private MeshFilter meshFilter;

    private void Start() {
        point = new Vector3[sphere.Length];
        for (int i = 0; i < sphere.Length; i++) point[i] = sphere[i].gameObject.transform.position;

        mesh = new Mesh();
        List<Vector3> verticles = new List<Vector3>();
        for (int i = 0; i < point.Length; i++) verticles.Add(point[i]);
        mesh.SetVertices(verticles);

        List<int> triangles = new List<int>();
        for (int i = 0; i < point.Length; i++) triangles.Add(i);
        mesh.SetTriangles(triangles, 0);

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private void Update() {
    }
}
