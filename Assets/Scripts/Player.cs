using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Player : LivingEntity
{
    public enum PlayerState { Idle, Build };
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    [SerializeField]
    public NavMeshAgent agent;

    [Header("Attack Status")]
    [SerializeField] private float throwingCoolTime = 2f;
    [SerializeField] private float curTime;
    public Granade granadePrefab;
    public LayerMask TargetLayer;
    public Transform attackAncor;
    public int damage = 10;
    public float throwSpeed = 300f;
    //public Vector2 boxSize;
    public float knockbackBombRadius = 5;
    [Header("Sprite Status")]
    public PlayerState state;
    //private SpriteLibrary basicSprite;
    //private SpriteLibraryAsset prevSprite;
    //[SerializeField]
    //private SpriteLibraryAsset workSprite;
    void Start()
    {
        state = PlayerState.Idle;
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        //basicSprite = GetComponentInChildren<SpriteLibrary>();
        //prevSprite = basicSprite.spriteLibraryAsset;
    }
    void Update()
    {
        if (!Dead)
        {
            Flip();
            if (curTime <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Throw();
                    curTime = throwingCoolTime;
                }
            }
            else
            {
                curTime -= Time.deltaTime;
            }
            SortingLayerByHalf();
            if (Input.GetKeyDown(KeyCode.M))
            {
                MoveTo(Inputs.Instance.HalfVector);
            }
            transform.rotation = Quaternion.identity;
            TestCodes();
        }
    }

    void Throw()
    {
        animator.SetTrigger("Slash");
        StartCoroutine(ThrowGranade());
    }
    private IEnumerator ThrowGranade()
    {
        Vector2 targetpos = Inputs.Instance.HalfVector;
        transform.LookAt(targetpos);
        yield return CheckAnimationState("Slash"); ;
        Granade clone = Instantiate(granadePrefab, attackAncor.position, Quaternion.identity);
        clone.SetUp(targetpos, damage, throwSpeed);
    }
    private void Flip()
    {
        if (agent.velocity.magnitude > 0)
        {
            if (transform.position.x > lastDirection.x)
            {
                transform.localScale = PrevScale;
            }
            else if (transform.position.x < lastDirection.x)
            {
                transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z);
            }
            lastDirection = transform.position;
        }
        animator.SetBool("Runing", agent.velocity.magnitude > 0);
        rb.velocity = Vector2.zero;
    }
    public void LookAt(Vector2 targetPos)
    {
        if (transform.position.x < targetPos.x)
        {
            transform.localScale = PrevScale; // �������� �ٶ󺸵��� ����
        }
        else
        {
            transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z); // ������ �ٶ󺸵��� ����
        }
    }
    public void MoveTo(Vector2 _targetPos)
    {
        if (agent.hasPath)
        {
            agent.ResetPath();
        }
        agent.SetDestination(_targetPos);
    }
    private IEnumerator MovePlayerTo(Vector2 targetPos)
    {
        if (agent.enabled == true)
        {
            agent.SetDestination(targetPos);
            while (Vector2.Distance(this.transform.position, targetPos) > 0.3f)
            {
                yield return null;
            }
            agent.ResetPath();
        }
    }

    private void TestCodes()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            animator.SetTrigger("Hit");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            animator.SetTrigger("Heal");
        }
    }
    public void StopMove()
    {
        if (agent.hasPath)
        {
            agent.ResetPath();

        }
        agent.velocity = Vector3.zero;
        Debug.Log("StopMove");
    }

    //public void ChangeState()
    //{
    //    if (Buildable.Instance.CurrentObjectGetter() || Buildable.Instance.OnClickObjectGetter())
    //    {
    //        basicSprite.spriteLibraryAsset = workSprite;
    //    }
    //    else
    //    {
    //        basicSprite.spriteLibraryAsset = prevSprite;
    //    }
    //    //switch (index)
    //    //{
    //    //    case 0:
    //    //        state = PlayerState.Idle;

    //    //        break;
    //    //    case 1:
    //    //        state = PlayerState.Build;

    //    //        break;
    //    //}
    //}

    //public bool AgentEnable()
    //{
    //    if (dead) return false;
    //    return agent.enabled;
    //}
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    //Gizmos.DrawWireCube(meleeAncor.position, boxSize);
    //    Gizmos.DrawWireSphere(transform.position, knockbackBombRadius);
    //}

    //void MoveWithInput()
    //{
    //    // ����� �Է� �ޱ� (��: Ű���� �Է�)
    //    float horizontalInput = UserInput.Instance.HorizontalInput;
    //    float verticalInput = UserInput.Instance.VerticalInput;

    //    // �̵� ���� ���
    //    Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

    //    // �̵� ������ �����ϴ� ��쿡�� ������ ����
    //    if (movementDirection != Vector3.zero)
    //    {
    //        // ���� ��ġ���� ���� �Ÿ���ŭ �̵��� ��ġ�� ����Ͽ� ������ ����
    //        Vector3 targetPosition = transform.position + movementDirection * 3f;

    //        // ����޽� ������Ʈ���� ������ ����
    //        agent.SetDestination(targetPosition);
    //    }
    //}

}
