using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public LayerMask obsticleMask;
    public GameObject obsticlePrefab;
    public Vector2 obsticleScaleMinMax;
    public float scaleStepSize;

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
                GenerateObsticle(mousePos);
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

    void GenerateObsticle(Vector2 position)
    {
        GameObject obsticle = Instantiate(obsticlePrefab, position, Quaternion.Euler(0f, 0f, Random.Range(0, 360)));



        obsticle.transform.localScale = new Vector3(RandomStep(obsticleScaleMinMax.x, obsticleScaleMinMax.y, scaleStepSize), RandomStep(obsticleScaleMinMax.x * 2, obsticleScaleMinMax.y * 2, scaleStepSize), 1f);
        obsticle.transform.parent = this.transform;//purely to keep the editor clean
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

    float RandomStep(float min, float max, float stepSize)
    {
        float random = Random.Range(min, max);
        float numSteps = Mathf.Floor(random / stepSize);
        float adjustedRandom = numSteps * stepSize;

        return adjustedRandom;
    }

}
