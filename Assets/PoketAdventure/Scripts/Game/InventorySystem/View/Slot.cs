using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CloudeDev.Game.Inventory
{
    public class Slot : MonoBehaviour, ISlot, IPointerEnterHandler, IPointerExitHandler
    {  
        public ItemConfig itemConfig;
        public int itemAmount;
        [SerializeField]
        private Image IconImage;
        [SerializeField]
        private TMP_Text textAmount;
        [SerializeField]
        private GameObject SelectView;
        public bool Hovering => hovering;
        private bool hovering;

        private void Start()
        {
            UpdateSlot();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
            SelectView.SetActive(false);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            hovering = true;
            SelectView.SetActive(true);
        }

        public ItemConfig GetItem()
        {
            return itemConfig;
        }

        public int GetItemAmount()
        {
            return itemAmount;
        }

        public void SetItem(ItemConfig newItemConfig, int amount = 1)
        {
            itemConfig = newItemConfig;
            itemAmount = amount;

            UpdateSlot();
        }

        public int AddItemAmount(int value, ItemConfig item)
        {
            if (item.nameItem == itemConfig.nameItem &&
                value <= itemConfig.maxStackSize)
            {
                itemAmount += value;
                UpdateSlot();
                return itemAmount;
            }
            return value;
        }

        public int RemoveItemAmount(int value, ItemConfig item)
        {
            if (item.nameItem == itemConfig.nameItem)
            {
                itemAmount -= value;

                if (itemAmount <= 0)
                    ClearSlot();
                else
                    UpdateSlot();

                return itemAmount;
            }
            return value;
        }

        public bool IsHasItem()
        {
            return itemConfig != null;
        }

        public void ClearSlot()
        {
            itemConfig = null;
            itemAmount = 0;
            UpdateSlot();
        }

        private void UpdateSlot()
        {
            if (itemConfig != null)
            {
                IconImage.enabled = true;
                IconImage.sprite = itemConfig.icon;
                textAmount.text = itemAmount.ToString();
            }
            else
            {
                IconImage.enabled = false;
                textAmount.text = "";
            }
        }


    }
}
