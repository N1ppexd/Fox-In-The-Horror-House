using HorrorFox.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(HunterFov))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        HunterFov fov = (HunterFov)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.seeDistance);

    }
}
