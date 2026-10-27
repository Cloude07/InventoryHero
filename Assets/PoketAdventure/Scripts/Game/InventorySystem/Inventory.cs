using System.Collections.Generic;
using CloudeDev.Game.Player;
using UnityEngine;
using UnityEngine.UI;

namespace CloudeDev.Game.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [Header("Test")]
        public ItemConfig ItemConfig1;
        public ItemConfig ItemConfig2;
        public ItemConfig ItemConfig3;
        public ItemConfig ItemConfig4;
        public ItemConfig ItemConfig5;
        public ItemConfig ItemConfig6;
        public ItemConfig ItemConfig7;
        public ItemConfig ItemConfig8;
        public ItemConfig ItemConfig9;

        [Header("References")]
        public GameObject inventoryObject;
        public GameObject inventoryEquipedObject;
        public Image dragItemIcon;
        [SerializeField] private PlayerStats playerStats; // ← назначить

        private ISlot draggetSlot = null;
        private bool IsDraging = false;

        private List<ISlot> inventorySlots = new List<ISlot>();
        private List<ISlot> inventoryEquipedSlots = new List<ISlot>();
        private List<ISlot> allSlots = new List<ISlot>();
        private Color colorDragItem = new Color(1, 1, 1, 0.5f);

        private void Awake()
        {
            inventorySlots.AddRange(inventoryObject.GetComponentsInChildren<Slot>());
            inventoryEquipedSlots.AddRange(inventoryEquipedObject.GetComponentsInChildren<EquipedSlot>());

            allSlots.AddRange(inventorySlots);
            allSlots.AddRange(inventoryEquipedSlots);

            Debug.Log($"[Inventory] inventory={inventorySlots.Count}, equipped={inventoryEquipedSlots.Count}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) AddItem(ItemConfig1, 1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) AddItem(ItemConfig2, 1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) AddItem(ItemConfig3, 1);
            if (Input.GetKeyDown(KeyCode.Alpha4)) AddItem(ItemConfig4, 1);
            if (Input.GetKeyDown(KeyCode.Alpha5)) AddItem(ItemConfig5, 1);
            if (Input.GetKeyDown(KeyCode.Alpha6)) AddItem(ItemConfig6, 1);
            if (Input.GetKeyDown(KeyCode.Alpha7)) AddItem(ItemConfig7, 1);
            if (Input.GetKeyDown(KeyCode.Alpha8)) AddItem(ItemConfig8, 1);
            if (Input.GetKeyDown(KeyCode.Alpha9)) AddItem(ItemConfig9, 1);

            TryUseHoveredItem();   // правый клик — использовать
            StartDrag();
            UpdateDragItemPosition();
            EndDrag();
        }

        // ---------------- ИСПОЛЬЗОВАНИЕ (ПКМ) ----------------

        private void TryUseHoveredItem()
        {
            if (IsDraging) return;
            if (!Input.GetMouseButtonDown(1)) return;

            var hovered = GetHoveredSlot();
            if (hovered == null || hovered is EquipedSlot) return;
            if (!hovered.IsHasItem() || playerStats == null) return;

            var item = hovered.GetItem();

            switch (item.itemType)
            {
                case ItemType.HealthPoint:
                    playerStats.ChangeStats(item.itemAddValue, StatsPlayer.Health);
                    break;
                case ItemType.Coin:
                    playerStats.ChangeStats(item.itemAddValue, StatsPlayer.Coin);
                    break;
                default:
                    return; // не расходник
            }

            hovered.RemoveItemAmount(1, item);
        }

        // ---------------- ДОБАВЛЕНИЕ ----------------

        public void AddItem(ItemConfig itemToAdd, int amount)
        {
            int remaining = amount;

            // Дозаполняем существующие стаки — ТОЛЬКО инвентарь
            foreach (ISlot slot in inventorySlots)
            {
                if (slot.IsHasItem() && slot.GetItem() == itemToAdd)
                {
                    int currentAmount = slot.GetItemAmount();
                    int maxStack = itemToAdd.maxStackSize;
                    if (currentAmount < maxStack)
                    {
                        int spaceLeft = maxStack - currentAmount;
                        int amountToAdd = Mathf.Min(spaceLeft, remaining);
                        slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                        remaining -= amountToAdd;
                        if (remaining <= 0) return;
                    }
                }
            }

            // Кладём в пустые — ТОЛЬКО инвентарь
            foreach (ISlot slot in inventorySlots)
            {
                if (!slot.IsHasItem())
                {
                    int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining);
                    slot.SetItem(itemToAdd, amountToPlace);
                    remaining -= amountToPlace;
                    if (remaining <= 0) return;
                }
            }

            if (remaining > 0)
                Debug.Log($"Inventory is full, could not add {remaining} of {itemToAdd.nameItem}");
        }

        // ---------------- ДРАГ ----------------

        private void StartDrag()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ISlot hovered = GetHoveredSlot();
                if (hovered != null && hovered.IsHasItem())
                {
                    draggetSlot = hovered;
                    IsDraging = true;

                    dragItemIcon.sprite = hovered.GetItem().icon;
                    dragItemIcon.color = colorDragItem;
                    dragItemIcon.enabled = true;
                }
            }
        }

        private void EndDrag()
        {
            if (Input.GetMouseButtonUp(0) && IsDraging)
            {
                ISlot hovered = GetHoveredSlot();
                if (hovered != null)
                    HandleDrop(draggetSlot, hovered);

                // ⚠️ сбрасываем состояние всегда, даже если бросили мимо слотов
                dragItemIcon.enabled = false;
                draggetSlot = null;
                IsDraging = false;
            }
        }

        private void UpdateDragItemPosition()
        {
            if (IsDraging)
                dragItemIcon.rectTransform.position = Input.mousePosition; // для Screen Space Overlay
        }

        // ---------------- DROP ----------------

        private void HandleDrop(ISlot from, ISlot to)
        {
            if (from == to) return;

            // Инвентарь → слот экипировки
            if (to is EquipedSlot equipTo)
            {
                TryEquip(from, equipTo);
                return;
            }

            // Слот экипировки → инвентарь
            if (from is EquipedSlot equipFrom)
            {
                TryUnequip(equipFrom, to);
                return;
            }

            // Оба — обычные слоты инвентаря
            if (to.IsHasItem() && to.GetItem() == from.GetItem())
            {
                int max = to.GetItem().maxStackSize;
                int space = max - to.GetItemAmount();

                if (space > 0)
                {
                    int move = Mathf.Min(space, from.GetItemAmount());
                    to.SetItem(to.GetItem(), to.GetItemAmount() + move);
                    from.SetItem(from.GetItem(), from.GetItemAmount() - move);

                    if (from.GetItemAmount() <= 0)
                        from.ClearSlot();
                    return;
                }
            }

            if (to.IsHasItem())
            {
                ItemConfig tempItem = to.GetItem();
                int tempAmount = to.GetItemAmount();

                to.SetItem(from.GetItem(), from.GetItemAmount());
                from.SetItem(tempItem, tempAmount);
                return;
            }

            to.SetItem(from.GetItem(), from.GetItemAmount());
            from.ClearSlot();
        }

        private void TryEquip(ISlot from, EquipedSlot to)
        {
            if (!from.IsHasItem()) return;

            ItemConfig item = from.GetItem();
            if (!item.isEquipingItem) return;

            var expected = ItemEquipHelper.ToEquipedType(item.itemType);
            if (expected == null || expected.Value != to.TypeEquiped) return;

            ItemConfig prevItem = to.GetItem();
            int prevAmount = to.GetItemAmount();

            from.RemoveItemAmount(1, item);
            to.SetItem(item, 1); // сам применит статы

            if (prevItem != null)
            {
                if (!from.IsHasItem())
                    from.SetItem(prevItem, prevAmount);
                else
                    AddItem(prevItem, prevAmount);
            }
        }

        private void TryUnequip(EquipedSlot from, ISlot to)
        {
            if (!from.IsHasItem()) return;

            ItemConfig item = from.GetItem();
            int amount = from.GetItemAmount();

            if (to.IsHasItem())
            {
                ItemConfig targetItem = to.GetItem();
                int targetAmount = to.GetItemAmount();

                // можно ли надеть то, что лежит в целевом слоте?
                if (!targetItem.isEquipingItem) return;
                var expected = ItemEquipHelper.ToEquipedType(targetItem.itemType);
                if (expected == null || expected.Value != from.TypeEquiped) return;

                from.ClearSlot();                 // снимает статы старого
                to.SetItem(item, amount);
                from.SetItem(targetItem, targetAmount); // применяет статы нового
            }
            else
            {
                from.ClearSlot();
                to.SetItem(item, amount);
            }
        }

        // ---------------- HOVER ----------------

        private ISlot GetHoveredSlot()
        {
            foreach (ISlot slot in allSlots)
                if (slot.Hovering) return slot;
            return null;
        }
    }
}