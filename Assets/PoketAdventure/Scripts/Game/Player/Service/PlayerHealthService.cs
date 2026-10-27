using System;
using VContainer;

namespace CloudeDev.Game.Player.Service
{
    public class PlayerHealthService : IPlayerHealth
    {
        public event Action OnPlayerDied;

        private PlayerStatsModel _playerStats;

        public int Health => _playerStats.Health;
        private bool _isDead;

        [Inject]
        public void Construct(PlayerStatsModel playerStats)
        {
            _playerStats = playerStats;
        }

        public void Heal(int amount)
        {
            if (_isDead) return;
            if (amount <= 0) return;
            _playerStats.ChangeStats(amount, StatsPlayer.Health);
        }

        public void TakeDamage(int damage)
        {
            if (_isDead || damage <= 0) return;

            _playerStats.ChangeStats(-damage, StatsPlayer.Health);   

            if (_playerStats.Health <= 0)
            {
                _isDead = true;
                OnPlayerDied?.Invoke();
            }
        }
    }
}

