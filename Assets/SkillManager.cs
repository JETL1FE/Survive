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

    private float clickIgnoreDuration = 0.8f; // Ŭ�� ������ �ð�

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

        // ���� �ε����� ������ HashSet�� �ʱ�ȭ�մϴ�.
        HashSet<int> usedIndexes = new HashSet<int>();

        for (int i = 0; i < Slots.Length; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, skillList.Count);
            } while (usedIndexes.Contains(randomIndex)); // �ߺ��� �ε����� ��� �ٽ� �����մϴ�.

            usedIndexes.Add(randomIndex); // ���� �ε����� HashSet�� �߰��մϴ�.

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
