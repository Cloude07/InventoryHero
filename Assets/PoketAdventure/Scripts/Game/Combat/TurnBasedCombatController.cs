using System;
using System.Collections;
using UnityEngine;
using VContainer;

namespace CloudeDev.Game.Combat
{
    public class TurnBasedCombatController : MonoBehaviour
    {
        [SerializeField] private float _attackAnimTime = 0.6f;   // ← сколько ждём анимацию
        [SerializeField] private float _hitPause = 0.2f;   // ← пауза после
        [SerializeField] private float _turnDelay = 0.5f;   // ← между ходами

        public event Action<string> OnCombatLog;   // для UI
        public event Action OnCombatEnded;

        private ICombatant _player;
        private ICombatant _enemy;
        private bool _combatActive;

        [Inject]
        private void Construct(PlayerCombatant playerCombatant)
        {
            _player = playerCombatant;
        }

        public bool IsActive => _combatActive;

        public void StartCombat(ICombatant enemy)
        {
            if (_combatActive) return;
            _enemy = enemy;
            _combatActive = true;
            StartCoroutine(CombatLoop());
        }

        private IEnumerator CombatLoop()
        {
            while (_combatActive && !_player.IsDead && !_enemy.IsDead)
            {
                yield return Attack(_player, _enemy);
                if (_enemy.IsDead) break;
                yield return new WaitForSeconds(_turnDelay);

                yield return Attack(_enemy, _player);
                if (_player.IsDead) break;
                yield return new WaitForSeconds(_turnDelay);
            }

            _combatActive = false;
            OnCombatEnded?.Invoke();
        }

        private IEnumerator Attack(ICombatant attacker, ICombatant defender)
        {
            // 1. Анимация — ждём реального завершения
            yield return attacker.PlayAttackAnimation();

            // 2. Блок
            if (UnityEngine.Random.Range(0, 100) < defender.BlockChance)
            {
                OnCombatLog?.Invoke($"{defender.Name} BLOCKS {attacker.Name}!");
                yield break;
            }

            // 3. Урон
            int dmg = Mathf.Max(1, attacker.AttackPower - defender.Defense);
            defender.ApplyDamage(dmg);
            OnCombatLog?.Invoke($"{attacker.Name} hits {defender.Name} for {dmg}");
        }
    }
}