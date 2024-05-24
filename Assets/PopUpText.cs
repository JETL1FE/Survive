using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PopUpText : MonoBehaviour
{
    public TextMeshProUGUI body;
    public int maxLines = 6; // 최대 표시할 행 수
    public static Action<string> DisplayText;
    private Queue<string> messageQueue = new Queue<string>(); // 문자열 큐
    public float displayDuration = 4f; // 텍스트가 화면에 표시될 기간
    private void Start()
    {
        DisplayText = AddMessage;
        body.text = string.Empty;
    }
    // 문자열을 큐에 추가하고 텍스트 업데이트를 호출
    public void AddMessage(string message)
    {
        messageQueue.Enqueue(message);
        UpdateText();
        StartCoroutine(RemoveMessageAfterDelay());
    }

    // 텍스트 업데이트
    private void UpdateText()
    {
        // 최대 표시할 행 수를 넘어가면 가장 오래된 메시지를 제거
        while (messageQueue.Count > maxLines)
        {
            messageQueue.Dequeue();
        }

        // 텍스트를 업데이트
        body.text = string.Join("\n", messageQueue.ToArray());
    }

    // 일정 시간이 지난 후 메시지를 하나씩 제거
    private IEnumerator RemoveMessageAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        if (messageQueue.Count > 0)
        {
            messageQueue.Dequeue();
            UpdateText();
        }
    }
}