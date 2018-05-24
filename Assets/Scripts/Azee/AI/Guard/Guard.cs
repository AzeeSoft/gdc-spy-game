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
    public readonly Stunned.StateData StunnedStateData;

    private readonly StateMachine<Guard> _stateMachine;
    private GuardsManager _guardsManager;
    private NavMeshAgent _navMeshAgent;
    private InteractiveObject _interactiveObject;

    private List<StateMachine<Guard>.State> _nonStunnableStates = new List<StateMachine<Guard>.State>(); 

    public Guard()
    {
        _stateMachine = new StateMachine<Guard>(this);
        _stateMachine.AddOnStateSwitchedCallback(OnStateSwitched);

        _nonStunnableStates.Add(Chase.Instance);
        _nonStunnableStates.Add(Stunned.Instance);

        PatrolStateData = new Patrol.StateData {IsMoving = false, TargetWaypointIndex = -1};
        ChaseStateData = new Chase.StateData {TargetTransform = null};
        StunnedStateData = new Stunned.StateData();
    }

    void Awake()
    {
        _guardsManager = FindObjectOfType<GuardsManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _interactiveObject = GetComponent<InteractiveObject>();

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

    void OnStateSwitched(StateMachine<Guard> stateMachine)
    {
        _interactiveObject.ToggleInteraction(0, !_nonStunnableStates.Contains(_stateMachine.GetCurrentState()));
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

    public void Stun()
    {
        if (!_nonStunnableStates.Contains(_stateMachine.GetCurrentState()))
        {
            _stateMachine.SwitchState(GuardStates.Stunned.Instance);
        }
    }

    public StateMachine<Guard> GetStateMachine()
    {
        return _stateMachine;
    } 

}