using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuardStates
{
    public class Idle : StateMachine<Guard>.State
    {
        private static Idle _thisState;

        public static Idle State
        {
            get { return _thisState ?? (_thisState = new Idle()); }
        }

        private Idle()
        {
        }


        public void Enter(Guard owner)
        {
        }

        public void Update(Guard owner)
        {
        }

        public void Exit(Guard owner)
        {
        }
    }
}