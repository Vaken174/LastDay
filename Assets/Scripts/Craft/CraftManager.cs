using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CraftManager : MonoBehaviour
{
    public bool isOpen;
    [SerializeField]
    private GameObject craftingPanel;
    public GameObject inventoryPanel;
    [SerializeField]
    private GameObject UIBG;
    [SerializeField]
    private GameObject crosshair;

    private InventoryManager inventoryManager;

    public Button craftButton;
    public FillCraftItemDitails currentCraftItem;

    public Transform CraftItemsPanel;
    public GameObject craftItemButtonPrefabs;

    public List<CraftScriptableObject> allCrafts;

    [SerializeField]
    private KeyCode openCloseCraft;

    [Header("Craft Item Details")]
    public TMP_Text craftItemName;
    public TMP_Text craftItemDescription;
    public Image craftItemIcon;
    public TMP_Text craftItemDuration;
    public TMP_Text craftItemAmount;

    public void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

        GameObject craftItemButton = Instantiate(craftItemButtonPrefabs, CraftItemsPanel);
        craftItemButton.GetComponent<Image>().sprite = allCrafts[0].finalObject.icon;
        craftItemButton.GetComponent<FillCraftItemDitails>().currentCraftItem = allCrafts[0];
        craftItemButton.GetComponent<FillCraftItemDitails>().FillItemDetails();
        Destroy(craftItemButton);
        craftingPanel.gameObject.SetActive(false);
    }

    public void FillItemDetailsHelper()
    {
        currentCraftItem.FillItemDetails();
    }

    public void Update()
    {
        if (Input.GetKeyDown(openCloseCraft))
        {
            if (!inventoryManager.isOpen)
            {
                isOpen = !isOpen;
                GetComponent<InventoryManager>().isOpen = false;
                if (isOpen)
                {
                    craftingPanel.SetActive(true);
                    UIBG.SetActive(true);
                    crosshair.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Camera_Movement.sensitivityMouse = 0f;

                }
                else
                {
                    craftingPanel.SetActive(false);
                    UIBG.SetActive(false);
                    crosshair.SetActive(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Camera_Movement.sensitivityMouse = 250;

                }
            }
        }
    }

    public void LoadCraftItem(string craftType)
    {
        for (int i = 0; i < CraftItemsPanel.childCount; i++)
        {
            Destroy(CraftItemsPanel.GetChild(i).gameObject);
        }
        foreach (CraftScriptableObject cso in allCrafts)
        {
            if (cso.craftType.ToString().ToLower() == craftType.ToLower())
            {
                GameObject craftItemButton = Instantiate(craftItemButtonPrefabs, CraftItemsPanel);
                craftItemButton.GetComponent<Image>().sprite = cso.finalObject.icon;
                craftItemButton.GetComponent<FillCraftItemDitails>().currentCraftItem = cso;
            }
        }
    }
}
