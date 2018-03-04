using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Utility;

public class WaypointCircuitExtended : WaypointCircuit
{
    public int FindNearestWaypointIndex(Vector3 position)
    {
        float closestDistance = float.MaxValue;

        int closestIndex = -1;

        for (int i = 0; i < Waypoints.Length; i++)
        {
            float distance = Vector3.Distance(Waypoints[i].position, position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    public int GetNextWaypointIndex(int currentIndex)
    {
        if (Waypoints.Length > 0)
        {
            int nextIndex = currentIndex + 1;
            if (nextIndex >= Waypoints.Length)
            {
                nextIndex = 0;
            }

            return nextIndex;
        }

        return -1;
    }

    public Transform GetWaypoint(int currentIndex)
    {
        if (currentIndex >= 0 && currentIndex < Waypoints.Length)
        {
            return Waypoints[currentIndex];
        }

        return null;
    }
    
}