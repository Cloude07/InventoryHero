using CloudeDev.Game.Player;
using System;
using UnityEngine;

namespace CloudeDev.Game.Inventory
{
    [Serializable]
    public struct StatModifier
    {
        public StatsPlayer stat;
        public int value;
    }

    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Configs/ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        public string nameItem;
        public Sprite icon;
        public ItemType itemType;
        public GameObject objectItemEquiped;
        public int maxStackSize;
        public int itemAddValue;
        public bool isEquipingItem;


        [Header("Stat Modifiers (применяются при экипировке)")]
        public StatModifier[] modifiers;
    }
}

