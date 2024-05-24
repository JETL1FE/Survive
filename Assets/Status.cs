using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class UIElement
{
    public GameObject element;
    public TextMeshProUGUI elementText;

    public UIElement(GameObject element, TextMeshProUGUI elementText)
    {
        this.element = element;
        this.elementText = elementText;
    }
}


public class Status : MonoBehaviour
{
    public List<UIElement> uiElements = new List<UIElement>();

    public void UpdateUIElement(int index, string newText)
    {
        if (index >= 0 && index < uiElements.Count)
        {
            uiElements[index].elementText.text = newText;
        }
    }
}
