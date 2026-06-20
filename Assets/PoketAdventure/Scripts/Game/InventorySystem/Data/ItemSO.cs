using CloudeDev.Game.Player;
using UnityEngine;

namespace CloudeDev.Game.InventorySystem
{
    [CreateAssetMenu(fileName = "Item", menuName = "ItemConfig/Items")]
    public class ItemSO : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public ItemType type;
        public PotionType potionType;  // только для зелий
        public int maxStack = 1;
        public bool isConsumable;
        [TextArea] public string description;

        // Доп. статы для экипировки
        public int armor;
        public int damage;
        public int strength;
        public int dexterity;
        public int vitality;

        public virtual void Use(IPlayerHealth player)
        {
            // базовая реализация
        }
    }
}
