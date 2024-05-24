using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//[System.Serializable]
//public class ButtonTextPair
//{
//    public Button button;
//    public TextMeshProUGUI textMesh;
//}
//스텟 박스의 원소 순서들은 0 체력, 1 공격력, 2공격속도
public class OnClickMenu : MonoBehaviour
{
    public static Action<GameObject> ArgSetter;
    [SerializeField]
    private GameObject selectedObject;
    public TextMeshProUGUI ObjectName;
    public Image portrait;
    public Image portrait2;
    public OnClickPanel onClickPanel;
    public Status status;
    public CompanionAttackRange RangeDisplayer;
    private Vector3 prevPosition;
    private Image boxImage;
    private Vector2 boxSize;
    void Start()
    {
        ArgSetter = Setter;
        prevPosition = portrait.transform.position;
    }

    public void Update()
    {
        if (selectedObject)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(selectedObject.GetComponent<Companion>())
                {
                    selectedObject.GetComponent<Companion>().MoveTo(Inputs.Instance.mouseWorldPosition);
                }
                else
                {
                    onClickPanel.uiButton[0].button.onClick.Invoke();
                }

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                onClickPanel.uiButton[1].button.onClick.Invoke();
            }
            UpdateUI();
        }
        gameObject.SetActive(selectedObject);
    }
    public void Setter(GameObject _gameObject)
    {
        if (_gameObject != null)
        {
            selectedObject = _gameObject;

            onClickPanel.uiButton[0].button.onClick.RemoveAllListeners();
            onClickPanel.uiButton[1].button.onClick.RemoveAllListeners();

            bool isActive = selectedObject.GetComponent<Companion>() || selectedObject.GetComponent<Item>() || selectedObject.GetComponent<WallData>();
            onClickPanel.Setter(0, isActive);
            onClickPanel.Setter(1, isActive);

            onClickPanel.uiButton[0].elementText.text = $"Q Key\n";
            onClickPanel.uiButton[1].elementText.text = $"E Key\n";
            bool hasWallDataOnly = selectedObject.GetComponent<WallData>() != null &&
                       selectedObject.GetComponent<RepairTower>() == null &&
                       selectedObject.GetComponent<GoldMine>() == null;

            status.uiElements[1].element.SetActive(!(selectedObject.CompareTag("Player") || selectedObject.GetComponent<Barracks>() || hasWallDataOnly));
            status.uiElements[2].element.SetActive(!(selectedObject.CompareTag("Player") || selectedObject.GetComponent<Barracks>() || hasWallDataOnly));
           
            // 초기 UI 설정
            if (selectedObject.CompareTag("Player"))
            {
                LivingEntity temp = selectedObject.GetComponent<LivingEntity>();
                status.UpdateUIElement(0, $"{temp.MaxHealth}\n{temp.Health}");
            }
            else if (selectedObject.CompareTag("Companion"))
            {
                Companion temp = selectedObject.GetComponent<Companion>();
                portrait.sprite = temp.Body.sprite;
                onClickPanel.uiButton[0].elementText.text += "";
                onClickPanel.uiButton[1].elementText.text += $"{(int)temp.grade}/{temp.GradeLimitInt}";
                //onClickPanel.uiButton[0].button.onClick.AddListener(() => temp.MoveTo((Vector2)Inputs.Instance.mouseWorldPosition));
                onClickPanel.uiButton[0].button.onClick.AddListener(() => temp.MoveToMouseClick());
                onClickPanel.uiButton[1].button.onClick.AddListener(() => Builder.Instance.CompanionCombiner(temp));
                status.UpdateUIElement(0, $"{temp.MaxHealth}\n{temp.Health}");
                status.UpdateUIElement(1, $"{temp.damage}");
                status.UpdateUIElement(2, $"{temp.attackRate}\n{temp.attackRange}");
            }
            else if (selectedObject.CompareTag("Enemy"))
            {
                Monster temp = selectedObject.GetComponent<Monster>();
                status.UpdateUIElement(0, $"{temp.MaxHealth}\n{temp.Health}");
                status.UpdateUIElement(1, $"{temp.damage}");
                status.uiElements[2].element.SetActive(!selectedObject.CompareTag("Enemy"));
            }
            else if (selectedObject.CompareTag("Wall"))
            {
                WallData temp = selectedObject.GetComponent<WallData>();
                onClickPanel.uiButton[1].button.onClick.AddListener(() => temp.Sell());
                onClickPanel.uiButton[1].elementText.text += $"Sell";

                status.UpdateUIElement(0, $"{temp.MaxHealth}\n{temp.Health}");

                if (selectedObject.GetComponent<Barracks>())
                {
                    onClickPanel.uiButton[0].elementText.text += $"Spawn {selectedObject.GetComponent<Barracks>().SpawnCostGetter()}G";
                    onClickPanel.uiButton[0].button.onClick.AddListener(() => selectedObject.GetComponent<Barracks>().SpawnCompanion());
                }
                else if (selectedObject.GetComponent<RepairTower>())
                {
                    onClickPanel.uiButton[0].button.onClick.AddListener(() => selectedObject.GetComponent<RepairTower>().Upgrade());
                }
                else if (selectedObject.GetComponent<GoldMine>())
                {
                    status.UpdateUIElement(1, $"{temp.GetComponent<GoldMine>().Amount}");
                    status.UpdateUIElement(2, $"{temp.GetComponent<GoldMine>().maketime}\n{temp.GetComponent<GoldMine>().ReturnCur()}");
                }
                else
                {
                    onClickPanel.uiButton[0].button.onClick.AddListener(() => selectedObject.GetComponent<WallData>().Upgrade());

                }
            }

            this.gameObject.SetActive(true);
        }
        else
        {
            selectedObject = null;
            this.gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        if (selectedObject)
        {
            ObjectName.text = selectedObject.name;
            portrait.sprite = selectedObject.GetComponent<LivingEntity>().Body.sprite;
            portrait.gameObject.SetActive(!selectedObject.CompareTag("Wall"));
            portrait2.gameObject.SetActive(selectedObject.CompareTag("Wall"));
        }
        if (selectedObject.CompareTag("Player"))
        {
            LivingEntity temp = selectedObject.GetComponent<LivingEntity>();
            status.UpdateUIElement(0, $"{temp.MaxHealth}\n{temp.Health}");
        }
        else if (selectedObject.CompareTag("Companion"))
        {
            Companion temp = selectedObject.GetComponent<Companion>();
            onClickPanel.uiButton[1].elementText.text = $"E Key\nCombine\n{(int)temp.grade + 1}/{temp.GradeLimitInt}";
            status.UpdateUIElement(0, $"{temp.MaxHealth}\n{temp.Health}");
            status.UpdateUIElement(1, $"{temp.damage}");
            status.UpdateUIElement(2, $"{temp.attackRate}\n{temp.attackRange}");

        }
        else if (selectedObject.CompareTag("Enemy"))
        {
            Monster temp = selectedObject.GetComponent<Monster>();
            status.UpdateUIElement(0, $"{temp.MaxHealth}\n{temp.Health}");
            status.UpdateUIElement(1, $"{temp.damage}");
        }
        else if (selectedObject.CompareTag("Wall"))
        {
            WallData temp = selectedObject.GetComponent<WallData>();
            portrait2.sprite = selectedObject.GetComponent<LivingEntity>().Body.sprite;

            status.UpdateUIElement(0, $"{temp.MaxHealth}\n{temp.Health}");
            bool isActive = temp.GetComponent<GoldMine>() || selectedObject.GetComponent<Barracks>() || selectedObject.GetComponent<RepairTower>();
            if(!isActive)
            {
                if (temp.curLv >= temp.MaxLv)
                {
                    onClickPanel.uiButton[0].elementText.text = $"Q Key\nMax Lv";
                }
                else
                {
                    onClickPanel.uiButton[0].elementText.text = $"Q Key\nUpgrade\n{temp.curLv}/{temp.MaxLv}\n{temp.ReturnUpgradeBill()}G";
                }
            }
            if (temp.GetComponent<GoldMine>())
            {
                status.UpdateUIElement(1, $"{temp.GetComponent<GoldMine>().Amount}");
                status.UpdateUIElement(2, $"{temp.GetComponent<GoldMine>().maketime}\n{temp.GetComponent<GoldMine>().ReturnCur():F2}:");
            }
            else if (temp.GetComponent<RepairTower>())
            {
                status.UpdateUIElement(1, $"{temp.GetComponent<RepairTower>().Amount}");
                status.UpdateUIElement(2, $"{temp.GetComponent<RepairTower>().RepairRange}");
            }
        }
    }
}
