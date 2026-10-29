using System.Collections;

namespace CloudeDev.Game.Combat
{
    public interface ICombatant
    {
        string Name { get; }
        bool IsDead { get; }
        int AttackPower { get; }
        int Defense { get; }
        int BlockChance { get; }
        void ApplyDamage(int amount);

        IEnumerator PlayAttackAnimation();
        IEnumerator PlayBlockAnimation();
        IEnumerator PlayHitAnimation();
    }
}
