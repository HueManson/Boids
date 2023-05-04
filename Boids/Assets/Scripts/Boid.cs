using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float turnRadius;
    public bool hasTarget;

    Camera boundingCamera;

    void Start()
    {
        boundingCamera = Camera.main;
    }

    public void Move(Vector3 velocity)
    {
        transform.up = velocity.normalized;
        transform.position += velocity * Time.deltaTime;
    }

    public Vector3 Seperate(List<Transform> visibleTargets)
    {
        Vector3 seperationDir = Vector3.zero;
        float minDist = float.MaxValue;
        Vector3 minDir = Vector3.zero;

        foreach(Transform target in visibleTargets)
        {
            float dist = (target.position - transform.position).magnitude;
            if(dist < minDist)
            {
                minDist = dist;
                minDir = target.position - transform.position;
            }
            seperationDir -= (target.position - transform.position) * 1/dist;
        }
        //break clumping
        seperationDir += minDir;
        seperationDir.z = 0;
        
        return seperationDir;
    }
    public Vector3 Align(List<Transform> visibleTargets)
    {
        Vector3 averageDir = Vector3.zero;

        foreach(Transform target in visibleTargets)
        {
            averageDir += target.up;
        }

        return averageDir / visibleTargets.Count;
    }
    public Vector3 Cohere(List<Transform> visibleTargets)
    {   
        Vector3 avgCenter = Vector3.zero;

        foreach(Transform target in visibleTargets)
        {
            avgCenter += target.position;
        }
        
        avgCenter /= visibleTargets.Count;
        avgCenter.z = 0;//avg center from origin

        return avgCenter - transform.position;//subtract to get avgCenter from current boid
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
    //thank you https://forum.unity.com/threads/rotate-object-on-z-axis-to-look-at-vector3.444718/
    public static float SignedAngleTo(Vector3 fromVector, Vector3 toVector, Vector3 relativeUp) {
     return Mathf.Atan2(
       Vector3.Dot(relativeUp.normalized, Vector3.Cross(fromVector, toVector)),
       Vector3.Dot(fromVector, toVector)) * Mathf.Rad2Deg;
   }
}
