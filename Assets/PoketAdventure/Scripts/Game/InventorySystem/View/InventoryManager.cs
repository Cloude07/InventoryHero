using System.Collections.Generic;
using UnityEngine;

namespace CloudeDev.Game.InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        public InventorySlot[] inventorySlots; // например, 40 слотов
        public EquipmentSlot headSlot, bodySlot, bootsSlot, weaponSlot, sheeldSlot, ringSlot, amuletSlot;

        private List<ItemInstance> inventory = new List<ItemInstance>();

        void Awake() => Instance = this;

        public bool AddItem(ItemSO item, int qty = 1)
        {
            // Стакуемые предметы
            if (item.maxStack > 1)
            {
                foreach (var slot in inventorySlots)
                {
                    if (slot.currentItem?.item == item && slot.currentItem.quantity < item.maxStack)
                    {
                        int canAdd = item.maxStack - slot.currentItem.quantity;
                        int toAdd = Mathf.Min(canAdd, qty);
                        slot.currentItem.quantity += toAdd;
                        qty -= toAdd;
                        slot.SetItem(slot.currentItem);
                        if (qty <= 0) return true;
                    }
                }
            }
            // Новый предмет в пустой слот
            foreach (var slot in inventorySlots)
            {
                if (slot.currentItem == null)
                {
                    slot.SetItem(new ItemInstance(item, qty));
                    return true;
                }
            }
            Debug.Log("Инвентарь полон!");
            return false;
        }

        public void Equip(InventorySlot fromSlot, EquipmentSlot toSlot)
        {
            if (!toSlot.CanPlace(fromSlot.currentItem.item)) return;
            // Swap
            ItemInstance temp = toSlot.currentItem;
            toSlot.SetItem(fromSlot.currentItem);
            fromSlot.SetItem(temp);
            SaveInventory();
        }


        public void SaveInventory()
        {
            // Сохраняем в PlayerPrefs/JSON
        }

        // Контекстное меню
        public void ShowContextMenu(BaseSlot slot)
        {
            // Логика: если слот инвентаря с экипировкой -> "Экипировать"
            // Если пояс -> "Использовать"
        }
    }
}