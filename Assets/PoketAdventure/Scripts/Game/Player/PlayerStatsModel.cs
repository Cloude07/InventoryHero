using System;
using UnityEngine;

namespace CloudeDev.Game.Player
{
    public class PlayerStatsModel : IPlayerStats
    {
        public event Action<StatsPlayer, int> OnStatsChanged;
        public int Level
        {
            get => _level;
            private set => _level = Mathf.Max(1, value);
        }

        public int Health
        {
            get => _health;
            private set => _health = Mathf.Clamp(value, 0, MaxHealth);
        }

        public int MaxHealth
        {
            get => _maxHealth;
            private set => _maxHealth = Mathf.Max(1, value);
        }

        public int Armor
        {
            get => _armor;
            private set => _armor = Mathf.Clamp(value, 0, MaxArmor);
        }
        public int MaxArmor
        {
            get => _maxArmor;
            private set => _maxArmor = Mathf.Max(1, value);
        }
        public int Strength
        {
            get => _strength;
            private set => _strength = Mathf.Max(0, value);
        }
        public int Dexterity
        {
            get => _dexterity;
            private set => _dexterity = Mathf.Max(0, value);
        }
        public int Coin
        {
            get => _coin;
            private set => _coin = Mathf.Max(0, value);
        }
        public int Speed
        {
            get => _speed;
            private set => _speed = Mathf.Clamp(value, 1, 5);
        }

        private int _level;
        private int _health;
        private int _maxHealth;
        private int _armor;
        private int _maxArmor;
        private int _strength;
        private int _dexterity;
        private int _coin;
        private int _speed;

        public PlayerStatsModel(int level, int health, int maxHealth, int armor, int maxArmor,
                    int strength, int dexterity, int coin, int speed)
        {
            _level = level;
            _health = health;
            _maxHealth = maxHealth;
            _armor = armor;
            _maxArmor = maxArmor;
            _strength = strength;
            _dexterity = dexterity;
            _coin = coin;
            _speed = speed;
        }

        public void ChangeStats(int value, StatsPlayer statsType)
        {
            switch (statsType)
            {
                case StatsPlayer.Level: Level = value; break;
                case StatsPlayer.Health: Health = value; break;
                case StatsPlayer.MaxHealth: MaxHealth = value; break;
                case StatsPlayer.Armor: Armor = value; break;
                case StatsPlayer.MaxArmor: MaxArmor = value; break;
                case StatsPlayer.Strength: Strength = value; break;
                case StatsPlayer.Dexterity: Dexterity = value; break;
                case StatsPlayer.Coin: Coin = value; break;
                case StatsPlayer.Speed: Speed = value; break;
            }

            OnStatsChanged?.Invoke(statsType, GetStatValue(statsType));
        }

        public void GetDamage(int value)
        {
            Health -= value;
        }

        private int GetStatValue(StatsPlayer statsType) => statsType switch
        {
            StatsPlayer.Level => Level,
            StatsPlayer.Health => Health,
            StatsPlayer.MaxHealth => MaxHealth,
            StatsPlayer.Armor => Armor,
            StatsPlayer.MaxArmor => MaxArmor,
            StatsPlayer.Strength => Strength,
            StatsPlayer.Dexterity => Dexterity,
            StatsPlayer.Coin => Coin,
            StatsPlayer.Speed => Speed,
            _ => 0
        };
    }
}
