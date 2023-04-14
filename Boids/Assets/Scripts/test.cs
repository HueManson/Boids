using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Mesh boidMesh;
    MeshFilter boidMeshFilter;

    public LayerMask mask1;
    public LayerMask mask2;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        boidMesh = new Mesh();
        boidMesh.name = "BoidMesh";
        boidMeshFilter = GetComponent<MeshFilter>();
        boidMeshFilter.mesh = boidMesh;
        GenerateBoidMesh();

        Vizualize(mask1, mask2);
    }

    void GenerateBoidMesh()
    {
        boidMesh.Clear();
        boidMesh.vertices = new Vector3[]
        {
            new Vector3(0,.5f,0),
            new Vector3 (.2f, -.25f,0),
            new Vector3 (-.2f,-.25f,0)
        };
        boidMesh.triangles = new int[]
        {
            0, 1, 2
        };
        boidMesh.RecalculateNormals();
    }

    void Vizualize(LayerMask boidMask, LayerMask obsticleMask)
    {
        GameObject vizualizer = new GameObject("Vizualizer");
        vizualizer.AddComponent<MeshFilter>();
        vizualizer.AddComponent<MeshRenderer>();
        vizualizer.transform.parent = this.transform;
        vizualizer.transform.localPosition = Vector3.zero;
        vizualizer.transform.localRotation = Quaternion.identity;

        vizualizer.AddComponent<FieldOfView>();
        FieldOfView fov = vizualizer.GetComponent<FieldOfView>();
        fov.viewAngle = 60f;
        fov.viewRadius = 3.85f;
        fov.meshResolution = 1f;
        fov.edgeDistThreashold = .5f;
        fov.edgeResolution = 6;
        fov.boidMask = boidMask;
        fov.obsticleMask = obsticleMask;
        fov.viewMeshFilter = vizualizer.GetComponent<MeshFilter>();
    }
}
