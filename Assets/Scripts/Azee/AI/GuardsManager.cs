using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Utility;

class GuardsManager : MonoBehaviour
{
    public Guard[] Guards;

    public WaypointCircuitExtended[] WaypointCircuits;

    void Awake()
    {
        FindGuardsInScene();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void FindGuardsInScene()
    {
        Guards = FindObjectsOfType<Guard>();
    }
}
