using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject UIBG;
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private Transform InventoryPanel;
    [SerializeField]
    private Transform quickSlotPanel;
    [SerializeField]
    private KeyCode openCloseInventory;

    
    private CraftManager craftManager;
    
    public bool isOpen = true;
    public float sensitivity;

    public List<Slot> slots = new List<Slot>();
    private Camera mainCamera;

    public float reachDistante = 3f;


    void Start()
    {
        craftManager = FindObjectOfType<CraftManager>();

        for (int i = 0; i < InventoryPanel.childCount; i++)
        {
            if (InventoryPanel.GetChild(i).GetComponent<Slot>() != null)
            {
                slots.Add(InventoryPanel.GetChild(i).GetComponent<Slot>());
            }
        }
        for (int i = 0; i < quickSlotPanel.childCount; i++)
        {
            if (quickSlotPanel.GetChild(i).GetComponent<Slot>() != null)
            {
                slots.Add(quickSlotPanel.GetChild(i).GetComponent<Slot>());
            }
        }

        InventoryPanel.gameObject.SetActive(false);
        UIBG.SetActive(false);

        mainCamera = Camera.main;

        sensitivity = Camera_Movement.sensitivityMouse;
    }

     public void Update()
    {
        OpenInventory();
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(ray, out hit, reachDistante))
            {
                if (hit.collider.gameObject.GetComponent<Item>() != null)
                {
                    AddItem(hit.collider.gameObject.GetComponent<Item>().item, hit.collider.gameObject.GetComponent<Item>().amount);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
    public void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach (Slot slot in slots)
        {
            if (slot.item == _item)
            {
                if (slot.amount + _amount <= _item.MaximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmount.text = slot.amount.ToString();
                    return;
                }
                continue;
            }
        }
        foreach (Slot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                if(slot.item.MaximumAmount != 1)
                {
                slot.itemAmount.text = _amount.ToString();
                }
                break;
            }
        }
    }


    private void OpenInventory()
    {
        if (Input.GetKeyDown(openCloseInventory))
        {
            if (!craftManager.isOpen)
            {
                isOpen = !isOpen;
                if (isOpen)
                {
                    InventoryPanel.gameObject.SetActive(true);
                    crosshair.SetActive(false);
                    UIBG.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Camera_Movement.sensitivityMouse = 0f;
                }
                else
                {
                    InventoryPanel.gameObject.SetActive(false);
                    crosshair.SetActive(true);
                    UIBG.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Camera_Movement.sensitivityMouse = 250;
                }
            }
        }
    }
}