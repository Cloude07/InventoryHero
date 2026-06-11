using CloudeDev.Game.Player;
using VContainer;
using VContainer.Unity;
using UnityEngine;
using CloudeDev.Game.Enemy.Pool;

public class GameEntryPoint : IInitializable
{
    private readonly PlayerStatsModel _stats;
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly EnemyPool _enemyPool;
    private readonly IPlayerHealth _playerHealth;
    private readonly IPlayerWallet _playerWallet;
    private readonly IPlayerLevel _playerLevel;

    [Inject]
    public GameEntryPoint(
        PlayerStatsModel stats,
        PlayerStateMachine playerStateMachine,
        IPlayerHealth playerHealth,
        IPlayerWallet playerWallet,
        IPlayerLevel playerLevel,
        EnemyPool enemyPool)
    {
        _stats = stats;
        _playerStateMachine = playerStateMachine;
        _playerHealth = playerHealth;
        _playerWallet = playerWallet;
        _playerLevel = playerLevel;
        _enemyPool = enemyPool;

    }

    public void Initialize()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        Debug.Log($"Game started. Player Level: {_stats.Level}, Health: {_stats.Health}");
    }
}

