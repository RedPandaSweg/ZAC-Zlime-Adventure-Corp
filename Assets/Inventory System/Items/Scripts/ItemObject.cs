using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Helmet,
    Weapon,
    Twohanded,
    Shield,
    Chest,
    Default
}

public enum Attributes
{
    Strength,
    Dexterity,
    Constituion,
    Intelligence,
    Wisdom,
    Charisma,
    Armor
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
public class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public bool stackable;
    public bool isTwoHanded;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    public Item data = new Item();

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

}