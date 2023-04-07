using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.DrawWireArc(fow.transform.position, fow.transform.up, fow.transform.forward, 360f, fow.viewRadius);
        var viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2 - 90f, false);
        var viewAngleB = fow.DirFromAngle(fow.viewAngle / 2 - 90f, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        foreach (var visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.transform.position);
        }
    }
}
