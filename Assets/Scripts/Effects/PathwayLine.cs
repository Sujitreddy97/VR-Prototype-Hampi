using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathwayLine : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private LineRenderer lineRenderer;
    void Start()
    {
        DrawPath();
    }


    void DrawPath()
    {
        if (waypoints.Length == 0) return;

        lineRenderer.positionCount = waypoints.Length;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, waypoints[i].position);
        }
    }
}
