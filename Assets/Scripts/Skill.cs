using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    //[SerializeField]
    public Image Icon;
    //[SerializeField]
    public TextMeshProUGUI Title;
    //[SerializeField]
    public TextMeshProUGUI Description;
    void Awake()
    {
        Image[] images = GetComponentsInChildren<Image>();
        if (images.Length >= 2)
            Icon = images[1];

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        if (texts.Length >= 2)
        {
            Title = texts[0];
            Description = texts[1];
        }
    }
    public void UpdateUI(SkillData tempSkill)
    {
        Icon.sprite = tempSkill.skilIcon;
        Title.text = tempSkill.skilName;
        Description.text = tempSkill.skilDescription;
    }
}
