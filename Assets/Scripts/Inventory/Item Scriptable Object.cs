using UnityEngine;

public enum ItemType { Material, Food, Weapons, Instrument, Medical}
public class ItemScriptableObject : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public string itemDescription;
    public int MaximumAmount;
    public string inHandName;
    public GameObject itemPrefab;
    public Sprite icon;
    public int maxHealth;
    public bool isConsumeable;

    [Header("Consumable Characteristics")]
    public float changeHealth;
    public float changeHunger;
    public float changeThirst;
}
