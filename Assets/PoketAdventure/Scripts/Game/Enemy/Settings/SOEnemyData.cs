using CloudeDev.Game.Inventory;
using UnityEngine;

namespace CloudeDev.Game.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Settings/EnemySettings")]
    public class SOEnemyData : ScriptableObject
    {
        public GameObject enemyPrefab;
        public AttackType attackType;
        public ItemConfig[] itemConfig;
        public int damage;
        public int health;
        public int defense;        
        public int blockChance;    
        public float distance;

       
    }
}
