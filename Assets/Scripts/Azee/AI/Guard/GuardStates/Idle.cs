using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Azee.Interfaces;
using UnityEngine;

namespace GuardStates
{
    public class Idle : StateMachine<Guard>.State
    {
        private static Idle _instance;

        public static Idle Instance
        {
            get { return _instance ?? (_instance = new Idle()); }
        }

        private Idle()
        {
        }


        public void Enter(Guard owner, params object[] args)
        {
        }

        public void Update(Guard owner)
        {
            owner.transform.Rotate(Vector3.up, 2f);


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
        }
    }
}