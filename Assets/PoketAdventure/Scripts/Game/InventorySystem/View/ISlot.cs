namespace CloudeDev.Game.Inventory
{
    public interface ISlot
    {
        bool Hovering { get; }
        void ClearSlot();
        bool IsHasItem();
        public ItemConfig GetItem();
        int RemoveItemAmount(int value, ItemConfig item);
        void SetItem(ItemConfig newItemConfig, int amount = 1);
        int GetItemAmount();
    }
}