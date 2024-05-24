using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIButton
{
    public Button button;
    public TextMeshProUGUI elementText;

    public UIButton(Button button, TextMeshProUGUI elementText)
    {
        this.button = button;
        this.elementText = elementText;
    }
}

public class OnClickPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public List<UIButton> uiButton = new List<UIButton>();

    public void UpdateUIElement(int index, string newText)
    {
        if (index >= 0 && index < uiButton.Count)
        {
            uiButton[index].elementText.text = newText;
        }
    }
    public void Setter(int index, bool isActive)
    {
        if (index >= 0 && index < uiButton.Count)
        {
            uiButton[index].button.gameObject.SetActive(isActive);
        }
    }
}
