using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeGenerator : MonoBehaviour
{
    public float lengthToRadiusRatio;
    public float scale;
    public int vertexCount;
    public Material coneMaterial;
    public LayerMask targetMask;

    Mesh coneMesh;
    MeshFilter coneMeshFilter;
    MeshRenderer coneRenderer;

    void Start()
    {
        //create child to hold mesh
        GameObject coneVizualizer = new GameObject("ConeVizualizer");
        coneVizualizer.transform.parent = this.transform;
        coneVizualizer.transform.localPosition = Vector3.zero;
        coneVizualizer.transform.localRotation = Quaternion.identity;

        //create mesh
        coneMeshFilter = coneVizualizer.AddComponent<MeshFilter>();
        coneRenderer = coneVizualizer.AddComponent<MeshRenderer>();
        coneMesh = new Mesh();
        coneMesh.name = "coneMesh";
        coneMeshFilter.mesh = coneMesh;
        coneRenderer.material = coneMaterial;

        GenerateConeMesh();
        coneVizualizer.AddComponent<MeshCollider>();
        //thank you https://answers.unity.com/questions/1288179/layer-layermask-which-is-set-in-inspector.html
        coneVizualizer.layer = (int) Mathf.Log(targetMask.value, 2);
    }

    void GenerateConeMesh()
    {
        //Vector3[] vertices = new Vector3[vertexCount];
        //int[] triangles = new int[(vertexCount - 2) * 3];

        ////viewmesh is child of thiss so all vertcies need to be in local space
        ////transform.position (global space) == Vector3.zero (local space)
        //vertices[0] = Vector3.zero;

        //for(int i = 0; i < vertexCount-1; i ++)
        //{
        //    float angle = i * (360 / vertexCount);
        //    float angleInRadians = angle / 180 * Mathf.PI;
        //    vertices[i + 1] = new Vector3(radius * Mathf.Cos(angleInRadians), radius * Mathf.Sin(angleInRadians), length);
        //    if (i < vertexCount - 2)
        //    {
        //        triangles[i * 3] = 0;
        //        //order dictates which way triangle is rendered (clockwise)
        //        triangles[(i * 3) + 1] = i + 1;
        //        triangles[(i * 3) + 2] = i + 2;
        //    }
        //}

        //coneMesh.Clear();
        //coneMesh.vertices = vertices;
        //coneMesh.triangles = triangles;
        //coneMesh.RecalculateNormals();

        coneMesh.Clear();
        coneMesh.vertices = new Vector3[]
        {
            Vector3.up * scale,
            (Vector3.right * 1/lengthToRadiusRatio - Vector3.up) * scale,
            (Vector3.left * 1/lengthToRadiusRatio - Vector3.up) * scale
        };
        coneMesh.triangles = new int[]
        {
            0, 1, 2
        };
        coneMesh.RecalculateNormals();
    }
}
