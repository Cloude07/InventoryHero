using CloudeDev.Game.ItemManager;
using CloudeDev.Inventory.View;
using UnityEngine;

namespace CloudeDev
{
    public class TestManagerInventory : MonoBehaviour
    {
        public ItemSlot[] itemSlot;
        public ItemConfig[] itemConfig;

        public string _itemName;
        public int _quantity;
         public Sprite _sprite;

        private void Start()
        {
            for (int i = 0; i < itemSlot.Length; i++)
            {
                itemSlot[i].OnSelectedSlot += OnSelectedSlot;
            }
        }

        private void OnSelectedSlot()
        {
            DeselectAllSlots();
        }

        private void OnDisable()
        {
            for (int i = 0; i < itemSlot.Length; i++)
            {
                itemSlot[i].OnSelectedSlot += OnSelectedSlot;
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                AddItem(_itemName, _quantity, _sprite);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                RemoveItem(_itemName, _quantity, _sprite);
            }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                DeselectAllSlots();
            }
        }
        public void AddItem(string itemName, int quantity, Sprite sprite)
        {
            for (int i = 0; i < itemSlot.Length;)
            {
                if (itemSlot[i].isFull == true)
                {
                    i++;
                }
                if (itemSlot[i].isFull == false && itemSlot[i]._itemName == itemName || itemSlot[i]._quantity == 0)
                {
                    itemSlot[i].AddItem(itemName, quantity, sprite);
                    return;
                }
         
                
            }
        }

        public void RemoveItem(string itemName, int quantity, Sprite sprite)
        {
            for (int i = 0; i < itemSlot.Length;)
            {
                if (itemSlot[i]._quantity == 0)
                {
                    i++;         
                }
                    
                if (itemSlot[i]._itemName == itemName || itemSlot[i]._quantity > 0)
                {
                    itemSlot[i].RemoveItem(itemName, quantity, sprite);           
                    return;
                }

            }
        }

        public void UseItem(string itemName)
        {
            for (int i = 0; i < itemConfig.Length; i++)
            {
                if (itemConfig[i]._name == itemName && 
                    itemConfig[i]._isEquipingItem == false)
                {
                    
                }
            }
        }

        public void DeselectAllSlots()
        {
            for (int i = 0; i < itemSlot.Length; i++)
            {
                itemSlot[i].DeselectSlots();
            }
        }
    }
}
