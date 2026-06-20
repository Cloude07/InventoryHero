using System;

namespace CloudeDev.Game.InventorySystem
{
    [Serializable]
    public class ItemInstance
    {
        public ItemSO item;
        public int quantity;
        public int durability; // для экипировки

        public ItemInstance(ItemSO baseItem, int qty = 1)
        {
            item = baseItem;
            quantity = qty;
            durability = 100;
        }
    }
}
