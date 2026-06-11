using CloudeDev.Game.Player;
using CloudeDev.Game.StateMachine;
using UnityEngine;

namespace CloudeDev.Game.Enemy.State
{
    public class AttackEnemyState : IState
    {
        private Animator _animator;
        private AttackType _attackType;
        private float _distance;
        private int _damage;
        private float _attackInterval = 1.5f;   // интервал между ударами
        private float _nextAttackTime;
        private Transform _transform;
        private PlayerController _currentPlayer;

        public AttackEnemyState(Animator animator, AttackType attackType,
            Transform enemyTransform, float distance, int damage)
        {
            _animator = animator;
            _attackType = attackType;
            _transform = enemyTransform;
            _damage = damage;
            _distance = distance;

        }


        public void Enter()
        {
            RaycastHit2D hit = Physics2D.Raycast(_transform.position, Vector2.left, _distance);

            if (hit.collider?.CompareTag("Player") == true)
                _currentPlayer = hit.collider.GetComponent<PlayerController>();
            Debug.Log("Raycast " + hit.collider);
            _nextAttackTime = Time.time + _attackInterval;
        }

        public void Exit()
        {
            _animator.SetBool("Attack", false);

        }

        public void Update()
        {

            if (_currentPlayer.Health <= 0)
            {
                _animator.SetBool("Attack", false);
                return;

            }

            if (Time.time >= _nextAttackTime)
            {
                _currentPlayer?.PlayerHealth.TakeDamage(_damage);
                _nextAttackTime = Time.time + _attackInterval;
                _animator.SetBool("Attack", true);
            }
        }
    }
}
