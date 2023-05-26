using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
/// IPointerDownHandler - Следит за нажатиями мышки по объекту на котором висит этот скрипт
/// IPointerUpHandler - Следит за отпусканием мышки по объекту на котором висит этот скрипт
/// IDragHandler - Следит за тем не водим ли мы нажатую мышку по объекту
public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Slot oldSlot;
    private Transform player;
    private Quickslotinventory Quickslotinventory;


    private void Start()
    {
        Quickslotinventory = FindObjectOfType<Quickslotinventory>();
        //ПОСТАВЬТЕ ТЭГ "PLAYER" НА ОБЪЕКТЕ ПЕРСОНАЖА!
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Находим скрипт InventorySlot в слоте в иерархии
        oldSlot = transform.GetComponentInParent<Slot>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Если слот пустой, то мы не выполняем то что ниже return;
        if (oldSlot.isEmpty)
            return;
        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;
        //Делаем картинку прозрачнее
        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f);
        // Делаем так чтобы нажатия мышкой не игнорировали эту картинку
        GetComponentInChildren<Image>().raycastTarget = false;
        // Делаем наш DraggableObject ребенком InventoryPanel чтобы DraggableObject был над другими слотами инвенторя
        transform.SetParent(transform.parent.parent);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;
        // Делаем картинку опять не прозрачной
        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);
        // И чтобы мышка опять могла ее засечь
        GetComponentInChildren<Image>().raycastTarget = true;

        //Поставить DraggableObject обратно в свой старый слот
        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;
        //Если мышка отпущена над объектом по имени UIPanel, то...
        if (eventData.pointerCurrentRaycast.gameObject.name == "UIBackground")
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GameObject gameObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
                gameObject.GetComponent<Item>().amount = Mathf.CeilToInt((float)oldSlot.amount / 2);
                oldSlot.amount -= Mathf.CeilToInt((float)oldSlot.amount / 2);
                oldSlot.itemAmount.text = oldSlot.amount.ToString();
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                GameObject gameObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
                gameObject.GetComponent<Item>().amount = 1;
                oldSlot.amount--;
                oldSlot.itemAmount.text = oldSlot.amount.ToString();
            }
            else
            {
                // Выброс объектов из инвентаря - Спавним префаб обекта перед персонажем
                GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
                // Устанавливаем количество объектов такое какое было в слоте
                itemObject.GetComponent<Item>().amount = oldSlot.amount;
                // убираем значения InventorySlot
                NullifySlotData();
                
            }
            Quickslotinventory.CheckItemInHand();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent == null)
        {
            return;
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>() != null)
        {
            //Перемещаем данные из одного слота в другой
            ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>());
            Quickslotinventory.CheckItemInHand();
        }
        if (oldSlot.amount <= 0)
            NullifySlotData();
    }
    public void NullifySlotData()
    {
        // убираем значения InventorySlot
        oldSlot.item = null;
        oldSlot.amount = 0;
        oldSlot.isEmpty = true;
        oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        oldSlot.iconGO.GetComponent<Image>().sprite = null;
        oldSlot.itemAmount.text = "";
    }
    void ExchangeSlotData(Slot newSlot)
    {
        // Временно храним данные newSlot в отдельных переменных
        ItemScriptableObject item = newSlot.item;
        int amount = newSlot.amount;
        bool isEmpty = newSlot.isEmpty;
        GameObject iconGO = newSlot.iconGO;
        TMP_Text itemAmountText = newSlot.itemAmount;

        if (item == null)
        {
            if (oldSlot.item.MaximumAmount > 1 && oldSlot.amount > 1)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    
                    newSlot.item = oldSlot.item;
                    newSlot.amount = Mathf.CeilToInt((float)oldSlot.amount / 2);
                    newSlot.isEmpty = false;
                    newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
                    newSlot.itemAmount.text = newSlot.amount.ToString();

                    oldSlot.amount = Mathf.FloorToInt((float)oldSlot.amount / 2);
                    oldSlot.itemAmount.text = oldSlot.amount.ToString();
                    return;
                }
                else if (Input.GetKey(KeyCode.LeftControl) )//&& oldSlot.amount > 1)
                {
                    newSlot.item = oldSlot.item;
                    newSlot.amount = 1;
                    newSlot.isEmpty = false;
                    newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
                    newSlot.itemAmount.text = newSlot.amount.ToString();

                    oldSlot.amount--;
                    oldSlot.itemAmount.text = oldSlot.amount.ToString();
                    return;

                }
            }
        }

        if (newSlot.item != null)
        {

            if (oldSlot.item.name.Equals(newSlot.item.name))
            {
                if (Input.GetKey(KeyCode.LeftShift) && oldSlot.amount > 1)
                {
                    if (Mathf.CeilToInt((float)oldSlot.amount / 2) < newSlot.item.MaximumAmount - newSlot.amount)
                    {
                        newSlot.amount += Mathf.CeilToInt((float)oldSlot.amount / 2);
                        newSlot.itemAmount.text = newSlot.amount.ToString();

                        oldSlot.amount -= Mathf.CeilToInt((float)oldSlot.amount / 2);
                        oldSlot.itemAmount.text = oldSlot.amount.ToString();
                    }
                    else
                    {
                        int difference = newSlot.item.MaximumAmount - newSlot.amount;
                        newSlot.amount = newSlot.item.MaximumAmount;
                        newSlot.itemAmount.text = newSlot.amount.ToString();

                        oldSlot.amount -= difference;
                        oldSlot.itemAmount.text = oldSlot.amount.ToString();
                    }
                    return;
                }
                else if (Input.GetKey(KeyCode.LeftControl) && oldSlot.amount > 1)
                {
                    if (newSlot.item.MaximumAmount != newSlot.amount)
                    {
                        newSlot.amount++;
                        newSlot.itemAmount.text = newSlot.amount.ToString();

                        oldSlot.amount--;
                        oldSlot.itemAmount.text = oldSlot.amount.ToString();

                    }
                    return;
                }
                else
                {
                    if (newSlot.amount + oldSlot.amount >= newSlot.item.MaximumAmount)
                    {
                        int difference = newSlot.item.MaximumAmount - newSlot.amount;
                        newSlot.amount = newSlot.item.MaximumAmount;
                        newSlot.itemAmount.text = newSlot.amount.ToString();

                        oldSlot.amount -= difference;
                        oldSlot.itemAmount.text = oldSlot.amount.ToString();
                    }
                    else
                    {
                        newSlot.amount += oldSlot.amount;
                        newSlot.itemAmount.text = newSlot.amount.ToString();
                        NullifySlotData();
                    }
                    return;
                }
            }
        }

        // Заменяем значения newSlot на значения oldSlot
        newSlot.item = oldSlot.item;
        newSlot.amount = oldSlot.amount;
        if (oldSlot.isEmpty == false)
        {
            newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
            if (oldSlot.item.MaximumAmount != 1)
                newSlot.itemAmount.text = oldSlot.amount.ToString();
            else
                newSlot.itemAmount.text = "";
        }
        else
        {
            newSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            newSlot.iconGO.GetComponent<Image>().sprite = null;
            newSlot.itemAmount.text = "";
        }

        newSlot.isEmpty = oldSlot.isEmpty;

        // Заменяем значения oldSlot на значения newSlot сохраненные в переменных
        oldSlot.item = item;
        oldSlot.amount = amount;
        if (isEmpty == false)
        {
            oldSlot.SetIcon(item.icon);
            if (item.MaximumAmount != 1)
            {
                oldSlot.itemAmount.text = amount.ToString();
            }
            else
            {
                oldSlot.itemAmount.text = "";
            }
        }
        else
        {
            oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            oldSlot.iconGO.GetComponent<Image>().sprite = null;
            oldSlot.itemAmount.text = "";
        }

        oldSlot.isEmpty = isEmpty;
    }
}

