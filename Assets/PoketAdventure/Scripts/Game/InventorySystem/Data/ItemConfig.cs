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
        [Header("Drop Settings")]
        [SerializeField] private int _minDropAmount = 1;
        [SerializeField] private int _maxDropAmount = 1;

        public int MinDropAmount => Mathf.Max(0, _minDropAmount);
        public int MaxDropAmount => Mathf.Max(MinDropAmount, _maxDropAmount);

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

