using CloudeDev.Game.Enemy.Pool;
using CloudeDev.Game.Player;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CloudeDev.Game
{
    public class LevelController : IDisposable, IStartable
    {
        private EnemyPool _pool;
        private IPlayerView _Player;

        public void Dispose()
        {
            _pool.OnAllEnemiesDefeated -= OnLevelCompleted;
        }

        public void Start()
        {
            _pool.OnAllEnemiesDefeated += OnLevelCompleted;
        }

        [Inject]
        private void Construct(EnemyPool pool, IPlayerView Player)
        {
            _pool = pool;
            _Player = Player;
         
        }

        private void OnLevelCompleted()
        {
            Debug.Log("Уровень пройден!");
            // Показать UI победы, загрузить следующий уровень и т.д.
            _Player.Win();
        }

        private void OnLevelLose()
        {
            Debug.Log("Уровень не пройден!");
            // Показать UI победы, загрузить следующий уровень и т.д.
        }


    }
}
