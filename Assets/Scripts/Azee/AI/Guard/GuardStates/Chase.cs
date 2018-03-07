using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuardStates
{
    public class Chase : StateMachine<Guard>.State
    {
        public class StateData
        {
            public Transform TargetTransform;
            public Vector3 LastSeenPosition;
        }


        const float LostDistance = 3f;

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
            stateData.TargetTransform = transform;
            stateData.LastSeenPosition = transform.position;
        }

        public void Update(Guard owner)
        {
            StateData stateData = owner.ChaseStateData;

            bool targetOnSight = false;

            if (owner.IsObjectInSight(stateData.TargetTransform.gameObject))
            {
                targetOnSight = true;
                stateData.LastSeenPosition = stateData.TargetTransform.position;
            }

            if (Vector3.Distance(owner.transform.position, stateData.LastSeenPosition) > LostDistance)
            {
                owner.MoveTowards(stateData.LastSeenPosition);
            }
            else
            {
                if (!targetOnSight)
                {
                    // Lost target
                    StateMachine<Guard> stateMachine = owner.GetStateMachine();
                    stateMachine.SwitchState(stateMachine.GetPreviousState() ?? GuardStates.Idle.Instance);
                }
                else
                {
                    // Too Close to target
                    owner.StopMoving();
                }
            }
        }

        public void Exit(Guard owner)
        {
            StateData stateData = owner.ChaseStateData;
            stateData.TargetTransform = null;
            stateData.LastSeenPosition = Vector3.zero;
        }
    }
}
