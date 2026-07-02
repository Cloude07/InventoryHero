using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CloudeDev.Game.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [Header("Test")]
        public ItemConfig HealthItem;
        public ItemConfig SwordItem;

        [Header("ClassArrtebut")]
        public GameObject inventoryObject;
        public GameObject inventoryEquipedObject;

        public Image dragItemIcon;

        private ISlot draggetSlot = null;
        private bool IsDraging = false;

        private List<ISlot> inventorySlots = new List<ISlot>();
     // private List<ISlot> inventoryEquipedSlots = new List<ISlot>();
        private List<ISlot> allSlots = new List<ISlot>();

        private void Awake()
        {
            inventorySlots.AddRange(inventoryObject.GetComponentsInChildren<Slot>());
        //    inventoryEquipedSlots.AddRange(inventoryEquipedObject.GetComponentsInChildren<Slot>());
            allSlots.AddRange(inventorySlots);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddItem(HealthItem, 2);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                AddItem(SwordItem, 1);
            }

            StartDrag();
            UpdateDragItemPosition();
            EndDrag();
        }

        public void AddItem(ItemConfig itemToAdd, int amount)
        {
            int remaining = amount;
            foreach (ISlot slot in allSlots)
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

                        if (remaining <= 0)
                            return;

                    }
                }
            }

            foreach (ISlot slot in allSlots)
            {
                if (!slot.IsHasItem())
                {
                    int amountToPlase = Mathf.Min(itemToAdd.maxStackSize, remaining);
                    slot.SetItem(itemToAdd, amountToPlase);
                    remaining -= amountToPlase;
                    if (remaining <= 0)
                        return;
                }
            }

            if (remaining > 0)
                Debug.Log($"inventory is Full, could not add {remaining} of {itemToAdd.nameItem}");

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
                    dragItemIcon.color = new Color(1, 1, 1, 0.5f);
                    dragItemIcon.enabled = true;
                }
            }
        }

        private void EndDrag()
        {
            if (Input.GetMouseButtonUp(0) && IsDraging == true)
            {
                ISlot hovered = GetHoveredSlot();
                if (hovered != null)
                {
                    HandleDrop(draggetSlot, hovered);
                    dragItemIcon.enabled = false;
                    draggetSlot = null;
                    IsDraging = false;

                }
            }
        }

        private void HandleDrop(ISlot from, ISlot to)
        {
            if (from == to)
                return;
            //Stacking
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

            //Different
            if(to.IsHasItem())
            {
                ItemConfig tempItem = to.GetItem();
                int tempAmount = to.GetItemAmount();

                to.SetItem(from.GetItem(), from.GetItemAmount());
                from.SetItem(tempItem, tempAmount);
                return;
            }
            // Empty

            to.SetItem(from.GetItem(), from.GetItemAmount());
            from.ClearSlot();
        }



        private ISlot GetHoveredSlot()
        {
            foreach (ISlot slot in allSlots)
            {
                if (slot.Hovering)
                    return slot;
            }
            return null;
        }

        private void UpdateDragItemPosition()
        {
            if(IsDraging)
            {
                dragItemIcon.rectTransform.anchoredPosition = Input.mousePosition;
            }
        }
    }
}

