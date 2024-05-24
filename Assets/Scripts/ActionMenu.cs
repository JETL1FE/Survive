using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    public static Action<bool> Active;
    public static Func<bool> IsActive;
    [SerializeField]
    private Button buttonPrefab;
    [SerializeField]
    private Button[] buttons;
    private GridLayoutGroup grid;
    private void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        buttons = GetComponentsInChildren<Button>();        
        if(buttonPrefab != null)
        {
            buttons = new Button[Builder.Instance.wallPrefabs.Length];
            for(int i = 0; i < Builder.Instance.wallPrefabs.Length; i++)
            {                
                buttons[i] = Instantiate(buttonPrefab, transform.position, Quaternion.identity, transform);
                buttons[i].name = Builder.Instance.wallPrefabs[i].name;
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = 
                    Builder.Instance.wallPrefabs[i].name + $"\n{Builder.Instance.wallPrefabs[i].GetComponent<WallData>().CostGetter()} Gold";
                // 버튼에 클릭 이벤트 추가
                int index = i; // 람다식에서 사용할 로컬 변수로 index를 선언
                buttons[i].onClick.AddListener(() => Builder.Instance.Selecter(index));
                buttons[i].onClick.AddListener(() => ButtonToExit());
            }
            if (buttons.Length > 5)
            {
                grid.constraintCount = 3;
            }
        }

    }
    private void Update()
    {
        BarracksChecker();
    }

    private void BarracksChecker()
    {
        if (Builder.Instance.barrackLimit != 0)
        {
            buttons[1].interactable = false;
        }
        else
        {
            buttons[1].interactable = true;
        }
    }

    private void OnEnable()
    {
        Active = OnUIActivate;
        IsActive = ActivateGetter;        
    }
    public void ButtonToExit()
    {
        gameObject.SetActive(false);
    }
    private void OnUIActivate(bool activate)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    private bool ActivateGetter()    
    {
        return gameObject.activeSelf;
    }
}
