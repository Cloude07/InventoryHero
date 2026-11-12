using CloudeDev.Game.Player;
using System.Collections.Generic;
using System.Text;

namespace CloudeDev.Game.Inventory
{
    public static class ItemDescriptionHelper
    {
        public static string GetStatsLine(ItemConfig item)
        {
            if (item == null) return string.Empty;

            switch (item.itemType)
            {
                case ItemType.HealthPoint: return $"Heal: +{item.itemAddValue}";
                case ItemType.Coin: return $"Coin: +{item.itemAddValue}";
            }

            if (item.isEquipingItem && item.modifiers != null && item.modifiers.Length > 0)
            {
                var parts = new List<string>(item.modifiers.Length);
                foreach (var m in item.modifiers)
                {
                    string sign = m.value >= 0 ? "+" : "";
                    parts.Add($"{StatsToShortName(m.stat)}: {sign}{m.value}");
                }
                return string.Join("  ", parts);
            }

            return string.Empty;
        }

        /// <summary>
        /// Разница между новым и текущим предметом. Возвращает строку вида
        /// "Dmg: +5 → +15 (+10)" — если текущего нет — просто "+15".
        /// </summary>
        public static string GetComparisonLine(ItemConfig newItem, ItemConfig equippedItem)
        {
            if (newItem == null || newItem.modifiers == null || newItem.modifiers.Length == 0)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (var newMod in newItem.modifiers)
            {
                int oldValue = 0;
                if (equippedItem != null && equippedItem.modifiers != null)
                {
                    foreach (var oldMod in equippedItem.modifiers)
                    {
                        if (oldMod.stat == newMod.stat)
                        {
                            oldValue = oldMod.value;
                            break;
                        }
                    }
                }

                int delta = newMod.value - oldValue;
                string statName = StatsToShortName(newMod.stat);
                string sign = delta >= 0 ? "+" : "";

                if (equippedItem != null)
                    sb.AppendLine($"{statName}: {oldValue:+0;-0;0} → {newMod.value:+0;-0;0}  ({sign}{delta})");
                else
                    sb.AppendLine($"{statName}: {sign}{newMod.value}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string StatsToShortName(StatsPlayer stat) => stat switch
        {
            StatsPlayer.Health => "Heal",
            StatsPlayer.MaxHealth => "Max HP",
            StatsPlayer.Armor => "Arm",
            StatsPlayer.MaxArmor => "Max Arm",
            StatsPlayer.Strength => "Dmg",
            StatsPlayer.Dexterity => "Dex",
            StatsPlayer.Speed => "Spd",
            StatsPlayer.Luck => "Luck",
            StatsPlayer.Coin => "Coin",
            StatsPlayer.Level => "Lvl",
            _ => stat.ToString()
        };
    }
}