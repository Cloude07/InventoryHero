using UnityEngine;
using System.Collections.Generic;

namespace CloudeDev.Game.Enemy
{
    [CreateAssetMenu(fileName = "SettingLevel", menuName = "Settings/SettingsLevel")]
    public class SOSettingsLevel : ScriptableObject
    {
        public List<SOEnemyData> spawnEntries;
    }
}
