﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetwork_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target;
        network.DisplayMode = (PathDisplayMode)EditorGUILayout.EnumPopup("Display Mode", network.DisplayMode);
        if (network.DisplayMode == PathDisplayMode.Paths)
        {
            network.UIStart = EditorGUILayout.IntSlider("Waypoint Start", network.UIStart, 0, network.Waypoints.Count - 1);
            network.UIEnd = EditorGUILayout.IntSlider("Waypoint End", network.UIEnd, 0, network.Waypoints.Count - 1);
        }
        base.OnInspectorGUI();
    }

    void OnSceneGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target;

        for (int i = 0; i < network.Waypoints.Count; i++)
        {
            if (network.Waypoints[i] != null)
                Handles.Label(network.Waypoints[i].position, "Waypoints " + i);
            else
                return;
        }

        if (network.DisplayMode == PathDisplayMode.Connections)
        {
            Vector3[] linePoints = new Vector3[network.Waypoints.Count + 1];
            for (int i = 0; i <= network.Waypoints.Count; i++)
            {
                int index = i != network.Waypoints.Count ? i : 0;
                if (network.Waypoints[index] != null)
                    linePoints[i] = network.Waypoints[index].position;
                else
                    linePoints[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
            }

            Handles.color = Color.cyan;
            Handles.DrawPolyLine(linePoints);
        }
        else if (network.DisplayMode == PathDisplayMode.Paths)
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();

            if (network.Waypoints[network.UIStart] != null && network.Waypoints[network.UIEnd] != null)
            {
                Vector3 from = network.Waypoints[network.UIStart].position;
                Vector3 to = network.Waypoints[network.UIEnd].position;

                UnityEngine.AI.NavMesh.CalculatePath(from, to, UnityEngine.AI.NavMesh.AllAreas, path);
                Handles.color = Color.yellow;
                Handles.DrawPolyLine(path.corners);
            }


        }
        else if (network.DisplayMode == PathDisplayMode.None)
        {

        }

    }

}
