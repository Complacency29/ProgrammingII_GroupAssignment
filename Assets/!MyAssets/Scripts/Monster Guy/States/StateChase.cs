using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyStuff
{
    public class StateChase : EnemyState
    {
        public StateChase(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
        {

        }

        public override void EnterState()
        {
            Debug.Log("Enter chase state");
        }

        public override void ExitState()
        {
            Debug.Log("Exit chase state");
        }

        public override void FrameUpdate()
        {

        }
    }
}

