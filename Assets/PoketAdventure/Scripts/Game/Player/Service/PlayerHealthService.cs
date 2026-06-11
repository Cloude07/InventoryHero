using System;
using UnityEngine;
using VContainer;

namespace CloudeDev.Game.Player.Service
{
    public class PlayerHealthService : IPlayerHealth
    {
        public event Action OnPlayerDied;

        private PlayerStatsModel _playerStats;

        public int Health => _playerStats.Health;

        [Inject]
        public void Construct(PlayerStatsModel playerStats)
        {
            _playerStats = playerStats;
        }

        public void Heal(int amount)
        {
            if(amount + _playerStats.Health  < _playerStats.MaxHealth)
            {
                var currentHeal = _playerStats.Health + amount;
                _playerStats.ChangeStats(currentHeal, StatsPlayer.Health);
            }
            else
            {
                _playerStats.ChangeStats(_playerStats.MaxHealth, StatsPlayer.Health);
            }
        }

        public void TakeDamage(int damage)
        {
              
            if (damage >= _playerStats.Health)
            {
                _playerStats.GetDamage(damage);
                OnPlayerDied.Invoke();
            }
            else if(damage < _playerStats.Health)
            {
                Debug.Log(Health);
                _playerStats.GetDamage(damage);
            }
        }
    }
}
