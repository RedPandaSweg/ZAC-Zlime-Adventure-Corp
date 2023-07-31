using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public string statName;
    public int ID { get; private set; }
    public float CurrentValue { get; set; } // Value decreased by taking damage
    public float BaseValue { get; private set; } // Value increased with training
    public float MaxValue { get; private set; } // Maximum reachable value


    public Stat(int id, float baseValue, float maximumValue)
    {
        ID = id;
        BaseValue = baseValue;
        CurrentValue = baseValue; // Start with the standard value
        MaxValue = maximumValue;

        //Name Stat
        switch (ID)
        {
            case 0:
                statName = "Strength";
                break;
            case 1:
                statName = "Dexterity";
                break;
            case 2:
                statName = "Constitution";
                break;
            case 3:
                statName = "Intelligence";
                break;
            case 4:
                statName = "Wisdom";
                break;
            case 5:
                statName = "Charisma";
                break;

        }
    }

    public void IncreaseValue(float amount)
    {
        BaseValue += amount;
        CurrentValue += amount;
        // You can add additional logic here if needed
    }

    public void DecreaseValue(float amount)
    {
        CurrentValue -= amount;
        // You can add additional logic here if needed, such as handling minimum value
    }

    public void UpdateMaxValue(float newMaxValue)
    {
        MaxValue = newMaxValue;
        // You can add additional logic here if needed, such as adjusting CurrentValue if it exceeds the new MaxValue
    }
}
