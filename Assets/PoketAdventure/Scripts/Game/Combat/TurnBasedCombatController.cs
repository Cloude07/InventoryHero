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
        private const int MaxRounds = 200;

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
            int round = 0;
            while (_combatActive && !_player.IsDead && !_enemy.IsDead)
            {
                if (++round > MaxRounds)
                {
                    OnCombatLog?.Invoke("Combat timeout — stalemate!");
                    break;
                }
                while (_combatActive && !_player.IsDead && !_enemy.IsDead)
                {
                    yield return Attack(_player, _enemy);
                    if (_enemy.IsDead) break;
                    yield return new WaitForSeconds(_turnDelay);

                    yield return Attack(_enemy, _player);
                    if (_player.IsDead) break;
                    yield return new WaitForSeconds(_turnDelay);
                }
            }
            _combatActive = false;
            OnCombatEnded?.Invoke();
        }

        private IEnumerator Attack(ICombatant attacker, ICombatant defender)
        {
            // 1. Бросок блока — ДО анимаций
            bool blocked = UnityEngine.Random.Range(0, 100) < defender.BlockChance;

            if (blocked)
                OnCombatLog?.Invoke($"{defender.Name} BLOCKS {attacker.Name}!");

            // 2. Анимации атаки + блока параллельно
            Coroutine attackRoutine = StartCoroutine(attacker.PlayAttackAnimation());
            Coroutine blockRoutine = blocked ? StartCoroutine(defender.PlayBlockAnimation()) : null;

            yield return attackRoutine;
            if (blockRoutine != null) yield return blockRoutine;

            // 3. Заблокировано — урона нет
            if (blocked) yield break;

            // 4. Урон
            int dmg = Mathf.Max(1, attacker.AttackPower - defender.Defense);
            defender.ApplyDamage(dmg);
            OnCombatLog?.Invoke($"{attacker.Name} hits {defender.Name} for {dmg}");

            // 🔴 если защищающийся умер — Hit не играем (Die уже проигран)
            if (defender.IsDead) yield break;

            yield return defender.PlayHitAnimation();
        }

    }
}