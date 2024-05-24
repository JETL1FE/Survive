using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Grid TileGrid;

    [Header("Stage Time Amount")]
    public int PreStageTime = 60;
    public int InPlayTime = 60;
    [Header("Stage Time Amount")]
    [SerializeField]
    private int CurrentGold;
    [Header("Item Prefabs")]
    public GameObject[] Items;

    public Slider expSlider;

    private Player Player;
    public int currentLevel;
    public int currentExp;
    public int[] expCurve = { 30, 60, 90, 120, 150 };
    public SkillManager LevelUpPanel;

    public GameObject GameOverPanel;
    public GameObject GoalOverPanel;
    public Transform AllyHpCanvas;
    public GameObject AllyHpSlider;
    public bool Invade;

    public GameObject HealEffect;
    public GameObject PopEffect;
    public GameObject MakeEffect;
    public GameObject boomEffect;

   // public GradePicker picker;
    public int daysgone = 0;
    public bool StartLine = false;
    public WaveSystem wave;

    public Timer timer;
    public GameObject WorldTimeSystem;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Player = FindAnyObjectByType<Player>();
        if(Player)
        {

        }
        //picker = GetComponent<GradePicker>();

    }
    private int MonsterCount = 0;

    private void Start()
    {
        LevelUpPanel.gameObject.SetActive(false);
        timer.SetDuration(PreStageTime).Begin();
        timer.OnTimerEnd += OnPrestageEnd;

    }
    private void OnPrestageEnd()
    {
        //isPrestageOver = true;
        timer.SetDuration(InPlayTime).Begin(); // goaltime 타이머로 전환합니다.
        timer.OnTimerEnd -= OnPrestageEnd; // prestage 타이머 종료 이벤트 구독을 해제합니다.
        timer.OnTimerEnd += OnGoalEnd; // goaltime 타이머 종료 이벤트에 OnGoalEnd 메서드를 구독합니다.
        Debug.Log("PreStage 시간 종료");
    }
    private void OnGoalEnd()
    {
        GoalOverPanel.SetActive(true);
        Debug.Log("게임 종료 시간");
    }
    public void GetExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= expCurve[currentLevel])
        {
            currentLevel++;
            currentExp = 0;
            LevelUpPanel.gameObject.SetActive(true);
        }
        expSlider.maxValue = expCurve[currentLevel];
        expSlider.value = currentExp;
    }
    public void CheckExp()
    {
        //exp += amount;
        if (currentExp >= expCurve[currentLevel])
        {
            currentLevel++;
            currentExp = 0;
            //levelPanel.Show();
        }
    }

    void Update()
    {
        //PauseCheck();
        if (Input.GetKeyDown(KeyCode.G))
        {
            Invade = !Invade;
        }

        if(Player.Dead)
        {
            Time.timeScale = 0.3f;
            GameOverPanel.SetActive(true);
        }

        expSlider.value = currentExp;
        expSlider.maxValue = expCurve[currentLevel];
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetExp(10);
        }

        if (LevelUpPanel.gameObject.activeSelf || GoalOverPanel.activeSelf)
        {
            Time.timeScale = 0.0f;
        }

        //PreStage();
    }
    //public void PreStage()
    //{
    //    if (!StartLine)
    //    {
    //        if(Builder.Instance.barrackLimit > 0)
    //        {
    //            WorldTimeSystem.SetActive(true);
    //            StartLine = true;
    //            wave.StartWave();
    //        }
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}
    //public void PauseCheck()
    //{
    //    if(timer)
    //    {
    //        if(timer.isPause)
    //        {
    //            Time.timeScale = 0f;
    //        }
    //        else
    //        {
    //            Time.timeScale = 1f;
    //        }
    //    }
    //}
    public void TimeToAttack()
    {
        Invade = true;
    }
    public void TimeToEscape()
    {
        Invade = false;
    }
    public void SpendGold(int amount)
    {
        CurrentGold -= amount;
    }
    public void GainGold(int amount)
    {
        CurrentGold += amount;
    }
    public int CurrentGoldGetter()
    {
        return CurrentGold;
    }
    public Grid GridGetter()
    {
        return TileGrid;
    }
    public Player PlayerGetter()
    {
        return Player;
    }
    public void GainExp(int amount)
    {
        currentExp += amount;
    }
    public void MonsterSpawnCount(int amount)
    {
        MonsterCount += amount;
    }
    public int MonsterCountGetter()
    {
        return MonsterCount; 
    }
    public void ReStart()
    {
        // 현재 씬의 인덱스를 가져옵니다.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // 현재 씬을 다시 로드하여 재시작합니다.
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void DayGoes()
    {
        daysgone++;
        Debug.Log($"Day :: {daysgone}");
    }
}
