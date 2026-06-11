using CloudeDev.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace CloudeDev.Game.Enemy.Pool
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private SOSettingsLevel settingsPool;
        private Queue<GameObject> pool = new Queue<GameObject>();
        private IPlayerView _playerView;

        private int _totalEnemies;      // всего врагов в уровне
        private int _enemiesDefeated;   // сколько убито

        // Событие, на которое подпишется менеджер уровня
        public event Action OnAllEnemiesDefeated;

        private void Start()
        {
            foreach (SOEnemyData enemy in settingsPool.spawnEntries)
            {
                GameObject obj = CreateNewEnemy(enemy);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            _totalEnemies = pool.Count;
            _enemiesDefeated = 0;
        }
        public void SpawnNextEnemy()
        {
            if (pool.Count == 0) return;

            GameObject enemyObj = pool.Dequeue();
            Vector3 pos = new Vector3(
                _playerView.PlayerTransform.position.x + 20,
                _playerView.PlayerTransform.position.y,
                0
            );
            enemyObj.transform.position = pos;
            enemyObj.SetActive(true);
        }

        private GameObject CreateNewEnemy(SOEnemyData character)
        {
            GameObject obj = Instantiate(character.enemyPrefab, transform);
            EnemyController enemy = obj.GetComponent<EnemyController>();
            enemy.OnSpawn(character, this);
            return obj;
        }

        public void ReturnEnemy(GameObject enemyObj)
        {
            enemyObj.SetActive(false);
            _enemiesDefeated++;

            if (_enemiesDefeated >= _totalEnemies)
            {
                // Все враги убиты → завершаем уровень
                OnAllEnemiesDefeated?.Invoke();
            }
            else
            {
                // Возвращаем убитого врага в пул для повторного использования
                pool.Enqueue(enemyObj);
                // Если нужно, чтобы следующий враг появлялся сразу после смерти предыдущего:
                SpawnNextEnemy();
            }
        }

        [Inject]
        private void Construct(IPlayerView playerView)
        {
            _playerView = playerView;
        }
    }
}
