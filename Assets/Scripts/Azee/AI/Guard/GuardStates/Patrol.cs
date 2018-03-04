using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GuardStates
{
    public class Patrol : StateMachine<Guard>.State
    {
        public class StateData
        {
            public bool IsMoving;
            public int TargetWaypointIndex;
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


        public void Enter(Guard owner)
        {
            StateData stateData = owner.PatrolStateData;
            stateData.IsMoving = false;

            int nearestWaypointIndex = owner.DefaultPatrolCircuit.FindNearestWaypointIndex(owner.transform.position);
            stateData.TargetWaypointIndex = owner.DefaultPatrolCircuit.GetNextWaypointIndex(nearestWaypointIndex);
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
                    owner.PatrolTowards(targetPosition);
                    stateData.IsMoving = true;
                }
                else
                {
                    float distanceLeft = Vector3.Distance(owner.transform.position, targetPosition);
                    if (distanceLeft < ArrivalDistance)
                    {
                        stateData.IsMoving = false;
                        stateData.TargetWaypointIndex = owner.DefaultPatrolCircuit.GetNextWaypointIndex(targetWaypointIndex);
                    }
                }
            }
        }

        public void Exit(Guard owner)
        {
            StateData stateData = owner.PatrolStateData;
            stateData.IsMoving = false;
        }
    }
}