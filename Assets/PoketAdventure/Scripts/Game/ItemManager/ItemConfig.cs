using System;
using UnityEngine;

namespace CloudeDev.Game.ItemManager
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Configs/ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        public ItemType _itemType;
        public Sprite _icon;
        public string _name;
        public string _description;
        public int _count = 1;
        public bool _isEquipingItem;
        public EquipedType _equipedType;


    }
}

