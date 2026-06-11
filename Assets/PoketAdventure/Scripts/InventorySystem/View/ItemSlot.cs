using System;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CloudeDev.Inventory.View
{
    public class ItemSlot : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnSelectedSlot;

        public string _itemName;
        public int _quantity;
        public Sprite _itemSprite;
        public bool isFull;
        public int _maxSize;

        [SerializeField]
        private TMP_Text quantityText;

        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private GameObject selectedSlot;
        public bool isSelectedSlot;

        public void AddItem(string itemName, int quantity, Sprite itemSprite)
        {
            if (isFull)
                return;

            _itemName = itemName;
            _itemSprite = itemSprite;
            itemImage.sprite = _itemSprite;

            if (quantity >= _maxSize)
            {
                isFull = true;
                quantityText.gameObject.SetActive(true);
                quantityText.text = _maxSize.ToString();

                int extraItems = _quantity - _maxSize;
                _quantity = _maxSize;
            }
            else
            {
                _quantity += quantity;
                quantityText.gameObject.SetActive(true);
                quantityText.text = _quantity.ToString();
            }

            if (_quantity >= _maxSize)
                isFull = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnSelectedSlot?.Invoke();
                OnLeftClick();
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick();
            }
        }

        public void OnLeftClick()
        {
            selectedSlot.SetActive(true);
            isSelectedSlot = true;
        }

        public void OnRightClick()
        {

        }

        public void DeselectSlots()
        {
            selectedSlot.SetActive(false);
            isSelectedSlot = false;
        }

        public void RemoveItem(string itemName, int quantity, Sprite sprite)
        {
            if (_quantity - quantity == 0)
            {
                _quantity = 0;
                _itemName = "";
                itemImage.sprite = null;
                quantityText.gameObject.SetActive(false);
                quantityText.text = _quantity.ToString();
                return;
            }
            _quantity -= quantity;
            quantityText.text = _quantity.ToString();

        }
    }
}
