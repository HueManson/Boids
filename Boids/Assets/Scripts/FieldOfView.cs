using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obsticleMask;

    void FindVisibileTargets()
    {

    }

    //see this-https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague for why sin and cos are swapped
    public Vector3 DirFromAngle(float angleInDeg, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            //add the objects own rotation to it
            angleInDeg += transform.eulerAngles.z;
        }
        //note :: change order of vector to change axis angles are projected onto
        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), Mathf.Cos(angleInDeg * Mathf.Deg2Rad), 0);
    }

    public Vector2 DirFromAngle2D(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }
}
