using UnityEngine;
using UnityEngine.UI;

namespace CloudeDev.Game.InventorySystem
{
    public static class DragHandler
    {
        private static GameObject dragObject;
        private static Image dragImage;
        private static ItemInstance draggedItem;
        private static BaseSlot targetSlot;
        public static bool IsDragging => dragObject != null;

        public static void StartDrag(ItemInstance item, Sprite icon)
        {
            draggedItem = item;
            dragObject = new GameObject("DragIcon");
            dragImage = dragObject.AddComponent<Image>();
            dragImage.sprite = icon;
            dragImage.raycastTarget = false;
            dragObject.transform.SetParent(InventoryManager.Instance.transform.root, false);
            dragObject.transform.SetAsLastSibling();
        }

        public static void SetTargetSlot(BaseSlot slot) => targetSlot = slot;
        public static BaseSlot GetTargetSlot() => targetSlot;
        public static ItemInstance EndDrag() { var item = draggedItem; draggedItem = null; Object.Destroy(dragObject); return item; }
        public static void StopDrag() { if (dragObject) Object.Destroy(dragObject); }
    }
}