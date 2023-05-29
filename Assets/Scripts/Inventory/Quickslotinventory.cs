using UnityEngine;
using UnityEngine.UI;

public class Quickslotinventory : MonoBehaviour
{
    // ������ � �������� ���� �������� �������
    public Transform quickslotParent;
    public InventoryManager inventoryManager;
    public int currentQuickslotID = 0;
    public Sprite selectedSprite;
    public Sprite notSelectedSprite;
    public Text healthText;
    public Transform itemContainer;
    public  Slot activeSlot = null;
    public Transform allWeapons;
    public Indicators indicators;

    // Update is called once per frame
    void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        // ���������� �������� �����
        if (mw > 0.1)
        {
            // ����� ���������� ���� � ������ ��� �������� �� �������

            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            // ����� ��������� ��� �������� ����� �� ������� ��������� �� ����� (��������� ������ ��� �������, �������� �������� ...)

            // ���� ������ ��������� ����� ������ � ���� ����� currentQuickslotID ����� ���������� �����, �� �������� ��� ������ ���� (������ ���� ��������� �������)
            if (currentQuickslotID >= quickslotParent.childCount - 1)
            {
                currentQuickslotID = 0;
            }
            else
            {
                // ���������� � ����� currentQuickslotID ��������
                currentQuickslotID++;
            }
            // ����� ���������� ���� � ������ ��� �������� �� "���������"

            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
            activeSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>();
            ShowItemInHand();
            // ����� ��������� ��� �������� ����� �� �������� ���� (�������� ������ ��� �������, �������� �������� ...)

        }
        if (mw < -0.1)
        {
            // ����� ���������� ���� � ������ ��� �������� �� �������

            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            // ����� ��������� ��� �������� ����� �� ������� ��������� �� ����� (��������� ������ ��� �������, �������� �������� ...)


            // ���� ������ ��������� ����� ����� � ���� ����� currentQuickslotID ����� 0, �� �������� ��� ��������� ����
            if (currentQuickslotID <= 0)
            {
                currentQuickslotID = quickslotParent.childCount - 1;
            }
            else
            {
                // ��������� ����� currentQuickslotID �� 1
                currentQuickslotID--;
            }
            // ����� ���������� ���� � ������ ��� �������� �� "���������"

            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
            activeSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>();
            ShowItemInHand();
            // ����� ��������� ��� �������� ����� �� �������� ���� (�������� ������ ��� �������, �������� �������� ...)

        }
        // ���������� �����
        for (int i = 0; i < quickslotParent.childCount; i++)
        {
            // ���� �� �������� �� ������� 1 �� 5 ��...
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                // ��������� ���� ��� ��������� ���� ����� ����� ������� � ��� ��� ������, ��
                if (currentQuickslotID == i)
                {
                    // ������ �������� "selected" �� ���� ���� �� "not selected" ��� ��������
                    if (quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == notSelectedSprite)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                        activeSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>();
                        ShowItemInHand();
                        //foreach ...
                    }
                    else
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                        activeSlot = null;
                        HideItemsInHand();
                        //foreach ...

                    }
                }
                // ����� �� ������� �������� � ����������� ����� � ������ ���� ������� �� ��������
                else
                {
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    // ����� ���� ����� FOREACH ���� ������� ����� ��������� ��� ������� � ������� ITEMS
                    currentQuickslotID = i;

                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                    activeSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>();
                    ShowItemInHand();
                    // ���� �������� �� ��� �� ����� ����� ������� ����� <--
                }
            }
        }
        // ���������� ������� �� ������� �� ����� ������ ����
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>().item != null)
            {
                if (quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>().item.isConsumeable && !inventoryManager.isOpen && quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == selectedSprite)
                {
                    // ��������� ��������� � �������� (������� � ������ � �����) 
                    ChangeCharacteristics();

                    if (quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>().amount <= 1)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponentInChildren<DragAndDrop>().NullifySlotData();
                    }
                    else
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>().amount--;
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>().itemAmount.text = quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>().amount.ToString();
                    }
                }
            }
        }
    }

    public void CheckItemInHand() 
    {
        if (activeSlot != null)
            ShowItemInHand();
        else
            HideItemsInHand();
    }
    private void ChangeCharacteristics()
    {
        indicators.ChangeFoodAmount(quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>().item.changeHunger);
        indicators.ChangeWaterAmount(quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>().item.changeThirst);
        indicators.ChangeHealthAmount(quickslotParent.GetChild(currentQuickslotID).GetComponent<Slot>().item.changeHealth);
    }
    private void ShowItemInHand()
    {
        HideItemsInHand();
        if (activeSlot.item == null)
        {
            return;
        }
        for (int i = 0; i < allWeapons.childCount; i++)
        {
            if (activeSlot.item.inHandName == allWeapons.GetChild(i).name)
            {
                allWeapons.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    private void HideItemsInHand()
    {
        for (int i = 0; i < allWeapons.childCount; i++)
        {
            allWeapons.GetChild(i).gameObject.SetActive(false);
        }
    }
}