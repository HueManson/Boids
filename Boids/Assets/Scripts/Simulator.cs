using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simulator : MonoBehaviour
{
    public LayerMask obsticleMask;
    public GameObject obsticlePrefab;
    public Vector2 obsticleScaleMinMax;
    public float scaleStepSize;
    public GameObject boidManager;

    [Header("TExt")]
    public Text onScreenHelp;
    public Text wallText;
    public Text seperationText;
    public Text alignmentText;
    public Text cohesionText;
    public Text helpText;
    public Text vizText;
    public Image textImage;

    Vector2 dragOffset;
    Transform draggedObject;
    bool dragging;
    bool toggleWalls;
    bool toggleMenu;
    GameObject[] walls;
    BoidsDriver driver;
    

    void Start()
    {
        walls = new GameObject[4];
        toggleWalls = true;
        toggleMenu = false;
        driver = boidManager.GetComponent<BoidsDriver>();

        seperationText.color = Color.red;
        alignmentText.color = Color.red;
        cohesionText.color = Color.red;
        wallText.color = Color.red;
        vizText.color = Color.red;
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

        if (Input.GetKeyDown(KeyCode.B))
        {
            if(toggleWalls)
            {
                BuildWalls();
                wallText.color = Color.green;
            }
            else
            {
                BreakWalls();
                wallText.color = Color.red;
            }
            toggleWalls = !toggleWalls;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            driver.seperation = !driver.seperation;
            seperationText.color = driver.seperation ? Color.green : Color.red; 
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            driver.alignment = !driver.alignment;
            alignmentText.color = driver.alignment ? Color.green : Color.red;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            driver.cohesion = !driver.cohesion;
            cohesionText.color = driver.cohesion ? Color.green : Color.red;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            driver.isVisualized = !driver.isVisualized;
            vizText.color = driver.isVisualized ? Color.green : Color.red;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if(toggleMenu)
            {
                helpText.text = "Hide help menu [press h]";
                onScreenHelp.text = "Add/drag obsticle [left click]\nRemove obsticle [right click]";
                wallText.text = "Toggle boundry [press b]";
                seperationText.text = "Toggle seperation [press s]";
                alignmentText.text =  "Toggle alignment [press a]";
                cohesionText.text = "Toggle cohesion [press c]";
                vizText.text = "Toggle fov [press f]";
                textImage.enabled = true;
            }
            else
            {
                helpText.text = "Show help menu [press h]";
                onScreenHelp.text = "";
                wallText.text = "";
                seperationText.text = "";
                alignmentText.text =  "";
                cohesionText.text = "";
                vizText.text = "";
                textImage.enabled = false;
            }
            toggleMenu = !toggleMenu;
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

    void BuildWalls()
    {
        Vector2 halfscreenWidthInWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        float thickness = 1f;
        Vector2 hotizontalPos = Vector2.zero;
        hotizontalPos.x = halfscreenWidthInWorldUnits.x + thickness/2;
        Vector3 horizontalScale = new Vector3(thickness, halfscreenWidthInWorldUnits.y * 2, 0);

        GameObject leftHorizontal = Instantiate(obsticlePrefab, -hotizontalPos, Quaternion.identity);
        GameObject rightHorizontal = Instantiate(obsticlePrefab, hotizontalPos, Quaternion.identity);
        leftHorizontal.transform.localScale = horizontalScale;
        rightHorizontal.transform.localScale = horizontalScale;

        Vector2 voticalPos = Vector2.zero;
        voticalPos.y = halfscreenWidthInWorldUnits.y + thickness/2;
        Vector3 verticalScale = new Vector3(halfscreenWidthInWorldUnits.x * 2, thickness, 0);

        GameObject upperVertical = Instantiate(obsticlePrefab, voticalPos, Quaternion.identity);
        GameObject lowwerVertical = Instantiate(obsticlePrefab, -voticalPos, Quaternion.identity);
        upperVertical.transform.localScale = verticalScale;
        lowwerVertical.transform.localScale = verticalScale;

        walls = new GameObject[] {leftHorizontal, rightHorizontal, upperVertical, lowwerVertical};
    }

    void BreakWalls()
    {
        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }

    }

}
