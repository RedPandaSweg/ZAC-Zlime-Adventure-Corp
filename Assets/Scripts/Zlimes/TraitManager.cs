using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    [SerializeField] private TraitData[] traits;

    // Other properties and methods for the TraitManager

    public void ApplyTraits(Zlime monster)
    {
        foreach (TraitData traitData in traits)
        {
            ApplyTraitEffect(monster, traitData);
        }
    }

    private void ApplyTraitEffect(Zlime monster, TraitData traitData)
    {
        switch (traitData.traitType)
        {
            case TraitData.TraitType.Armor:
                monster.armor += traitData.armorBonus;
                break;
            case TraitData.TraitType.Skill:
                monster.AddSkill(traitData.skillName);
                break;
                // Add more cases for additional trait types if needed
        }
    }
}