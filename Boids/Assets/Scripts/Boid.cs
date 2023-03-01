using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    Camera boundingCamera;

    public float moveSpeed = 5f;

    void Start()
    {
        boundingCamera = Camera.main;
    }

    void Update()
    {
        Move(transform.right * moveSpeed);
    }

    void Move(Vector3 velocity)
    {
        transform.position += velocity * Time.deltaTime;
    }

    //note :: the editor camera will activate these functions (any camera)
    public void OnBecameInvisible()
    {
        //get position relative to camera
        Debug.Log(">>>>>> invisibleb warp <<<<<<<<<<<<<<<<<");
        ViewportWarp(this.transform, boundingCamera);
    }

    //note :: the editor camera will activate these functions (any camera)
    public void OnBecameVisible()
    {
        Debug.Log(">>>>>>> visible <<<<<<<<<<<<<<<<<<<<");
    }

    void ViewportWarp(Transform transform, Camera camera)
    {
        if (camera == null)
            return;

        Vector3 viewportPos = camera.WorldToViewportPoint(transform.position);

        //note :: everything in the cameras viewport in 0-1 (with pos being < 0 or > 1 indicating it is off screne) (.5, .5) is the cameras center.
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
}
