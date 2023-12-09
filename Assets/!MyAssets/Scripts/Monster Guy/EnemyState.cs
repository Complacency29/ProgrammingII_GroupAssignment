using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyStuff
{
    public abstract class EnemyState
    {
        protected Enemy enemy;
        protected EnemyFSM enemyFSM;

        public EnemyState(Enemy enemy, EnemyFSM enemyFSM)
        {
            this.enemy = enemy;
            this.enemyFSM = enemyFSM;
        }

        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void FrameUpdate();
        //public virtual void PhysicsUpdate() { }
        //public virtual void AnimationTriggerEvent() { }
    }
}

