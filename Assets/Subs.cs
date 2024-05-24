using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Subs : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI enemysCountText;
    public TextMeshProUGUI goalTime;
    public TextMeshProUGUI populationcount;
    // Update is called once per frame
    void Update()
    {
        goldText.text = $"{GameManager.Instance.CurrentGoldGetter()}G";
        enemysCountText.text = $"{GameManager.Instance.MonsterCountGetter()}";

        populationcount.text = $"{Builder.Instance.curpopulation}/{Builder.Instance.populationLimit}";
        if(GameManager.Instance.StartLine)
        {
            goalTime.text = $"{GameManager.Instance.InPlayTime.ToString("F0")}";
        }
        else
        {
            //goalTime.text = $"PreStage Count\n{GameManager.Instance.PreStageCount} / {Builder.Instance.builtObjects.Count}";
        }
    }
}
