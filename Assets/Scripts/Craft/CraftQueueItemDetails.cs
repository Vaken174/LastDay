using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftQueueItemDetails : MonoBehaviour
{
    public TMP_Text amountText, timeText;
    public Image itemImage;
    public CraftScriptableObject currentCraftItem;
    public int craftTime;
    private InventoryManager inventoryManager;
    private CraftManager craftManager;

    private void Start()
    {
        craftManager = FindObjectOfType<CraftManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        InvokeRepeating("UpdateTime", 1f,1f);
    }
    void UpdateTime()
    {
        
        craftTime--;
        if (craftTime <= 0)
        {
            craftManager.craftButton.interactable = true;
            inventoryManager.AddItem(currentCraftItem.finalObject, currentCraftItem.craftAmount);
            CancelInvoke();
            Destroy(gameObject);
        }
        else
        {
            craftManager.craftButton.interactable = false;
            int minutes = Mathf.FloorToInt(craftTime / 60);
            int seconds = craftTime - minutes * 60;
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
