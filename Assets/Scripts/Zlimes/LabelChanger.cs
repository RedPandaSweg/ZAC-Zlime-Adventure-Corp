using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class LabelChanger : MonoBehaviour
{
    public UIDocument uiDocument; // Reference to the UIDocument containing the XML file

    private void Start()
    {
        //uiDocument = GameObject.Find("UIDocument").GetComponent<UIDocument>();
    }

    private void OnMouseDown()
    {
        Zlime zlime = this.GetComponent<Zlime>();
        ZlimePanel zlimePanel = zlime.zlimePanel;
        string format = "0";
        //ChangeLabel("StrText", zlime.statsCurrent[0].ToString(format));
        //ChangeLabel("DexText", zlime.statsCurrent[1].ToString(format));
        //ChangeLabel("ConText", zlime.statsCurrent[2].ToString(format));
        //ChangeLabel("IntText", zlime.statsCurrent[3].ToString(format));
        //ChangeLabel("WisText", zlime.statsCurrent[4].ToString(format));
        //ChangeLabel("ChaText", zlime.statsCurrent[5].ToString(format));
        //ChangeLabel("StrMaxText", zlime.statsMax[0].ToString(format));
        //ChangeLabel("DexMaxText", zlime.statsMax[1].ToString(format));
        //ChangeLabel("ConMaxText", zlime.statsMax[2].ToString(format));
        //ChangeLabel("IntMaxText", zlime.statsMax[3].ToString(format));
        //ChangeLabel("WisMaxText", zlime.statsMax[4].ToString(format));
        //ChangeLabel("ChaMaxText", zlime.statsMax[5].ToString(format));

        //ChangeText(zlimePanel.nameText, this.gameObject.name);
        ChangeText(zlimePanel.massText, zlime.mass.ToString(format) + " / " + zlime.massMax.ToString(format) + " m");
        ChangeText(zlimePanel.mojoText, zlime.mojo.ToString(format) + " / " + zlime.mojoMax.ToString(format) + " M");
        ChangeText(zlimePanel.nameText, this.name);
        ChangeText(zlimePanel.growthText, zlime.growthPotential.ToString("0.0") + " G");
        ChangeText(zlimePanel.armorText, zlime.armor.ToString(format) + " A");

        for (int i = 0; i < zlimePanel.statTexts.Count; i++)
        {
            ChangeText(zlimePanel.statTexts[i], zlime.statsCurrent[i].ToString(format) + " / " + zlime.statsMax[i].ToString(format));
        }


        Debug.Log("Labels changed!");
    }

    private void ChangeText(TextMeshProUGUI label, string text)
    {
        label.text = text;
    }

    //private void ChangeLabel(string label, string text)
    //{
    //    Label labelElement = uiDocument.rootVisualElement.Q<Label>(label); // Find the label element by its name
    //    if (labelElement != null)
    //    {
    //        labelElement.text = text; // Change the text of the label
    //    }
    //}
}