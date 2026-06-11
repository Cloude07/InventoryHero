using CloudeDev.Boot.SaveSystema;

namespace CloudeDev.Game.Player.Factories
{
    public interface IPlayerFactory
    {
        PlayerStatsModel CreateNewGameStats();
        PlayerStatsModel CreateFromSave(SaveLoaderManager saveData);
        PlayerStateMachine CreateStateMachine(IPlayerStats stats);
    }
}
