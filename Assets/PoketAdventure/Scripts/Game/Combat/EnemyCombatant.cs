using System.Collections;
using CloudeDev.Game.Enemy;

namespace CloudeDev.Game.Combat
{
    public class EnemyCombatant : ICombatant
    {
        private readonly EnemyController _enemy;

        public EnemyCombatant(EnemyController enemy) => _enemy = enemy;

        public string Name => _enemy.name;
        public bool IsDead => _enemy.IsDead;
        public int AttackPower => _enemy.DamageAmount;
        public int Defense => _enemy.Defense;
        public int BlockChance => _enemy.BlockChance;

        public void ApplyDamage(int amount)
        {
            if (amount > 0) _enemy.Damage(amount);
        }

        public IEnumerator PlayAttackAnimation()
        {
            _enemy.PlayAttackAnimation();                                         
            yield return AnimatorUtils.WaitForState(_enemy.Animator, "Attack");    
            _enemy.StopAttackAnimation();
        }
    }
}