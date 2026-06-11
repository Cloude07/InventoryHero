using System;
using VContainer;

namespace CloudeDev.Game.Player.Service
{
    public class PlayerLevelService : IPlayerLevel
    {
        public event Action<int> OnLevelUp;

        private PlayerStatsModel _playerStats;

        [Inject]
        private void Construct(PlayerStatsModel playerStatsModel)
        {
            _playerStats = playerStatsModel;
        }

        public void LevelUp(int level)
        {
            if (level == 0 || level == _playerStats.Level) return;
            _playerStats.ChangeStats(level, StatsPlayer.Level);
            OnLevelUp?.Invoke(level);
        }
    }
}
