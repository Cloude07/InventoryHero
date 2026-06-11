using CloudeDev.Game;
using CloudeDev.Game.Enemy.Pool;
using CloudeDev.Game.Player;
using CloudeDev.Game.Player.Factories;
using CloudeDev.Game.Player.Service;
using CloudeDev.Game.Ui;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CloudeDev.DI
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerStatsUI _playerStatsUI;
        [SerializeField] private EnemyPool _enemyPool;

        protected override void Configure(IContainerBuilder builder)
        {
            // Фабрика игрока создает модель
            builder.Register<IPlayerFactory, DefaultPlayerFactory>(Lifetime.Singleton);

            builder.Register<PlayerStatsModel>(resolver =>
            {
                var factory = resolver.Resolve<IPlayerFactory>();
                return factory.CreateNewGameStats();
            }, Lifetime.Singleton).As<IPlayerStats, IPlayerStatsReadonly, PlayerStatsModel>();

            builder.Register<PlayerStateMachine>(resolver =>
            {
                var stats = resolver.Resolve<IPlayerStats>();
                var factory = resolver.Resolve<IPlayerFactory>();
                return factory.CreateStateMachine(stats);   // нужно доработать фабрику
            }, Lifetime.Singleton);

            //GameObjects
            builder.RegisterComponent(_playerController).As<IPlayerView>();

            builder.RegisterComponent(_playerStatsUI);
            builder.RegisterComponent(_enemyPool);

            // Сервисы
            ///Players
            builder.Register<IPlayerHealth, PlayerHealthService>(Lifetime.Singleton);
            builder.Register<IPlayerWallet, PlayerWalletService>(Lifetime.Singleton);
            builder.Register<IPlayerLevel, PlayerLevelService>(Lifetime.Singleton);
            builder.Register<CameraFollow>(Lifetime.Singleton).As<ITickable>();

            builder.Register<LevelController>(Lifetime.Singleton).As<IStartable>();

            // Entry Point
            builder.RegisterEntryPoint<GameEntryPoint>();

        }
    }
}