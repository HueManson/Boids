using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalBoid : MonoBehaviour
{
    public bool isVisualized;

    [Header("Flock Behavior")]
    public bool seperation;
    public bool alignment;
    public bool cohesion;
    public bool hasTarget;

    [Header("Boid Behavior")]
    public float moveSpeed;
    public float turnRadius;

    [Header("Visualization")]
    [Range(0, 360)]
    public float viewAngle;
    public float viewRadius;
    public float meshResolution;
    public float edgeDistThreashold;
    public int edgeResolution;

    public LayerMask targetMask;
    public LayerMask obsticleMask;

    public Material boidMaterial;
    public Material vizMaterial;

    Mesh boidMesh;
    MeshFilter boidMeshFilter;
    MeshRenderer boidRenderer;
    Camera boundingCamera;

    void Start()
    {
        //generate boid mesh
        boidMeshFilter = gameObject.AddComponent<MeshFilter>();
        boidRenderer = gameObject.AddComponent<MeshRenderer>();
        boidMesh = new Mesh();
        boidMesh.name = "BoidMesh";
        boidMeshFilter.mesh = boidMesh;
        boidRenderer.material = boidMaterial;
        GenerateBoidMesh();

        boundingCamera = Camera.main;

        Vizualize(targetMask, obsticleMask);
    }

    void Update()
    {
        Move(transform.up * moveSpeed);
    }

    void Move(Vector3 velocity)
    {
        transform.position += velocity * Time.deltaTime;

        if (seperation)
            seperate();
        if (alignment)
            align();
        if (cohesion)
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

    //A much simpler solution would be to set this all up in the prefab... but this was fun to experiment with
    void Vizualize(LayerMask boidMask, LayerMask obsticleMask)
    {
        GameObject vizualizer = new GameObject("Vizualizer");
        MeshFilter meshFilter = vizualizer.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = vizualizer.AddComponent<MeshRenderer>();
        vizualizer.transform.parent = this.transform;
        vizualizer.transform.localPosition = Vector3.zero;
        vizualizer.transform.localRotation = Quaternion.identity;
        meshRenderer.material = vizMaterial;

        FieldOfView fov = vizualizer.AddComponent<FieldOfView>();
        fov.viewAngle = viewAngle;
        fov.viewRadius = viewRadius;
        fov.meshResolution = meshResolution;
        fov.edgeDistThreashold = edgeDistThreashold;
        fov.edgeResolution = edgeResolution;
        fov.boidMask = boidMask;
        fov.obsticleMask = obsticleMask;
        fov.viewMeshFilter = meshFilter;
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
            viewportPos.x = Mathf.Clamp(0.5f - viewportPos.x, 0, 1);
        }
        if (viewportPos.y > 1 || viewportPos.y < 0)
        {
            viewportPos.y = Mathf.Clamp(0.5f - viewportPos.y, 0, 1);
        }

        Vector3 newPos = camera.ViewportToWorldPoint(viewportPos);
        transform.position = newPos;
    }
}
