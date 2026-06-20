namespace CloudeDev.Game.InventorySystem
{
    public class EquipmentSlot : BaseSlot
    {
        public ItemType slotType; // Head, Body, Weapon и т.д.

        public override bool CanPlace(ItemSO item)
        {
            return item.type == slotType;
        }

        public override void SetItem(ItemInstance newItem)
        {
            base.SetItem(newItem);
            // Обновляем статы персонажа
         //   Player.Instance.RecalculateStats();
        }
    }
}