using System;

namespace CloudeDev.Game.Player
{
    public interface IPlayerStats : IPlayerStatsReadonly, IPlayerStatsModifier
    {
    }

    // Только чтение
    public interface IPlayerStatsReadonly
    {
        int Level { get; }
        int Health { get; }
        int MaxHealth { get; }
        int Armor { get; }
        int MaxArmor { get; }
        int Strength { get; }
        int Dexterity { get; }
        int Coin { get; }
        int Speed { get; }
        event Action<StatsPlayer, int> OnStatsChanged;
    }

    // Только запись
    public interface IPlayerStatsModifier
    {
        void ChangeStats(int value, StatsPlayer statsType);
    }
}