using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Boid : MonoBehaviour
{
    Camera boundingCamera;

    public float moveSpeed;
    public float turnRadius;
    public bool onDisplay;
    public bool hasTarget;

    [Header("Boid Behavior")]
    public bool seperation;
    public bool alignment;
    public bool cohesion;

    Mesh boidMesh;
    MeshFilter boidMeshFilter;


    void Start()
    {
        boundingCamera = Camera.main;
        //boidMeshFilter = GetComponent<MeshFilter>();
        boidMesh = new Mesh();
        boidMesh.name = "BoidMesh";
        boidMeshFilter.mesh = boidMesh;
        //GenerateBoidMesh();
    }

    void Update()
    {
        Move(transform.up * moveSpeed);
    }

    //Move
    //Boid behavior
    //#1    seperation::avoid collision with nearby boids
    //#2    alignment::move in the same direction as nearby boids
    //#3    cohesion::aim for the center of the group
    void Move(Vector3 velocity)
    {
        transform.position += velocity * Time.deltaTime;

        if(seperation)
            seperate();
        if (alignment)
            align();
        if(cohesion)
            cohere();
    }

    public void seperate()
    {

    }
    public void align()
    {

    }
    public void cohere()
    {

    }

    //OnBecomeInvisible
    //note::the editor camera will activate OnBecameInvisible() and OnBecameVisible() (any camera will trigger them)
    public void OnBecameInvisible()
    {
        ViewportWarp(this.transform, boundingCamera);
    }

    void ViewportWarp(Transform transform, Camera camera)
    {
        if (camera == null)
            return;

        Vector3 viewportPos = camera.WorldToViewportPoint(transform.position);

        //note::everything in the cameras viewport in [0,1] (with pos being < 0 or > 1 indicating it is off screne) (.5, .5) is the cameras center.
        if (viewportPos.x > 1 || viewportPos.x < 0)
        {
            viewportPos.x = Mathf.Clamp(0.5f - viewportPos.x, 0,1);
        }
        if (viewportPos.y > 1 || viewportPos.y < 0)
        {
            viewportPos.y = Mathf.Clamp(0.5f - viewportPos.y, 0,1);
        }

        Vector3 newPos = camera.ViewportToWorldPoint(viewportPos);
        transform.position = newPos;
    }

    void GenerateBoidMesh()
    {

        boidMesh.Clear();
        boidMesh.vertices = new Vector3[]
        {
            Vector3.zero,
            Vector3.right,
            Vector3.up
        };
        boidMesh.triangles = new int[]
        {
            0, 1, 2
        };
        boidMesh.RecalculateNormals();
    }
}
