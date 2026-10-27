using CloudeDev.Game.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace CloudeDev.Game.Inventory
{
    public class EquipedSlot : MonoBehaviour, ISlot, IPointerEnterHandler, IPointerExitHandler
    {
        public EquipedType TypeEquiped => type;
        public ItemConfig itemConfig;
        public int itemAmount;
        public bool Hovering => hovering;

        [SerializeField] private EquipedType type;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject selectView;
        private IPlayerStatsModifier playerStats;
        [SerializeField] private Transform equipParent;   // ← куда спавнить визуал (рука/тело)

        private bool hovering;
        private bool statsApplied;
        private GameObject currentVisual;

        private void Start()
        {
            // Если предмет был выставлен в инспекторе — применим его статы один раз
            if (itemConfig != null && !statsApplied)
            {
                ApplyStats();
                statsApplied = true;
            }
            UpdateSlot();
        }

        [Inject]
        private void Construct(IPlayerStatsModifier stats)
        {
            playerStats = stats;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
            selectView.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovering = true;
            selectView.SetActive(true);
        }

        public ItemConfig GetItem() => itemConfig;
        public int GetItemAmount() => itemAmount;

        public void SetItem(ItemConfig newItemConfig, int amount = 1)
        {
            RemoveStats();                 // снимаем статы СТАРОГО предмета
            itemConfig = newItemConfig;
            itemAmount = amount;
            ApplyStats();                  // накладываем статы НОВОГО
            statsApplied = true;
            UpdateSlot();
        }

        public int AddItemAmount(int value, ItemConfig item)
        {
            if (item.nameItem == itemConfig.nameItem &&
                itemAmount + value <= itemConfig.maxStackSize)
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

        public bool IsHasItem() => itemConfig != null;

        public void ClearSlot()
        {
            RemoveStats();
            itemConfig = null;
            itemAmount = 0;
            statsApplied = false;
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

            EquipedItem(); // визуал на персонаже
        }

        private void ApplyStats()
        {
            if (itemConfig == null || playerStats == null) return;
            if (itemConfig.modifiers == null) return;

            foreach (var m in itemConfig.modifiers)
                playerStats.ChangeStats(m.value, m.stat);
        }

        private void RemoveStats()
        {
            if (itemConfig == null || !statsApplied || playerStats == null) return;
            if (itemConfig.modifiers == null) return;

            foreach (var m in itemConfig.modifiers)
                playerStats.ChangeStats(-m.value, m.stat);
        }

        /// <summary>
        /// Спавн/деспавн визуальной модели на персонаже.
        /// </summary>
        public void EquipedItem()
        {
            if (currentVisual != null)
                Destroy(currentVisual);

            if (itemConfig != null && itemConfig.objectItemEquiped != null && equipParent != null)
                currentVisual = Instantiate(itemConfig.objectItemEquiped, equipParent);
        }
    }
}