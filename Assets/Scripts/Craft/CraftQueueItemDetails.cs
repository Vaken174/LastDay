using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftQueueItemDetails : MonoBehaviour
{
    public int craftAmount;
    private int initialCraftTime;
    public TMP_Text amountText, timeText;
    public Image itemImage;
    public CraftScriptableObject currentCraftItem;
    public int craftTime;
    private InventoryManager inventoryManager;
    private CraftManager craftManager;
    private void Start()
    {
        craftManager = FindObjectOfType<CraftManager>();
        initialCraftTime = craftTime;
        inventoryManager = FindObjectOfType<InventoryManager>();
        craftTime++;

        if(transform.parent.childCount <= 1)
        {
            InvokeRepeating("UpdateTime", 0f, 1f);
        }
        else
        {
        UpdateTime();
        }
    }
    public void StartInvoke()
    {
        InvokeRepeating("UpdateTime", 0f, 1f);
    }
    void UpdateTime()
    {
        amountText.text = "X"+craftAmount.ToString();
        craftTime--;
        if (craftTime <= 0)
        {
            inventoryManager.AddItem(currentCraftItem.finalObject, currentCraftItem.craftAmount);
            craftAmount--;
            craftTime = initialCraftTime;
            if(craftAmount <=0)
            {
                CancelInvoke();
                if(transform.parent.childCount >1)
                    transform.parent.GetChild(1).GetComponent<CraftQueueItemDetails>().StartInvoke();
                Destroy(gameObject);
            }
        }
        else
        {
            int minutes = Mathf.FloorToInt(craftTime / 60);
            int seconds = craftTime - minutes * 60;
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    public void RemoveFromQueue()
    {
        foreach(CraftResourse resourse in currentCraftItem.craftResourses)
        {
            inventoryManager.AddItem(resourse.craftObject, resourse.craftObjectAmount * craftAmount);
        }
        CancelInvoke();
        craftManager.currentCraftItem.FillItemDetails();
        if (transform.parent.childCount > 1)
            transform.parent.GetChild(1).GetComponent<CraftQueueItemDetails>().StartInvoke();
        Destroy(gameObject);
    }
}
