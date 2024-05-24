using NavMeshPlus.Components;
using UnityEngine;

public class WallData : LivingEntity
{
    [SerializeField]
    private int Cost = 10;
    public Rigidbody2D rb;
    private BoxCollider2D box;

    public GameObject warningEft;
    private GameObject warningObj;

    public int curLv = 0;
    public int MaxLv;
    public int[] gradeHealthCurve = { 200, 300, 400, 500, 600 };
    private int[] upgradebill = { 50, 100, 150, 200, 250 };
    public float healthPercentage;
    public float warningPercentage = 35f;
    private void Awake()
    {
        MaxLv = gradeHealthCurve.Length;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        box = GetComponent<BoxCollider2D>();

    }
    private void OnDestroy()
    {
        Destroy(warningObj);
        warningObj = null;
    }
    public int ReturnUpgradeBill()
    {
        return upgradebill[curLv];
    }
    private void LateUpdate()
    {
        healthPercentage = (float)Health / (float)MaxHealth * 100f; // ���� ü���� ����� ���
        if (healthPercentage < warningPercentage) // ü���� 20% �̸��̸�
        {
            if (warningObj == null)
            {
                warningObj = Instantiate(warningEft, transform.position, Quaternion.identity);
                warningObj.GetComponent<Renderer>().sortingOrder = Body.sortingOrder+1;
            }
            else
            {
                warningObj.SetActive(true);
            }
        }
        else // ü���� 20% �̻��̸�
        {
            if (warningObj != null)
            {
                warningObj.SetActive(false);
            }
        }
    }
    public int ReturnCurMax()
    {
        if (curLv >= gradeHealthCurve.Length)
        {
            // curLv�� gradeHealthCurve �迭�� ���̸� �Ѿ�� ������ ��Ҹ� ��ȯ
            return gradeHealthCurve[gradeHealthCurve.Length - 1];
        }
        else
        {
            // �׷��� ������ �ش� �ε����� ��Ҹ� ��ȯ
            return gradeHealthCurve[curLv];
        }
    }

    public virtual void Upgrade()
    {
        if (curLv < upgradebill.Length && curLv < gradeHealthCurve.Length)
        {
            if (upgradebill[curLv] <= GameManager.Instance.CurrentGoldGetter())
            {
                GameManager.Instance.SpendGold(upgradebill[curLv]);
                MaxHealth += gradeHealthCurve[curLv];
                Health += gradeHealthCurve[curLv] / 4;
                Instantiate(GameManager.Instance.boomEffect, transform.position, Quaternion.identity);
                animator.SetTrigger("Heal");
                curLv++;
            }
            else
            {
                PopUpText.DisplayText("���׷��̵� ����� ���ڶ��ϴ�.");
            }
        }
        else
        {
            PopUpText.DisplayText("�ִ� ����Դϴ�.");
        }
    }
    public void Sell()
    {
        base.Die();
        Instantiate(GameManager.Instance.boomEffect, transform.position, Quaternion.identity);
        Builder.Instance.builtObjects.Remove(this.gameObject);
    }
    public override void Die()
    {
        base.Die();
        Builder.Instance.builtObjects.Remove(gameObject);
        Instantiate(GameManager.Instance.boomEffect, transform.position, Quaternion.identity);
    }
    //public Color color;
    //private void Enable()
    //{

    //}
    //private void Start()
    //{
    //    Body.color = Color.red;
    //}

    //private void Update()
    //{
    //    Body.color = color;
    //}
    //public override void OnEnable()
    //{
    //    base.OnEnable();
    //    Body.color = Color.red;
    //}
    //public void Set()
    //{
    //    box.enabled = true;
    //    modifier.enabled = true;
    //    Builder.Instance.surface.BuildNavMesh();
    //}
    public int CostGetter()
    {
        return Cost;
    }
    public virtual void SetUp()
    {
        animator.SetTrigger("Heal");
        //SortingLayerByHalf();
        box.enabled = true;
    }
}
