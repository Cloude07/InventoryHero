using CloudeDev.Boot.SaveSystema;
using CloudeDev.Game.Combat;

namespace CloudeDev.Game.Player.Factories
{
    public class DefaultPlayerFactory : IPlayerFactory
    {
        public PlayerStatsModel CreateFromSave(SaveLoaderManager saveLoader)
        {
            // TODO: заменить на реальную загрузку из saveLoader.LoadData()
            return new PlayerStatsModel(
                level: 1,
                health: 25,
                maxHealth: 25,
                armor: 5,
                maxArmor: 5,
                strength: 2,
                dexterity: 3,
                coin: 0,
                speed: 3,
                luck: 10                                   // ← добавить
            );
        }

        public PlayerStatsModel CreateNewGameStats()
        {
            return new PlayerStatsModel(
                level: 1,
                health: 25,
                maxHealth: 25,
                armor: 5,
                maxArmor: 5,
                strength: 2,
                dexterity: 3,
                coin: 0,
                speed: 4,
                luck: 10                                   // ← добавить
            );
        }

        public PlayerStateMachine CreateStateMachine(
            IPlayerStats stats,
            TurnBasedCombatController combat)              // ← новый параметр
        {
            return new PlayerStateMachine(stats, combat);
        }
    }
}