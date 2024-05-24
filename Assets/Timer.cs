using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Timer UI :")]
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private Image fillimage;
    public bool isPause = false;
    public int Duration { get; private set; }
    private int remainingDuration;
    public event Action OnTimerEnd;
    private void Awake()
    {
        ResetTimer();
    }
    void ResetTimer()
    {
        timeText.text = "0:00";
        fillimage.fillAmount = 0f;
        Duration = remainingDuration = 0;
    }
    private void Update()
    {
        if(isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public Timer SetDuration(int sesconds)
    {
        Duration = remainingDuration = sesconds;
        return this;
    }
    public void Begin()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }
    public IEnumerator UpdateTimer()
    {
        while (remainingDuration > 0)
        {
            UpdateUI(remainingDuration);
            {
                remainingDuration--;
                yield return new WaitForSeconds(1f);
            }
        }
        End();
    }
    private void UpdateUI(int seconds)
    {
        timeText.text = string.Format("{0:D2}:{1:D2}", seconds / 60, seconds % 60);
        fillimage.fillAmount = Mathf.InverseLerp(0, Duration, seconds);
    }
    public void End()
    {
        ResetTimer();
        OnTimerEnd?.Invoke(); // 타이머 종료 이벤트 호출
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    public void PauseBtn()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        Debug.Log($"Pause :: {isPause}");
    }
}
