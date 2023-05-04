using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsDriver : MonoBehaviour
{
    public int boidCount;
    public bool isVisualized;

    public GameObject boidPrefab;

    [Header("Flock Behavior")]
    public bool seperation;
    public bool alignment;
    public bool cohesion;
    public bool hasTarget;

    [Header("Boid Behavior")]
    public float moveSpeed;
    public float turnRadius;
    public float viewAngle;
    public float viewRadius;

    [Header("Foce Weights")]
    public float avoidForceWeight;
    public float seperationForceWeight;
    public float alignmentForceWeight;
    public float cohesionForceWeight;

    BoidInstance[] boids;
    Vector2 halfscreenWidthInWorldUnits;
    FieldOfView visualizedBoid;
    bool seeing;

    void Start()
    {
        halfscreenWidthInWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        boids = new BoidInstance[boidCount];

        for (int i = 0; i < boidCount; i++)
        {
            GameObject boid = Instantiate(boidPrefab, new Vector2(Random.Range(-halfscreenWidthInWorldUnits.x, halfscreenWidthInWorldUnits.x), Random.Range(-halfscreenWidthInWorldUnits.y, halfscreenWidthInWorldUnits.y)), Quaternion.Euler(0f, 0f, Random.Range(0, 360)));
            boid.transform.parent = transform;

            BoidInstance boidInstance = new BoidInstance(boid.GetComponent<Boid>(), boid.GetComponent<FieldOfView>(), boid);
            boids[i] = boidInstance;
        }
    }

    void Update()
    {
        if(isVisualized && !seeing)
        {
            visualizedBoid = boids[Random.Range(0, boids.Length)].fov;
            visualizedBoid.isVisualized = true;
            seeing = true;
        }
        else if(!isVisualized && seeing)
        {   
            visualizedBoid.isVisualized = false;
            seeing = false;
        }

        foreach(BoidInstance boid in boids)
        {
            UpdateBoid(boid);
        }
    }

    void UpdateBoid(BoidInstance boid)
    {
        Vector3 acceleration = Vector3.zero;
        Vector3 velocity = boid.boidObject.transform.up * moveSpeed;

        if(boid.fov.visibleTargets.Count != 0)
        {
            if(seperation)
            {
                Vector3 seperationForce = boid.boidBehavior.Seperate(boid.fov.visibleTargets) * turnRadius;
                acceleration += seperationForce * seperationForceWeight;
            }
            if (alignment)
            {
                Vector3 alignmentForce = boid.boidBehavior.Align(boid.fov.visibleTargets) * turnRadius;
                acceleration += alignmentForce *alignmentForceWeight;
            }
            if(cohesion && boid.fov.visibleTargets.Count > 0)
            {
                Vector3 cohesionForce = boid.boidBehavior.Cohere(boid.fov.visibleTargets) * turnRadius;
                acceleration += cohesionForce * cohesionForceWeight;
            }
        }

        if(boid.fov.HeadingForCollision())
        {
            Vector3 avoidForce = SteerTowards(velocity, boid.fov.FindClearPath());
            acceleration += avoidForce * avoidForceWeight;
        }

        velocity += acceleration * Time.deltaTime;
        boid.boidBehavior.Move(velocity.normalized * moveSpeed);
    }

    Vector3 SteerTowards(Vector3 currentVelocity, Vector3 targetVelocity)
    {
        Vector3 steerForceDir = targetVelocity.normalized * currentVelocity.magnitude - currentVelocity;
        return steerForceDir.normalized;
    }

    public struct BoidInstance
    {
        public Boid boidBehavior;
        public FieldOfView fov;
        public GameObject boidObject;

        public BoidInstance(Boid boidBehavior_, FieldOfView fov_, GameObject boidObject_)
        {
            boidBehavior = boidBehavior_;
            fov = fov_;
            boidObject = boidObject_;
        }
    }
}
