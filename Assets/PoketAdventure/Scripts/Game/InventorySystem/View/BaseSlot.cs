using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CloudeDev.Game.InventorySystem
{
    public class BaseSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler, IDropHandler
    {
        public Image icon;
        public TMP_Text quantityText;
        public ItemInstance currentItem;

        public virtual bool CanPlace(ItemSO item) => true;

        public virtual void SetItem(ItemInstance newItem)
        {
            currentItem = newItem;
            if (newItem != null)
            {
                icon.sprite = newItem.item.icon;
                icon.enabled = true;
                quantityText.text = newItem.quantity > 1 ? newItem.quantity.ToString() : "";
            }
            else
            {
                icon.sprite = null;
                icon.enabled = false;
                quantityText.text = "";
            }
        }

        // Drag & Drop реализация
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (currentItem == null) return;
            DragHandler.StartDrag(currentItem, icon.sprite);
            SetItem(null); // временно очищаем
        }

        public void OnDrag(PointerEventData eventData) { }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!DragHandler.IsDragging) return;
            BaseSlot targetSlot = DragHandler.GetTargetSlot();
            if (targetSlot != null && targetSlot != this)
                SwapItems(targetSlot);
            else
                SetItem(DragHandler.EndDrag());
            DragHandler.StopDrag();
        }

        public void OnDrop(PointerEventData eventData)
        {
            DragHandler.SetTargetSlot(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // Контекстное меню: экипировать/использовать
                InventoryManager.Instance.ShowContextMenu(this);
            }
        }

        protected virtual void SwapItems(BaseSlot other)
        {
            ItemInstance temp = other.currentItem;
            other.SetItem(currentItem);
            SetItem(temp);
            InventoryManager.Instance.SaveInventory();
        }
    }
}