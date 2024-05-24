using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

//public class GradePicker : MonoBehaviour
//{
//    private int[] grades = { 0, 1, 2, 3 };

//    public int PickGrade()
//    {
//        float totalProbability = 0f;
//        foreach (float probability in probabilities)
//        {
//            totalProbability += probability;
//        }

//        float randomValue = UnityEngine.Random.Range(0f, totalProbability);
//        float cumulativeProbability = 0f;
//        for (int i = 0; i < probabilities.Length; i++)
//        {
//            cumulativeProbability += probabilities[i];
//            if (randomValue <= cumulativeProbability)
//            {
//                Debug.Log($"{grades[i]} Grade Pick");
//                return grades[i];
//            }
//        }
//        return grades[0];
//    }
//}
public class Barracks : WallData
{
    public Companion[] Companions;
    private int SpawnCost = 50;
    private int grades;
    [SerializeField]
    [Header("Probablities")]
    public int[] weights = { 50, 30, 15, 5 };
    public float spawnRadius = 2f;
    public int maxAttempts = 10;
    public int maxCompanionsAllowed = 10;
    private void Start()
    {
        if (Companions != null)
        {
            grades = Companions[0].GradeLimitInt - 1; //3
        }
    }
    private int GetRandomGrade()
    {
        int totalWeight = weights.Sum();
        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                Debug.Log("Random" + i);
                return i;
            }
        }

        return 0; // �⺻�� (Ȯ���� �߸� ���� ���)
    }
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.B))
    //    {
    //        GetRandomGrade();
    //    }
    //}
    public override void OnEnable()
    {
        base.OnEnable();
        Builder.Instance.barrackLimit++;
    }

    //public Companion PickGrade()
    //{
    //    float totalProbability = 0f;
    //    foreach (float probability in probabilities)
    //    {
    //        totalProbability += probability;
    //    }

    //    float randomValue = UnityEngine.Random.Range(0f, totalProbability);
    //    float cumulativeProbability = 0f;
    //    for (int i = 0; i < probabilities.Length; i++)
    //    {
    //        cumulativeProbability += probabilities[i];
    //        if (randomValue <= cumulativeProbability)
    //        {
    //            Debug.Log($"{Companions[i]} Grade Pick");

    //            return Companions[i];
    //        }
    //    }
    //    Debug.LogWarning("Failed to pick");
    //    return Companions[0];
    //}

    void SpawnAllyHpSlider(Companion _ally)
    {
        GameObject sliderClone = Instantiate(GameManager.Instance.AllyHpSlider);
        sliderClone.transform.SetParent(GameManager.Instance.AllyHpCanvas);
        sliderClone.transform.localScale = Vector3.one;
        sliderClone.GetComponent<SliderPositionAutoSetter>().SetUp(_ally.transform);
        sliderClone.GetComponent<HpViewer>().SetUp(_ally.GetComponent<LivingEntity>());
    }
    private void OnDestroy()
    {
        Builder.Instance.barrackLimit--;
    }
    public void SpawnCompanion()
    {
        if (Companions.Length > 0)
        {
            if (Builder.Instance.curpopulation < Builder.Instance.populationLimit)
            {
                if (SpawnCost < GameManager.Instance.CurrentGoldGetter())
                {
                    int maxAttempts = 3; // �ִ� �õ� Ƚ�� ����
                    for (int i = 0; i < maxAttempts; i++)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, Companions.Length);
                        Vector3 spawnPosition = transform.position + UnityEngine.Random.insideUnitSphere; // �ݰ� 2�� ���ο� ���� ��ġ ����

                        bool overlap = Physics.CheckSphere(spawnPosition, 0.5f); // �ݰ� 0.5 ���� �ٸ� collider�� �ִ��� Ȯ��

                        if (!overlap)
                        {
                            animator.SetTrigger("Brr");
                            //Companion clone = Instantiate(Companions[randomIndex], spawnPosition, Quaternion.identity);                        
                            Companion clone = Instantiate(Companions[randomIndex], spawnPosition, Quaternion.identity);
                            clone.GrandeSetter(GetRandomGrade());
                            //clone.GrandeSetter(picker.PickGrade());
                            SpawnAllyHpSlider(clone);
                            Builder.Instance.companions.Add(clone);
                            GameManager.Instance.SpendGold(SpawnCost);
                            PopUpText.DisplayText($"{clone.name} {clone.grade}����� �̾Ҵ�.");
                            Instantiate(GameManager.Instance.PopEffect, clone.transform.position, Quaternion.identity);
                            break;
                        }
                        else
                        {
                            PopUpText.DisplayText($"������ �����Ǳ⿡ ������ �����ϴ�");
                        }
                    }
                }
                else
                {
                    Debug.Log("�ڽ�Ʈ�� ������");
                }
            }
            else
            {
                PopUpText.DisplayText($"���� ���� ���� Gold Mine �ʿ�");
            }

        }
    }
    public int SpawnCostGetter()
    {
        return SpawnCost;
    }
}

