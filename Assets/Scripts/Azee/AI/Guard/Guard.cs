using System.Collections;
using System.Collections.Generic;
using GuardStates;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Utility;

public class Guard : MonoBehaviour
{
    public readonly StateMachine<Guard> StateMachine;
    public WaypointCircuitExtended DefaultPatrolCircuit;

    public readonly Patrol.StateData PatrolStateData;

    private GuardsManager _guardsManager;
    private NavMeshAgent _navMeshAgent;

    public Guard()
    {
        StateMachine = new StateMachine<Guard>(this);
        PatrolStateData = new Patrol.StateData {IsMoving = false, TargetWaypointIndex = -1};
    }

    void Awake()
    {
        _guardsManager = FindObjectOfType<GuardsManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

	// Use this for initialization
	void Start ()
	{
		StateMachine.SwitchState(GuardStates.Patrol.Instance);
	}
	
	// Update is called once per frame
	void Update () {
		StateMachine.Update();
	}


    // Patrol Helper Functions
    public void PatrolTowards(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }
}
