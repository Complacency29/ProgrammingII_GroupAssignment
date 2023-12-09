using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyStuff
{
    public class StateWander : EnemyState
    {
        public StateWander(Enemy enemy, EnemyFSM enemyFSM) : base(enemy, enemyFSM)
        {
            Vector3 newPosition;

        }

        public override void EnterState()
        {
            Debug.Log("Enter wander state");
        }

        public override void ExitState()
        {
            Debug.Log("Exit wander state");
        }

        public override void FrameUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}

