using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public List<SkillData> skillList = new List<SkillData>();
    public Skill[] Slots;
    public HashSet<int> usedIndexes = new HashSet<int>();

    private float clickIgnoreDuration = 0.8f; // 클릭 무시할 시간

    void Awake()
    {
        SkillData[] skillDataArray = Resources.LoadAll<SkillData>("SkillData");
        Slots = GetComponentsInChildren<Skill>();
        skillList.AddRange(skillDataArray);
        //gameObject.SetActive(false);
    }
    //private void OnEnable()
    //{
    //    StartCoroutine(IgnoreClickForDuration());
    //    for (int i = 0; i < Slots.Length; i++)
    //    {
    //        int randomIndex = Random.Range(0, skillList.Count);
    //        Slots[i].UpdateUI(skillList[randomIndex]);
    //        Slots[i].GetComponent<Button>().interactable = false;
    //    }
    //}
    private void OnEnable()
    {
        StartCoroutine(IgnoreClickForDuration());

        // 사용된 인덱스를 추적할 HashSet을 초기화합니다.
        HashSet<int> usedIndexes = new HashSet<int>();

        for (int i = 0; i < Slots.Length; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, skillList.Count);
            } while (usedIndexes.Contains(randomIndex)); // 중복된 인덱스인 경우 다시 선택합니다.

            usedIndexes.Add(randomIndex); // 사용된 인덱스를 HashSet에 추가합니다.

            Slots[i].UpdateUI(skillList[randomIndex]);
            Slots[i].GetComponent<Button>().interactable = false;
        }
    }
    private IEnumerator IgnoreClickForDuration()
    {
        yield return new WaitForSecondsRealtime(clickIgnoreDuration);
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].GetComponent<Button>().interactable = true;
        }
    }
    public void SetOff()
    {
        gameObject.SetActive(false);
    }
}
