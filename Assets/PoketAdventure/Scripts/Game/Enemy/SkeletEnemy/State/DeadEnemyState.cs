using CloudeDev.Game.StateMachine;
using System;
using UnityEngine;

namespace CloudeDev.Game.Enemy.State
{
    public class DeadEnemyState : IState
    {
        private Animator _animator;

        public DeadEnemyState(Animator animator)
        {
            _animator = animator;
        }


        public void Enter()
        {
            _animator.SetTrigger("Death");
        }

        public void Exit()
        {
           
        }

        public void Update()
        {
            
        }
    }
}
