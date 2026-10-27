using System;
using UnityEngine;

namespace CloudeDev.Game.Player
{
    public class PlayerStats : MonoBehaviour, IPlayerStats
    {
        [Header("Base Stats")]
        [SerializeField] private int level = 1;
        [SerializeField] private int health = 100;
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int armor = 0;
        [SerializeField] private int maxArmor = 100;
        [SerializeField] private int strength = 5;
        [SerializeField] private int dexterity = 5;
        [SerializeField] private int coin = 0;
        [SerializeField] private int speed = 5;

        public event Action<StatsPlayer, int> OnStatsChanged;

        public int Level => level;
        public int Health => health;
        public int MaxHealth => maxHealth;
        public int Armor => armor;
        public int MaxArmor => maxArmor;
        public int Strength => strength;
        public int Dexterity => dexterity;
        public int Coin => coin;
        public int Speed => speed;

        public void ChangeStats(int value, StatsPlayer statsType)
        {
            Debug.Log($"[PlayerStats] ChangeStats({value}, {statsType}) on '{name}'");
            switch (statsType)
            {
                case StatsPlayer.Level:
                    level = Mathf.Max(1, level + value);
                    break;
                case StatsPlayer.Health:
                    health = Mathf.Clamp(health + value, 0, maxHealth);
                    break;
                case StatsPlayer.MaxHealth:
                    maxHealth = Mathf.Max(1, maxHealth + value);
                    health = Mathf.Min(health, maxHealth);
                    break;
                case StatsPlayer.Armor:
                    armor = Mathf.Max(0, armor + value);
                    break;
                case StatsPlayer.MaxArmor:
                    maxArmor = Mathf.Max(0, maxArmor + value);
                    armor = Mathf.Min(armor, maxArmor);
                    break;
                case StatsPlayer.Strength:
                    strength = Mathf.Max(0, strength + value);
                    break;
                case StatsPlayer.Dexterity:
                    dexterity = Mathf.Max(0, dexterity + value);
                    break;
                case StatsPlayer.Coin:
                    coin = Mathf.Max(0, coin + value);
                    break;
                case StatsPlayer.Speed:
                    speed = Mathf.Max(0, speed + value);
                    break;
            }

            OnStatsChanged?.Invoke(statsType, value);
        }
    }
}