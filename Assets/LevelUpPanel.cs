using System.Collections.Generic;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    public Skill[] Slots;
   // public SkillData[] Datas;
    public SkillManager Manager;


    //public void Hide()
    //{
    //    gameObject.SetActive(false);
    //}
    //private void OnEnable()
    //{
    //    if (SkillSlots != null)
    //        AssignRandomSkills();
    //}

    //public void Show()
    //{
    //    gameObject.SetActive(true);
    //    if (SkillSlots != null)
    //        AssignRandomSkills();
    //}
    //private void AssignRandomSkills()
    //{
    //    HashSet<int> usedIndexes = new HashSet<int>();
    //    for (int i = 0; i < SkillSlots.Length; i++)
    //    {
    //        int randomIndex;
    //        do
    //        {
    //            randomIndex = Random.Range(0, Datas.Length);
    //        } while (usedIndexes.Contains(randomIndex));
    //        usedIndexes.Add(randomIndex);
    //        SkillSlots[i].UpdateUI(Datas[randomIndex]);
    //    }
    //}
}