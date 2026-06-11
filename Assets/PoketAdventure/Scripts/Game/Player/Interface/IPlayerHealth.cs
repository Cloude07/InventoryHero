using System;

namespace CloudeDev.Game.Player
{
    public interface IPlayerHealth
    {
        event Action OnPlayerDied;
        int Health { get; }
        void TakeDamage(int damage);
        void Heal(int amount);
    }
}


