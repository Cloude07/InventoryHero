using System;

namespace CloudeDev.Game.Player
{
    public interface IPlayerAbilities
    {
        event Action<int, StatsPlayer> OnChangeAbility;
        void UpAbility(int value, StatsPlayer abilityType);
        bool TryUpgradeAbility(int cost, int newValue, StatsPlayer abilityType);
    }

}


