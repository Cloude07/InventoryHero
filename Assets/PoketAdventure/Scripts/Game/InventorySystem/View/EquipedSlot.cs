using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CloudeDev.Game.Inventory
{
    public class EquipedSlot : MonoBehaviour, ISlot, IPointerEnterHandler, IPointerExitHandler
    {
        public EquipedType TypeEquiped => type;
        public ItemConfig itemConfig;
        public bool Hovering => hovering;

        [SerializeField]
        private EquipedType type;
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private GameObject selectView;
        private bool hovering;

        private void Start()
        {
            UpdateSlot();
        }

        private void UpdateSlot()
        {
            if (itemConfig != null)
            {
                iconImage.enabled = true;
                iconImage.sprite = itemConfig.icon;
            }
            else
            {
                iconImage.enabled = false;
            }
        }

        public void ClearSlot()
        {
            itemConfig = null;
            UpdateSlot();
        }

        public void EquipedItem()
        {

        }

        public ItemConfig GetItem()
        {
            throw new System.NotImplementedException();
        }



        public bool IsHasItem()
        {
            return itemConfig != null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovering = true;
            selectView.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
            selectView.SetActive(false);
        }

        public int RemoveItemAmount(int value, ItemConfig item)
        {
            throw new System.NotImplementedException();
        }

        public void SetItem(ItemConfig newItemConfig, int amount = 1)
        {
            throw new System.NotImplementedException();
        }

        public int GetItemAmount()
        {
            throw new NotImplementedException();
        }
    }
}
