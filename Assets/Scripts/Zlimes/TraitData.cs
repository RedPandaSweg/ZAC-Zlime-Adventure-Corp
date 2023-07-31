using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrait", menuName = "Traits/Trait")]
public class TraitData : ScriptableObject
{
    public enum TraitType
    {
        Armor,
        Skill,
        // Add more trait types here if needed
    }

    public TraitType traitType;
    public float armorBonus;
    public string skillName;
}