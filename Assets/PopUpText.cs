using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PopUpText : MonoBehaviour
{
    public TextMeshProUGUI body;
    public int maxLines = 6; // �ִ� ǥ���� �� ��
    public static Action<string> DisplayText;
    private Queue<string> messageQueue = new Queue<string>(); // ���ڿ� ť
    public float displayDuration = 4f; // �ؽ�Ʈ�� ȭ�鿡 ǥ�õ� �Ⱓ
    private void Start()
    {
        DisplayText = AddMessage;
        body.text = string.Empty;
    }
    // ���ڿ��� ť�� �߰��ϰ� �ؽ�Ʈ ������Ʈ�� ȣ��
    public void AddMessage(string message)
    {
        messageQueue.Enqueue(message);
        UpdateText();
        StartCoroutine(RemoveMessageAfterDelay());
    }

    // �ؽ�Ʈ ������Ʈ
    private void UpdateText()
    {
        // �ִ� ǥ���� �� ���� �Ѿ�� ���� ������ �޽����� ����
        while (messageQueue.Count > maxLines)
        {
            messageQueue.Dequeue();
        }

        // �ؽ�Ʈ�� ������Ʈ
        body.text = string.Join("\n", messageQueue.ToArray());
    }

    // ���� �ð��� ���� �� �޽����� �ϳ��� ����
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