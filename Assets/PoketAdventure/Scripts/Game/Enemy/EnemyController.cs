using UnityEngine;
using CloudeDev.Game.Enemy.State;
using CloudeDev.Game.Enemy.Pool;
using UnityEngine.UI;
using System;

namespace CloudeDev.Game.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _PosDamage;
        [SerializeField] private SpriteRenderer _sprite;
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

        [SerializeField] private EnemyHealthBar _healthBar;   // ← новое

        private float _maxHeath;


        public void Damage(float damage)
        {
            if (_isDead) return;

            _heath -= damage;

            if (_healthBar != null)
                _healthBar.SetHealth(_heath, _maxHeath);

            if (_heath <= 0)
            {
                _heath = 0;
                _isDead = true;
                if (_healthBar != null) _healthBar.Hide();
                _StateMachine?.SwitchState<DeadEnemyState>();
            }
        }

        public void OnSpawn(SOEnemyData enemyData, EnemyPool pool)
        {
            _damage = enemyData.damage;
            _attackType = enemyData.attackType;
            _distance = enemyData.distance;
            _defense = enemyData.defense;
            _blockChance = enemyData.blockChance;
            _heath = enemyData.health;
            _pool = pool;
            _isDead = false;
            _maxHeath = enemyData.health;


            if (_healthBar != null)
                _healthBar.ResetSilent(_heath, _maxHeath);


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

        public void PlayBlockAnimation()
        {
            if (_animator == null || _isDead) return;
            _animator.SetTrigger("Block");   // ← вместо SetBool
        }


        public void PlayHitAnimation()
        {
            if (_animator == null || _isDead) return;
            _animator.SetTrigger("Hit");

            if (_sprite != null)
                StartCoroutine(FlashRed());
        }
        private System.Collections.IEnumerator FlashRed()
        {
            var original = _sprite.color;
            _sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _sprite.color = original;
        }

        public void StopHitAnimation()
        {
            
        }

        internal void StopBlockAnimation()
        {
            
        }
    }
}
