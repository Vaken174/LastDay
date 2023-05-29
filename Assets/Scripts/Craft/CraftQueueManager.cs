using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftQueueManager : MonoBehaviour
{
    public int craftTime;
    public TMP_InputField craftAmountInputField;
    
    public GameObject craftQueuePrefabs;
    public CraftScriptableObject currentCraftItem;

    private CraftManager craftManager;
    public InventoryManager inventoryManager;


    public void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        craftManager = FindObjectOfType<CraftManager>();
        
    }
    public void AddToCraftQueue() 
    {
        foreach (CraftResourse craftResourse in currentCraftItem.craftResourses)
        {
            int amountToRemove = craftResourse.craftObjectAmount* int.Parse(craftAmountInputField.text);
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

        for(int i = 0; i < transform.childCount;i++)
        {
            if(transform.GetChild(i).GetComponent<CraftQueueItemDetails>().currentCraftItem == currentCraftItem)
            {
                transform.GetChild(i).GetComponent<CraftQueueItemDetails>().craftAmount += int.Parse(craftAmountInputField.text);
                return;
            }
        }
        GameObject craftQueueInstance = Instantiate(craftQueuePrefabs, transform);
        CraftQueueItemDetails craftQueueItemDetails = craftQueueInstance.GetComponent<CraftQueueItemDetails>();
        craftQueueItemDetails.itemImage.sprite = currentCraftItem.finalObject.icon;
        craftQueueItemDetails.amountText.text = craftAmountInputField.text;
        craftQueueItemDetails.craftAmount = int.Parse(craftAmountInputField.text);
        craftTime = currentCraftItem.craftTime;
        int minutes = Mathf.FloorToInt(craftTime/60);
        int seconds = craftTime - minutes *60;
        craftQueueItemDetails.timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        craftQueueItemDetails.craftTime = craftTime;
        craftQueueItemDetails.currentCraftItem = currentCraftItem;

        craftManager.currentCraftItem.FillItemDetails();
    }

    public void RemoveButtonFuncrion()
    {
        if (int.Parse(craftAmountInputField.text) <= 1)
            return;
        int newAmount = int.Parse(craftAmountInputField.text) - 1;
        craftAmountInputField.text = newAmount.ToString();
    }
    public void AddButtonFunction()
    {
        if (int.Parse(craftAmountInputField.text) >= 999)
            return;
        int newAmount = int.Parse(craftAmountInputField.text) + 1;
        craftAmountInputField.text = newAmount.ToString();
    }
}
