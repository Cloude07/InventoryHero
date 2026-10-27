using CloudeDev.Game.Player;

namespace CloudeDev.Game.Inventory
{
    public static class ItemEquipHelper
    {
        // В какой слот можно положить предмет данного типа
        public static EquipedType? ToEquipedType(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.HelmetType => EquipedType.Helm,
                ItemType.SwordType => EquipedType.Sword,
                ItemType.RingType => EquipedType.Ring,
                ItemType.AmuletType => EquipedType.Amulet,
                ItemType.ArmorType => EquipedType.Armor,  
                ItemType.SheeldType => EquipedType.Sheeld,
                ItemType.BootsType => EquipedType.Boots,

                _ => null
            };
        }

        // Какой стат даёт предмет из этого слота
        public static StatsPlayer? ToStatsPlayer(EquipedType equipedType)
        {
            return equipedType switch
            {
                EquipedType.Amulet => StatsPlayer.MaxHealth,
                EquipedType.Helm => StatsPlayer.MaxHealth,
                EquipedType.Ring => StatsPlayer.Dexterity,
                EquipedType.Sheeld => StatsPlayer.Armor,
                EquipedType.Armor => StatsPlayer.MaxHealth,
                EquipedType.Sword => StatsPlayer.Strength,
                EquipedType.Boots => StatsPlayer.Speed,
                _ => null
            };
        }
    }
}