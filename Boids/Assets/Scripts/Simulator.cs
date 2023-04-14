using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public LayerMask obsticleMask;

    Vector2 halfscreenWidthInWorldUnits;
    Vector2 dragOffset;
    Transform draggedObject;
    bool dragging;

    void Start()
    {
        Vector2 halfscreenWidthInWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
    }

    void Update()
    {
        HandleInput(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void HandleInput(Vector2 mousePos)
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if (MouseOverObsticle(out hit))
            {
                Destroy(hit.transform.gameObject);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (!MouseOverObsticle(out hit))
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            }
            else
            {
                dragOffset = new Vector2(mousePos.x - hit.transform.position.x, mousePos.y - hit.transform.position.y);
                draggedObject = hit.transform;
                dragging = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (dragging)
            {
                draggedObject.position = mousePos - dragOffset;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
    }

    bool MouseOverObsticle(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, obsticleMask))
        {
            return true;
        }
        return false;
    }
}
