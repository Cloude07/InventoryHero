using UnityEngine;
using System.Collections.Generic;

namespace CloudeDev.Game.Enemy
{
    [CreateAssetMenu(fileName = "SettingLevel", menuName = "Settings/SettingsLevel")]
    public class SOSettingsLevel : ScriptableObject
    {
        public List<SOEnemyData> spawnEntries;
    }

    [CreateAssetMenu(fileName = "EnemyData", menuName = "Settings/EnemySettings")]
    public class SOEnemyData : ScriptableObject
    {
        public GameObject enemyPrefab;
        public AttackType attackType;
        public int damage;
        public int health;
        public float distance;
    }
}
