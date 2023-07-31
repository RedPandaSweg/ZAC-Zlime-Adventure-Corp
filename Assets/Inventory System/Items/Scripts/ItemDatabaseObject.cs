using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item Database", menuName = "Inventory System/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemObjects;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i<ItemObjects.Length; i++)
        {
            if (ItemObjects[i].data.Id != i) ItemObjects[i].data.Id = i;            
        }
    }
    public void UpdateName()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].data.Name = ItemObjects[i].name;
            Debug.Log(ItemObjects[i].name);
        }
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
        //UpdateName();
    }

    public void OnBeforeSerialize()
    {
    }
}
