using CloudeDev.Game.Player;
using System.Collections;
using VContainer;

namespace CloudeDev.Game.Combat
{
    public class PlayerCombatant : ICombatant
    {
        private readonly IPlayerStatsReadonly _stats;
        private readonly IPlayerHealth _health;
        private readonly IObjectResolver _resolver;

        public PlayerCombatant(IPlayerStatsReadonly stats, IPlayerHealth health, IObjectResolver resolver)
        {
            _stats = stats;
            _health = health;
            _resolver = resolver;
        }

        public string Name => "Player";
        public bool IsDead => _health.Health <= 0;
        public int AttackPower => _stats.Strength;
        public int Defense => _stats.Armor;
        public int BlockChance => _stats.Luck;

        public void ApplyDamage(int amount)
        {
            if (amount > 0) _health.TakeDamage(amount);
        }

        public IEnumerator PlayAttackAnimation()
        {
            var view = _resolver.Resolve<IPlayerView>();
            var animator = view.Animator;

            view.PlayAnimation("Fight");                                  
            yield return AnimatorUtils.WaitForState(animator, "Fight");      
            view.PlayAnimation("Idle");                                    
        }
    }
}