using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Chest
}

[CreateAssetMenu(fileName = "new Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public InterfaceType type;
    public Inventory Container;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }

    public bool AddItem(Item _item, int _amount)
    {
        //Check if a stack already exists.
        InventorySlot slot = FindItemOnInventory(_item);
        if (slot != null && database.ItemObjects[_item.Id].stackable)
        {
            slot.AddAmount(_amount);
            return true;
        }
        if (EmptySlotCount > 0)
        {
            //Place in first empty slot.
            SetEmptySlot(_item, _amount);
            return true;
        }
        else
        {
            //No way to add item.
            return false;
        }
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1) counter++;
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item.Id == _item.Id) { return GetSlots[i]; }
        }
        return null;
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }
        //setup for full inventory!?
        return null;
    }

    public void SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        if (slot2.CanPlaceInSlot(slot1.ItemObject) && slot1.CanPlaceInSlot(slot2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(slot2.item, slot2.amount);
            slot2.UpdateSlot(slot1.item, slot1.amount);
            slot1.UpdateSlot(temp.item, temp.amount);
            //Debug.Log(slot1.item.Name + " swapped with " + slot2.item.Name);
        }
    }

    public void SwapTwoHandedItems(InventorySlot draggedSlot, InventorySlot targetSlot, InventorySlot otherSlot)
    {
        if (targetSlot.CanPlaceInSlot(draggedSlot.ItemObject) && draggedSlot.CanPlaceInSlot(targetSlot.ItemObject))
        {
            //Debug.Log(slot1.item.Name + " swapped with " + slot2.item.Name);
            if (otherSlot.ItemObject != null)
            {
                Debug.Log(otherSlot.ItemObject.type);
                if (otherSlot.ItemObject.isTwoHanded)
                {
                    if (draggedSlot.ItemObject.isTwoHanded && MouseData.interfaceMouseIsOver.inventory.type == InterfaceType.Equipment) otherSlot.UpdateSlot(draggedSlot.item, draggedSlot.amount);
                    //else if (targetSlot.ItemObject.isTwoHanded) otherSlot.RemoveItem();
                    else otherSlot.RemoveItem(); //dragged item is onehanded
                }
                else
                {
                    AddItem(otherSlot.item, otherSlot.amount); //other item gets back into inventory
                    otherSlot.UpdateSlot(draggedSlot.item, draggedSlot.amount); // otherslot gets twohanded duplicate
                }
            }
            else otherSlot.UpdateSlot(draggedSlot.item, draggedSlot.amount); //otherslot was empty so it cant be two handed

            InventorySlot temp = new InventorySlot(targetSlot.item, targetSlot.amount);
            targetSlot.UpdateSlot(draggedSlot.item, draggedSlot.amount);
            draggedSlot.UpdateSlot(temp.item, temp.amount);

        }
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item == _item)
            {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        ////Json save
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //bf.Serialize(file, saveData);
        //file.Close();

        //No edit save
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            ////Json Load
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            //file.Close();

            //No Edit Load
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }

}