using CloudeDev.Game.Inventory;
using CloudeDev.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using static UnityEditor.Progress;

namespace CloudeDev.Game.Enemy.Pool
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private SOSettingsLevel settingsPool;
        private Queue<GameObject> pool = new Queue<GameObject>();
        private InventoryManager _inventory;
        private IPlayerView _playerView;

        private int _totalEnemies;      
        private int _enemiesDefeated;   

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

        private GameObject CreateNewEnemy(SOEnemyData enemyCharacter)
        {
            GameObject obj = Instantiate(enemyCharacter.enemyPrefab, transform);
            EnemyController enemyController = obj.GetComponent<EnemyController>();
            enemyController.OnSpawn(enemyCharacter, this);
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
        private void Construct(IPlayerView playerView, InventoryManager inventory)
        {
            _playerView = playerView;
            _inventory = inventory;
        }

        public void DropItem(ItemConfig item)
        {
            if (item == null) return;

            int amount = UnityEngine.Random.Range(item.MinDropAmount, item.MaxDropAmount + 1);
            if (amount <= 0) return;   // 0 = предмет не выпал

            _inventory.AddItem(item, amount);


        }
    }
}
