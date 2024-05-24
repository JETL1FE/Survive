using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skil", menuName = "Scriptable Object/SkillData")]
public class SkillData : ScriptableObject
{
    //public enum SkilType { StatusBonus, Heal }
    [Header("Main Info")]
    public Sprite skilIcon;
    public string skilName;
    public string skilDescription;
}
