using UnityEngine;
using CloudeDev.Game.Enemy.State;
using CloudeDev.Game.Enemy.Pool;

namespace CloudeDev.Game.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _PosDamage;
        [SerializeField] private float _distance;
        [SerializeField] private float _heath;
        [SerializeField] private int _damage;
        [SerializeField] private int _defense;
        [SerializeField] private int _blockChance;
        public int DamageAmount => _damage;         
        public int Defense => _defense;
        public int BlockChance => _blockChance;
        public int HealthInt => (int)_heath;
        public bool IsDead => _isDead;
        public Animator Animator => _animator;

        private AttackType _attackType;
        private EnemyPool _pool;
        private EnemyStateMachine _StateMachine;
        private bool _isDead = false;


        public void Damage(float damage)
        {
            if (_isDead) return;

            _heath -= damage;                        
            if (_heath <= 0)                      
            {
                _heath = 0;
                _isDead = true;
                _StateMachine?.SwitchState<DeadEnemyState>();
            }
            else
            {
                _animator.SetBool("Hit", true);
            }
        }

        public void OnSpawn(SOEnemyData enemyData, EnemyPool pool)
        {
            _heath = enemyData.health;
            _damage = enemyData.damage;
            _attackType = enemyData.attackType;
            _distance = enemyData.distance;
            _defense = enemyData.defense;
            _blockChance = enemyData.blockChance;
            _pool = pool;
            _isDead = false;

            _StateMachine = new EnemyStateMachine(this ,_animator, _attackType, this._PosDamage, _distance, _damage);
            _StateMachine.SwitchState<IdleEnemyState>();
        }


        private void Update()
        {
            _StateMachine?.Update();
        }

        public void Death()
        {
            Debug.Log($"[Enemy] Death called for {name}");
            _pool.ReturnEnemy(gameObject);
        }

        public void PlayAttackAnimation()
        {
            if (_animator == null || _isDead) return;
            _animator.SetTrigger("Attack");   // ← вместо SetBool
        }

        public void StopAttackAnimation() { }   // ← Trigger сам сбрасывается, ничего не делаем
    }
}
