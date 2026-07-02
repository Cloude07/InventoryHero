using UnityEngine;

namespace CloudeDev.Game.Inventory
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Configs/ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        public string nameItem;
        public Sprite icon;
        public ItemType itemType;
        public EquipedType equipedType;
        public GameObject objectItemEquiped;
        public int maxStackSize;
        public bool isEquipingItem;


    }
}

