using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyStuff
{
    public class EnemyFSM : MonoBehaviour
    {
        public EnemyState CurEnemyState { get; set; }

        public void Initialize(EnemyState startingState)
        {
            CurEnemyState = startingState;
            CurEnemyState.EnterState();
        }

        public void ChangeState(EnemyState newState)
        {
            CurEnemyState.ExitState();
            CurEnemyState = newState;
            CurEnemyState.EnterState();
        }
    }
}

