using UnityEngine;

[CreateAssetMenu(fileName = "NewItemConfig", menuName = "Inventory/ItemConfig")]
public class ItemConfiguration : ScriptableObject
{
    public uint id;
    public string type;
    public string itemName;
    public float weight;
}