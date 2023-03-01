using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//required to be in editor folder
[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        //note :: change axis projections to rotate circle
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.right, 360, fov.viewRadius);

        Vector3 viewAngleA = fov.DirFromAngle2D(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle2D(fov.viewAngle / 2, false);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);
    }
}
