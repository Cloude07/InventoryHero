using CloudeDev.Game.StateMachine;
using UnityEngine;

namespace CloudeDev.Game.Enemy.State
{
    public class DeadEnemyState : IState
    {
        private readonly Animator _animator;
        private readonly EnemyController _controller;
        private float _deathTime;

        public DeadEnemyState(Animator animator, EnemyController controller)
        {
            _animator = animator;
            _controller = controller;
        }

        public void Enter()
        {
            _animator.SetTrigger("Death");
            _deathTime = Time.time + 1.5f;   // под длину клипа смерти
        }

        public void Exit() { }

        public void Update()
        {
            if (Time.time >= _deathTime)
                _controller.Death();
        }
    }
}
