using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            GameObject playerGameObject = GameManager.Instance.GetPlayerGameObject();

            if (owner.IsObjectInSight(playerGameObject))
            {
                owner.StartChasing(playerGameObject.transform);
            }
        }

        public void Exit(Guard owner)
        {
        }
    }
}