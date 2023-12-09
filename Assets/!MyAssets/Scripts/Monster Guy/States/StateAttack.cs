using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyStuff
{
    public class StateAttack : EnemyState
    {
        public StateAttack(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
        {

        }

        public override void EnterState()
        {
            Debug.Log("Enter attack state");
        }

        public override void ExitState()
        {
            Debug.Log("Exit attack state");
        }

        public override void FrameUpdate()
        {

        }
    }
}

