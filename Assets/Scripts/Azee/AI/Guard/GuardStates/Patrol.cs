﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Azee.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace GuardStates
{
    public class Patrol : StateMachine<Guard>.State
    {
        public class StateData
        {
            public bool IsMoving;
            public int TargetWaypointIndex;
            public float PrevAgentSpeed;
            public float PrevAgentStoppingDistance;
        }

        const float ArrivalDistance = 3f;

        private static Patrol _instance;

        public static Patrol Instance
        {
            get { return _instance ?? (_instance = new Patrol()); }
        }

        private Patrol()
        {
        }


        public void Enter(Guard owner, params object[] args)
        {
            StateData stateData = owner.PatrolStateData;
            NavMeshAgent navMeshAgent = owner.GetNavMeshAgent();

            stateData.IsMoving = false;

            int nearestWaypointIndex = owner.DefaultPatrolCircuit.FindNearestWaypointIndex(owner.transform.position);
            stateData.TargetWaypointIndex = owner.DefaultPatrolCircuit.GetNextWaypointIndex(nearestWaypointIndex);

            stateData.PrevAgentSpeed = navMeshAgent.speed;
            stateData.PrevAgentStoppingDistance = navMeshAgent.stoppingDistance;

            navMeshAgent.speed = owner.PatrolSpeed;
            navMeshAgent.stoppingDistance = 0;

            /* 
            int nextWaypointIndex = owner.DefaultPatrolCircuit.GetNextWaypointIndex(nearestWaypointIndex);

            float nearestDistance = Vector3.Distance(owner.transform.position,
                owner.DefaultPatrolCircuit.GetWaypoint(nearestWaypointIndex).position);
            float nextDistance = Vector3.Distance(owner.transform.position,
                owner.DefaultPatrolCircuit.GetWaypoint(nextWaypointIndex).position);
            float nearestToNextDistance = Vector3.Distance(owner.DefaultPatrolCircuit.GetWaypoint(nearestWaypointIndex).position,
                owner.DefaultPatrolCircuit.GetWaypoint(nextWaypointIndex).position);

            if (nearestDistance + nearestToNextDistance < nextDistance)
            {
                stateData.TargetWaypointIndex = nearestWaypointIndex;
            }
            else
            {
                stateData.TargetWaypointIndex = nextWaypointIndex;
            }
            */
        }

        public void Update(Guard owner)
        {
            StateData stateData = owner.PatrolStateData;

            int targetWaypointIndex = stateData.TargetWaypointIndex;
            if (targetWaypointIndex != -1)
            {
                Vector3 targetPosition = owner.DefaultPatrolCircuit.GetWaypoint(targetWaypointIndex).position;
                if (!stateData.IsMoving)
                {
                    owner.MoveTowards(targetPosition);
                    stateData.IsMoving = true;
                }
                else
                {
                    float distanceLeft = Vector3.Distance(owner.transform.position, targetPosition);
                    if (distanceLeft < ArrivalDistance)
                    {
                        stateData.IsMoving = false;
                        stateData.TargetWaypointIndex =
                            owner.DefaultPatrolCircuit.GetNextWaypointIndex(targetWaypointIndex);
                    }
                }
            }


            /*
             * Checking if the guard can see player, and switching to chasing state, if so.
             */
            GameObject playerGameObject = LevelManager.Instance.GetPlayerGameObject();

            if (owner.IsObjectInSight(playerGameObject))
            {
                owner.StartChasing(playerGameObject.transform);
            }
            else
            {
                AudibleObject playerAudibleObject = playerGameObject.GetComponent<AudibleObject>();
                Vector3? playerLocation = owner.LocateObjectFromNoise(playerAudibleObject);

                if (playerLocation != null)
                {
                    owner.StartChasing(playerGameObject.transform);
                    owner.ChaseStateData.LastKnownPosition = playerLocation.Value;  // Just in case, if we add approximation to noise location
                }
            }
        }

        public void Exit(Guard owner)
        {
            StateData stateData = owner.PatrolStateData;

            owner.GetNavMeshAgent().speed = stateData.PrevAgentSpeed;
            owner.GetNavMeshAgent().stoppingDistance = stateData.PrevAgentStoppingDistance;

            stateData.IsMoving = false;
            stateData.TargetWaypointIndex = -1;
            stateData.PrevAgentSpeed = 0;
            stateData.PrevAgentStoppingDistance = 0;
        }
    }
}