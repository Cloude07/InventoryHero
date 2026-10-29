using CloudeDev.Game.Enemy.Pool;
using CloudeDev.Game.Player;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CloudeDev.Game
{
    public class LevelController : IDisposable, IStartable, ITickable
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

        private void OnLevelFailed()
        {
            Debug.Log("Уровень не пройден!");
            // Показать UI проиграша, заново или на карту.
            _Player.Lose();
        }

        private void OnLevelCompleted()
        {
            Debug.Log("Уровень пройден!");
            // Показать UI победы, загрузить следующий уровень и т.д.
            _Player.Win();
        }

        public void Tick()
        {
           if(_Player.Health <= 0)
                OnLevelFailed();
        }
    }
}
