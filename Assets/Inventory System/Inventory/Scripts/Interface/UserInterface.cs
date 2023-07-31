using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Globalization;
using System.Linq;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    public Transform parent;
    static GameObject popUpObject;
    public GameObject popUpPrefab;
    private TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void OnSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.Id >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.uiDisplay;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    void Update()
    {
        //Update PopUp position
        if (popUpObject)
        {
            Vector2 _pos = new Vector2(Input.mousePosition.x + 50, Input.mousePosition.y + 50);
            //if (_pos.x > Screen.width - 50) _pos.x = Input.mousePosition.x - 50;
            //else _pos.x = Input.mousePosition.x + 50;
            //if (_pos.y + 50 > Screen.height) _pos.y = Screen.height - 50;
            //else _pos.y = +50;
            popUpObject.transform.position = _pos;
        }
    }

    void CreatePopUp(InventorySlot hoveringItem)
    {
        Transform _trans;
        if (parent == null)
            _trans = hoveringItem.parent.transform;
        else
            _trans = parent;

        popUpObject = Instantiate(popUpPrefab, Vector2.zero, Quaternion.identity, _trans);
        string _buffs = "";
        for (int i = 0; i < hoveringItem.item.buffs.Length; i++)
        {
            _buffs = string.Concat(_buffs, " ", hoveringItem.item.buffs[i].attribute.ToString(), ": +", hoveringItem.item.buffs[i].value.ToString(), Environment.NewLine);
        }
        _buffs = _buffs.Replace("_", " ");

        var _item = hoveringItem.item;

        popUpObject.GetComponent<ItemPopUp>().SetPopUpText(_item.Name, hoveringItem.ItemObject.description, textInfo.ToTitleCase(_buffs));
    }

    void DestroyPopUp()
    {
        if (popUpObject != null) Destroy(popUpObject);
    }

    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;

        InventorySlot hoveringItem = slotsOnInterface[obj];
        if (hoveringItem.item.Id >= 0)
        {
            CreatePopUp(hoveringItem);
        }
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
        DestroyPopUp();
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnBeginDrag(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if(slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(64, 64);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    public void OnEndDrag(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }

        if (MouseData.slotHoveredOver && MouseData.interfaceMouseIsOver)
        {
            InventorySlot mouseSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];

            if ((slotsOnInterface[obj].parent.inventory.type == InterfaceType.Equipment || mouseSlotData.parent.inventory.type == InterfaceType.Equipment) && (slotsOnInterface[obj].ItemObject?.isTwoHanded == true || mouseSlotData.ItemObject?.isTwoHanded == true))
            {
                inventory.SwapTwoHandedItems(slotsOnInterface[obj], mouseSlotData, FindOtherHandSlot(obj, mouseSlotData));
            }
            else inventory.SwapItems(slotsOnInterface[obj], mouseSlotData);

            DestroyPopUp();
        }
    }

    public InventorySlot FindOtherHandSlot(GameObject obj, InventorySlot mouseSlotData)
    {
        InventorySlot[] weaponSlots = new InventorySlot[0];
        if (slotsOnInterface[obj].parent.inventory.type == InterfaceType.Equipment) weaponSlots = slotsOnInterface[obj].parent.inventory.Container.Slots.Where(slot => slot.allowedItems.Contains(ItemType.Weapon)).ToArray();
        else if (mouseSlotData.parent.inventory.type == InterfaceType.Equipment) weaponSlots = MouseData.interfaceMouseIsOver.inventory.Container.Slots.Where(slot => slot.allowedItems.Contains(ItemType.Weapon)).ToArray();

        if (weaponSlots.Length > 0)
        {
            foreach (InventorySlot slot in weaponSlots)
            {
                if (slot != mouseSlotData && slot != slotsOnInterface[obj]) return slot;
            }
        }
        Debug.Log("Error on FindOtherHandSlot - twohanded changed slot on inventory?");
        return new InventorySlot();
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}

public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            if (_slot.Value.item.Id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}