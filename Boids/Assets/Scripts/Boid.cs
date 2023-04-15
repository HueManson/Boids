using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float moveSpeed;
    public float turnRadius;
    public bool hasTarget;

    [Header("Boid Behavior")]
    public bool seperation;
    public bool alignment;
    public bool cohesion;

    Camera boundingCamera;

    void Start()
    {
        boundingCamera = Camera.main;
    }

    void Update()
    {
        Move(transform.up * moveSpeed);
    }

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

    //OnBecomeInvisible -- requires renderer to be triggered
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
            viewportPos.x = 1 - viewportPos.x;
        }
        if (viewportPos.y > 1 || viewportPos.y < 0)
        {
            viewportPos.y = 1 - viewportPos.y;
        }

        Vector3 newPos = camera.ViewportToWorldPoint(viewportPos);
        transform.position = newPos;
    }
}
