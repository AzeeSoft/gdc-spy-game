﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Azee.Interfaces;
using Azee;
using GuardStates;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Utility;

public class Guard : MonoBehaviour
{
    [Header("PatrolCircuit")] public WaypointCircuitExtended DefaultPatrolCircuit;

    [Header("AISight")] public Transform SightOriginTransform;

    public float MaxSightAngle;

    public float PatrolSpeed = 5f;
    public float ChaseSpeed = 7.5f;

    public float LostDistance = 8f;

    public float MaxInfection = 5f;
    public float MaxInfectionRadius = 3f;

    [SerializeField] private ColorChanger _colorChanger;

    public readonly Patrol.StateData PatrolStateData;
    public readonly Chase.StateData ChaseStateData;
    public readonly Stunned.StateData StunnedStateData;

    private readonly StateMachine<Guard> _stateMachine;
    private GuardsManager _guardsManager;
    private NavMeshAgent _navMeshAgent;
    private InteractiveObject _interactiveObject;
    private AudioController _audioController;

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

    void OnDrawGizmos()
    {
        DebugExtension.DrawCone(transform.position, transform.forward * 10, Color.blue, MaxSightAngle);

        DebugExtension.DebugWireSphere(transform.position, Color.red, MaxInfectionRadius);
    }

    void Awake()
    {
        _guardsManager = FindObjectOfType<GuardsManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _interactiveObject = GetComponent<InteractiveObject>();
        _audioController = GetComponent<AudioController>();

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
        if (Time.timeScale > 0)
        {
            _stateMachine.Update();
        }
    }

    void OnEnable()
    {
        RestoreGuardData();
    }

    void OnDisable()
    {
        SaveGuardData();

        GetColorChanger().TurnColor(Color.black);
    }

    void OnStateSwitched(StateMachine<Guard> stateMachine)
    {
        _interactiveObject.ToggleInteraction(0, !_nonStunnableStates.Contains(_stateMachine.GetCurrentState()));
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return _navMeshAgent;
    }


    /***************************
     * States Helper Functions *
     ***************************/

    public bool IsObjectInSight(GameObject otherObject)
    {
        Vector3 guardToObject = (otherObject.transform.position - SightOriginTransform.position);
        Vector3 objectDir = guardToObject.normalized;

        if (Vector3.Angle(SightOriginTransform.forward, objectDir) < MaxSightAngle)
        {
            int layerMask = -5; //All layers

            RaycastHit raycastHit;
            if (Physics.Raycast(SightOriginTransform.position, objectDir, out raycastHit, float.MaxValue, layerMask,
                QueryTriggerInteraction.Ignore))
            {
                Debug.Log("First object in Guard's sight: " + raycastHit.transform.gameObject);

                if (raycastHit.transform.gameObject == otherObject)
                {
                    Debug.DrawLine(SightOriginTransform.position, otherObject.transform.position, Color.red);
                    return true;
                }
            }
        }

        Debug.DrawLine(SightOriginTransform.position, otherObject.transform.position, Color.blue);
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

    public bool MoveTowards(Vector3 position)
    {
        return _navMeshAgent.SetDestination(position);
    }

    public void StopMoving()
    {
        _navMeshAgent.ResetPath();
    }

    public void Stun(ActionController actionController)
    {
        if (actionController.SpecialActionControllerTag == ActionController.SpecialActionController.Player)
        {
            Player player = actionController.GetComponent<Player>();
            if (player != null)
            {
                if (player.CanStun())
                {
                    if (!_nonStunnableStates.Contains(_stateMachine.GetCurrentState()))
                    {
                        _stateMachine.SwitchState(GuardStates.Stunned.Instance);
                        player.ResetStunBar();
                    }
                }
            }
        }
    }

    public ColorChanger GetColorChanger()
    {
        return _colorChanger;
    }

    public StateMachine<Guard> GetStateMachine()
    {
        return _stateMachine;
    }

    public AudioController GetAudioController()
    {
        return _audioController;
    }



    private class GuardData
    {
        public Vector3? NavMeshDestination = null;
        public Color GuardColor = Color.yellow;
    }
    private GuardData _guardData = null;

    void SaveGuardData()
    {
        if (_guardData == null)
        {
            _guardData = new GuardData();

            if (_navMeshAgent.hasPath)
            {
                _guardData.NavMeshDestination = _navMeshAgent.destination;
            }

            _guardData.GuardColor = GetColorChanger().GetColor();
        }
    }

    void RestoreGuardData()
    {
        if (_guardData != null)
        {
            if (_guardData.NavMeshDestination != null)
            {
                _navMeshAgent.SetDestination(_guardData.NavMeshDestination.Value);
            }

            GetColorChanger().TurnColor(_guardData.GuardColor);

            _guardData = null;
        }
    }


    /**********************************
     * Scene Switch Handler Functions *
     **********************************/

    public void OnScenePaused()
    {
        SaveGuardData();
    }

    public void OnSceneResumed()
    {
        RestoreGuardData();
    }
}