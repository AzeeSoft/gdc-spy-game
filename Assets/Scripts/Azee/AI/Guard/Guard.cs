using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Azee.Interfaces;
using Azee;
using GuardStates;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Utility;

public class Guard : MonoBehaviour
{
    [Header("PatrolCircuit")]
    public WaypointCircuitExtended DefaultPatrolCircuit;

    [Header("AISight")]
    public float MaxSightAngle;

    public readonly Patrol.StateData PatrolStateData;
    public readonly Chase.StateData ChaseStateData;

    private readonly StateMachine<Guard> _stateMachine;
    private GuardsManager _guardsManager;
    private NavMeshAgent _navMeshAgent;

    public Guard()
    {
        _stateMachine = new StateMachine<Guard>(this);

        PatrolStateData = new Patrol.StateData {IsMoving = false, TargetWaypointIndex = -1};
        ChaseStateData = new Chase.StateData {TargetTransform = null};
    }

    void Awake()
    {
        _guardsManager = FindObjectOfType<GuardsManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _stateMachine.SwitchState(GuardStates.Idle.Instance);
    }

    // Use this for initialization
    void Start()
    {
        if (DefaultPatrolCircuit)
        {
            _stateMachine.SwitchState(GuardStates.Patrol.Instance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update();
    }


    /***************************
     * States Helper Functions *
     ***************************/

    public bool IsObjectInSight(GameObject otherObject)
    {
        Vector3 playerDir = (otherObject.transform.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, playerDir) < MaxSightAngle)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, playerDir, out raycastHit))
            {
                if (raycastHit.transform.gameObject == otherObject)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public Vector3? LocateObjectFromNoise(AudibleObject audibleObject)
    {
        return audibleObject.LocateFromNoise(gameObject);
    }

    public void StartChasing(Transform chaseTargetTransform)
    {
        _stateMachine.SwitchState(GuardStates.Chase.Instance, chaseTargetTransform);
    }

    public void MoveTowards(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }

    public void StopMoving()
    {
        _navMeshAgent.ResetPath();
    }

    public StateMachine<Guard> GetStateMachine()
    {
        return _stateMachine;
    } 

}