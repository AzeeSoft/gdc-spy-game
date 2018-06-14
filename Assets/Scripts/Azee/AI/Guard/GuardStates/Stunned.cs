using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Azee.Interfaces;
using UnityEngine;

namespace GuardStates
{
    public class Stunned : StateMachine<Guard>.State
    {
        public class StateData
        {
            public float stunStartTime = 0f;
        }

        private const float stunDuration = 5f;  // In seconds

        private static Stunned _instance;

        public static Stunned Instance
        {
            get { return _instance ?? (_instance = new Stunned()); }
        }

        private Stunned()
        {
        }


        public void Enter(Guard owner, params object[] args)
        {
            owner.StopMoving();
            owner.StunnedStateData.stunStartTime = Time.time;
            owner.GetAudioController().PlayClip(0);
        }

        public void Update(Guard owner)
        {
            if (Time.time - owner.StunnedStateData.stunStartTime >= stunDuration)
            {
                StateMachine<Guard> stateMachine = owner.GetStateMachine();
                stateMachine.SwitchState(stateMachine.GetPreviousState() ?? GuardStates.Idle.Instance);
            }
        }

        public void Exit(Guard owner)
        {
            owner.StunnedStateData.stunStartTime = 0f;
        }
    }
}