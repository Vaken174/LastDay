using UnityEngine;

public enum ItemType { Material, Food, Weapons, Instrument, Medical}
public class ItemScriptableObject : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public GameObject itemPrefab;
    public Sprite icon;
    public int MaximumAmount;
    public string itemDescription;
    public string inHandName;
    public bool isConsumeable;
    public int maxHealth;

    [Header("Consumable Characteristics")]
    public float changeHealth;
    public float changeHunger;
    public float changeThirst;
}
