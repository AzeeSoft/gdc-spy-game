﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Azee.Interfaces;
using Azee;
using UnityEngine;
using UnityEngine.AI;

namespace GuardStates
{
    public class Chase : StateMachine<Guard>.State
    {
        [Serializable]
        public class StateData
        {
            public Transform TargetTransform;
            public Vector3 LastKnownPosition;
            public float PrevAgentSpeed;
            public float PrevAgentStoppingDistance;
        }

        private static Chase _instance;

        public static Chase Instance
        {
            get { return _instance ?? (_instance = new Chase()); }
        }

        private Chase()
        {
        }

        public void Enter(Guard owner, params object[] args)
        {
            Transform transform = (Transform) args[0];

            StateData stateData = owner.ChaseStateData;
            NavMeshAgent navMeshAgent = owner.GetNavMeshAgent();

            stateData.TargetTransform = transform;
            stateData.LastKnownPosition = transform.position;
            stateData.PrevAgentSpeed = navMeshAgent.speed;
            stateData.PrevAgentStoppingDistance = navMeshAgent.stoppingDistance;

            navMeshAgent.speed = owner.ChaseSpeed;
            navMeshAgent.stoppingDistance = owner.LostDistance;

            owner.GetColorChanger().turnRed();
            owner.GetAudioController().PlayClip(1);
        }

        public void Update(Guard owner)
        {
            StateData stateData = owner.ChaseStateData;

            bool targetOnSight = false, targetAudible = false;

            if (owner.IsObjectInSight(stateData.TargetTransform.gameObject))
            {
                targetOnSight = true;
                stateData.LastKnownPosition = stateData.TargetTransform.position;
            }
            else
            {
                AudibleObject targetAudibleObject = stateData.TargetTransform.GetComponent<AudibleObject>();
                if (targetAudibleObject != null)
                {
                    Vector3? targetLocation = owner.LocateObjectFromNoise(targetAudibleObject);

                    if (targetLocation != null)
                    {
                        targetAudible = true;
                        stateData.LastKnownPosition = targetLocation.Value;
                    }
                }
            }

            if (targetOnSight)
            {
                Player player = stateData.TargetTransform.GetComponent<Player>();
                if (player != null && !player.IsInfected)
                {
                    float distance = Vector3.Distance(owner.transform.position, stateData.TargetTransform.position);

                    if (distance <= owner.MaxInfectionRadius)
                    {
                        float infectionValue =
                            StaticTools.Remap(distance, 0, owner.MaxInfectionRadius, owner.MaxInfection, 0);
                        player.Infect(infectionValue);
                    }
                }
            }

            if (Vector3.Distance(owner.transform.position, stateData.LastKnownPosition) > owner.LostDistance)
            {
                owner.MoveTowards(stateData.LastKnownPosition);

                /*if (!owner.MoveTowards(stateData.LastKnownPosition))
                {
                    StateMachine<Guard> stateMachine = owner.GetStateMachine();
                    stateMachine.SwitchState(stateMachine.GetPreviousState() ?? GuardStates.Idle.Instance);
                }*/
            }
            else
            {
                if (!targetOnSight && !targetAudible)
                {
                    // Lost target
                    StateMachine<Guard> stateMachine = owner.GetStateMachine();
                    stateMachine.SwitchState(stateMachine.GetPreviousState() ?? GuardStates.Idle.Instance);
                }
                else
                {
                    // Too Close to target
                    owner.StopMoving();

                    Vector3 lookAtPos = stateData.TargetTransform.position;
                    lookAtPos.y = owner.transform.position.y;

                    owner.transform.LookAt(lookAtPos);

                    //TODO: Switch state or do something
                }
            }
        }

        public void Exit(Guard owner)
        {
            StateData stateData = owner.ChaseStateData;

            owner.GetNavMeshAgent().speed = stateData.PrevAgentSpeed;
            owner.GetNavMeshAgent().stoppingDistance = stateData.PrevAgentStoppingDistance;

            stateData.TargetTransform = null;
            stateData.LastKnownPosition = Vector3.zero;
            stateData.PrevAgentSpeed = 0;
            stateData.PrevAgentStoppingDistance = 0;

            owner.GetColorChanger().turnDefault();
        }
    }
}
