using CloudeDev.Boot.SaveSystema;
using CloudeDev.Game.Combat;   // ← для TurnBasedCombatController

namespace CloudeDev.Game.Player.Factories
{
    public interface IPlayerFactory
    {
        PlayerStatsModel CreateFromSave(SaveLoaderManager saveLoader);
        PlayerStatsModel CreateNewGameStats();

        PlayerStateMachine CreateStateMachine(
            IPlayerStats stats,
            TurnBasedCombatController combat);    // ← новый параметр
    }
}