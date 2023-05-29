using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FillCraftItemDitails : MonoBehaviour
{
    private CraftManager craftManager;
    public CraftScriptableObject currentCraftItem;
    public GameObject crafResourcePrefab;
    public string craftInfoPanelName;
    private CraftQueueManager craftQueueManager;
    private GameObject craftInfoPanelGO;
    private void Awake()
    {
        craftInfoPanelGO = GameObject.Find(craftInfoPanelName);
        craftManager = FindObjectOfType<CraftManager>();
        craftQueueManager = FindObjectOfType<CraftQueueManager>();

    }
    public void FillItemDetails()
    {
        craftManager.currentCraftItem = this;

        for (int i = 0; i < craftInfoPanelGO.transform.childCount; i++)
        {
            Destroy(craftInfoPanelGO.transform.GetChild(i).gameObject);
        }
        craftManager.craftItemName.text = currentCraftItem.finalObject.name;
        craftManager.craftItemDescription.text = currentCraftItem.finalObject.itemDescription;
        craftManager.craftItemIcon.sprite = currentCraftItem.finalObject.icon;
        craftManager.craftItemDuration.text = currentCraftItem.craftTime.ToString();
        craftManager.craftItemAmount.text = currentCraftItem.craftAmount.ToString();

        bool canCraft = true;
        for (int i = 0; i < currentCraftItem.craftResourses.Count; i++)
        {
            GameObject craftResourceGO = Instantiate(crafResourcePrefab, craftInfoPanelGO.transform);
            CraftResourseDetails crd = craftResourceGO.GetComponent<CraftResourseDetails>();
            crd.amountText.text = currentCraftItem.craftResourses[i].craftObjectAmount.ToString();
            crd.itemTypeText.text = currentCraftItem.craftResourses[i].craftObject.itemName;
            int totalAmount = currentCraftItem.craftResourses[i].craftObjectAmount * int.Parse(craftQueueManager.craftAmountInputField.text);
            crd.totalText.text = totalAmount.ToString();
            int resourceAmount = 0;
            foreach (Slot slot in FindObjectsOfType<InventoryManager>()[0].slots)
            {
                if (slot.isEmpty)
                    continue;
                if (slot.item.itemName == currentCraftItem.craftResourses[i].craftObject.itemName)
                {
                    resourceAmount += slot.amount;
                }
            }
            crd.haveText.text = resourceAmount.ToString();


            if (resourceAmount < totalAmount)
            {
                canCraft = false;
            }
        }
        if (canCraft)
        {
            craftManager.craftButton.interactable = true;
        }
        else
        {
            craftManager.craftButton.interactable = false;
        }
        craftQueueManager.currentCraftItem = currentCraftItem;
    }
}
