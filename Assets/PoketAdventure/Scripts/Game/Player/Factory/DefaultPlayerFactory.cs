using CloudeDev.Boot.SaveSystema;

namespace CloudeDev.Game.Player.Factories
{
    public class DefaultPlayerFactory : IPlayerFactory
    {

        public PlayerStatsModel CreateFromSave(SaveLoaderManager LoadData)
        {
            //Load => PlayerStatsModel(Load.jsonData)
                return new PlayerStatsModel(
                level: 1,
                health: 25,
                maxHealth: 25,
                armor: 5,
                maxArmor: 5,
                strength: 2,
                dexterity: 3,
                coin: 0,
                speed: 3
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
                speed: 4
           );
        }

        public PlayerStateMachine CreateStateMachine(IPlayerStats stats)
        {
            return new PlayerStateMachine(stats);
        }
    }

}
