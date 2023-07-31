using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopUp : MonoBehaviour
{
    public TextMeshProUGUI nameText, description, buffs;

    public static ItemPopUp Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Error: There are multiple descriptions present, deleting old one");
            Destroy(Instance);
            Instance = this;
        }
    }

    private void OnDisable()
    {
        Instance = null;
    }

    public void SetPopUpText(string _name, string _description, string _buffs)
    {
        nameText.text = _name;
        description.text = _description;
        buffs.text = _buffs;
    }
}
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class ItemPopUp : MonoBehaviour
//{
//    public TextMeshProUGUI nameText;
//    public TextMeshProUGUI attributeText;
//    public TextMeshProUGUI descriptionText;

//    public void SetPopUpText(InventorySlot slot)
//    {
//        var obj = slot.ItemObject;
//        nameText.text = obj.data.Name;
//        string attributeList = "";
//        for (int i = 0; i < slot.item.buffs.Length; i++)
//        {
//            if (attributeList != "") attributeList = string.Concat(attributeList, "\n");
//            if (slot.item.buffs[i].value != 0) attributeList = string.Concat(attributeList, slot.item.buffs[i].attribute, ": +", slot.item.buffs[i].value);
//        }
//        attributeText.text = attributeList;
//        descriptionText.text = obj.description;
//        //LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());

//        //Debug.Log(attributeList);
//    }
//}
