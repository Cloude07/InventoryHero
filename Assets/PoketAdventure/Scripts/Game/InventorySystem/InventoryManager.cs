using CloudeDev.Game.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace CloudeDev.Game.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject inventoryObject;
        [SerializeField] private GameObject inventoryEquipedObject;
        [SerializeField] private Image dragItemIcon;
        [SerializeField] private RectTransform _inventoryPanel;

        private IPlayerStatsModifier playerStats;
        private IPlayerStatsReadonly playerStatsRead;

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

            _inventoryPanel = this.gameObject.GetComponent<RectTransform>();

        }

        [Inject]
        private void Construct(IPlayerStatsModifier statsMod, IPlayerStatsReadonly statsRead)
        {
            playerStats = statsMod;
            playerStatsRead = statsRead;
        }

        private void Update()
        {
            StartDrag();
            UpdateDragItemPosition();
            EndDrag();
        }


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
                    dragItemIcon.transform.SetAsLastSibling();
                }
            }
        }

        private void EndDrag()
        {
            if (Input.GetMouseButtonUp(0) && IsDraging)
            {
                ISlot hovered = GetHoveredSlot();

                if (hovered != null)
                {
                    HandleDrop(draggetSlot, hovered);
                }
                else if (!IsOverInventoryPanel())
                {

                    // 🔴 курсор вне панели инвентаря и не над слотом — выбрасываем
                    DiscardItem(draggetSlot);

                }
                // иначе — просто отмена (бросили над панелью, но не в слот)

                dragItemIcon.enabled = false;
                draggetSlot = null;
                IsDraging = false;
            }
        }

        private void UpdateDragItemPosition()
        {
            if (!IsDraging || dragItemIcon == null) return;

            var rt = dragItemIcon.rectTransform;
            var parentRT = rt.parent as RectTransform;
            if (parentRT == null) return;

            // Камера Canvas'а (для Screen Space - Camera это canvas.worldCamera,
            // для Overlay — null, RectTransformUtility сам разберётся)
            Camera cam = null;
            if (dragItemIcon.canvas != null &&
                dragItemIcon.canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                cam = dragItemIcon.canvas.worldCamera != null
                    ? dragItemIcon.canvas.worldCamera
                    : Camera.main;
            }

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRT, Input.mousePosition, cam, out var localPoint))
            {
                rt.localPosition = localPoint;
            }
        }

        // ---------------- DROP ----------------

        private void DiscardItem(ISlot slot)
        {
            if (slot == null || !slot.IsHasItem()) return;

            ItemConfig item = slot.GetItem();
            int amount = slot.GetItemAmount();

            // (опционально) запрет на выбрасывание квестовых/уникальных предметов
            // if (!item.canBeDiscarded) return;

            if(!item.isEquipingItem)
            {
                switch (item.itemType)
                {
                    case ItemType.HealthPoint:
                        // 🔴 проверка: не тратим аптечку, если HP уже полное
                        if (playerStatsRead.Health >= playerStatsRead.MaxHealth) return;

                        playerStats.ChangeStats(item.itemAddValue, StatsPlayer.Health);
                        slot.RemoveItemAmount(1, item);
                        return;   // ← выходим, чтобы нижний RemoveItemAmount не сработал

                    case ItemType.Coin:
                        playerStats.ChangeStats(item.itemAddValue, StatsPlayer.Coin);
                        slot.RemoveItemAmount(1, item);
                        return;

                    default:
                        return;   // не расходник — ничего не делаем
                }
            }

            slot.ClearSlot();
            Debug.Log($"[Inventory] Discarded {amount}x {item.nameItem}");
        }

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

        private bool IsOverInventoryPanel()
        {
            if (_inventoryPanel == null) return true;   // если не назначено — считаем "внутри"

            Camera cam = null;
            if (dragItemIcon != null && dragItemIcon.canvas != null &&
                dragItemIcon.canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                cam = dragItemIcon.canvas.worldCamera != null
                    ? dragItemIcon.canvas.worldCamera
                    : Camera.main;
            }

            return RectTransformUtility.RectangleContainsScreenPoint(
                _inventoryPanel, Input.mousePosition, cam);
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