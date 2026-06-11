using System;
using VContainer;

namespace CloudeDev.Game.Player.Service
{
    public class PlayerWalletService : IPlayerWallet
    {
        public event Action<int> _OnCoinChanged;
        private PlayerStatsModel _playerStats;

        [Inject]
        private void Construct(PlayerStatsModel playerStats)
        {
            _playerStats = playerStats;
        }

        public void AddCoin(int value)
        {
            if (value <= 0) return;
            _playerStats.ChangeStats(_playerStats.Coin + value, StatsPlayer.Coin);
            _OnCoinChanged?.Invoke(_playerStats.Coin);
        }

        public bool RemoveCoin(int value)
        {
            if (value <= 0 || value > _playerStats.Coin) return false;
            _playerStats.ChangeStats(_playerStats.Coin - value, StatsPlayer.Coin);
            _OnCoinChanged?.Invoke(_playerStats.Coin);
            return true;
        }
    }
}
