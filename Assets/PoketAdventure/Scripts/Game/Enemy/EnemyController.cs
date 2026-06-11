using UnityEngine;
using CloudeDev.Game.Enemy.State;
using CloudeDev.Game.Enemy.Pool;

namespace CloudeDev.Game.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _PosDamage;
        [SerializeField]
        private float _distance;
        [SerializeField]
        private float _heath;
        [SerializeField]
        private int _damage;
        private AttackType _attackType;
        private EnemyPool _pool;
        private EnemyStateMachine _StateMachine;
        private bool _isDead = false;
        public bool IsDead => _isDead;
        public float Health => _heath;

        public void Damage(float damage)
        {
            if (_heath < damage)
            {
                _StateMachine.SwitchState<DeadEnemyState>();
                _heath = 0;
                _isDead = true;
            }
            else
            {
                _heath -= damage;
                _animator.SetBool("Hit", true);
            }
        }

        public void OnSpawn(SOEnemyData enemyData, EnemyPool pool)
        {
            _heath = enemyData.health;
            _damage = enemyData.damage;
            _attackType = enemyData.attackType;
            _distance = enemyData.distance;
            _pool = pool;
            _isDead = false;

            _StateMachine = new EnemyStateMachine(_animator, _attackType, this._PosDamage, _distance, _damage);
            _StateMachine.SwitchState<IdleEnemyState>();
        }


        private void Update()
        {
            _StateMachine?.Update();
        }

        public void Death()
        {
            _pool.ReturnEnemy(this.gameObject);
        }
    }
}
