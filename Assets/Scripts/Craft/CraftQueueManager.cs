using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftQueueManager : MonoBehaviour
{
    public int craftTime;
    public TMP_InputField craftAmountInputField;

    public GameObject craftQueuePrefabs;
    public CraftScriptableObject currentCraftItem;

    private CraftManager craftManager;
    public InventoryManager inventoryManager;


    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        craftManager = FindObjectOfType<CraftManager>();
    }
    public void AddToCraftQueue() 
    {
        foreach (CraftResourse craftResourse in currentCraftItem.craftResourses)
        {
            int amountToRemove = craftResourse.craftObjectAmount;
            foreach (Slot slot in inventoryManager.slots)
            {
                if (amountToRemove <= 0)
                    continue;
                if(slot.item ==craftResourse.craftObject)
                {
                    if(amountToRemove > slot.amount)
                    {
                        amountToRemove -= slot.amount;
                        slot.GetComponentInChildren<DragAndDrop>().NullifySlotData();
                    }
                    else
                    {
                        slot.amount -= amountToRemove;
                        if(slot.amount <=0)
                        {
                            slot.GetComponentInChildren<DragAndDrop>().NullifySlotData();
                        }
                        else
                        {
                            slot.itemAmount.text = slot.amount.ToString();
                        }
                    }
                }
            }
        }
        GameObject craftQueueInstance = Instantiate(craftQueuePrefabs, transform);
        CraftQueueItemDetails craftQueueItemDetails = craftQueueInstance.GetComponent<CraftQueueItemDetails>();
        craftQueueItemDetails.itemImage.sprite = currentCraftItem.finalObject.icon;
        craftQueueItemDetails.amountText.text = craftAmountInputField.text;
        craftTime = currentCraftItem.craftTime;
        int minutes = Mathf.FloorToInt(craftTime/60);
        int seconds = craftTime - minutes *60;
        craftQueueItemDetails.timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        craftQueueItemDetails.craftTime = craftTime;
        craftQueueItemDetails.currentCraftItem = currentCraftItem;

        //craftManager.currentCraftItem.FillItemDetails();
    }
}
