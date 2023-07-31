using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public Fusion fusion;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button testButton = root.Q<Button>("TestButton");

        testButton.clicked += () => fusion.FusionHa();
    }
}
