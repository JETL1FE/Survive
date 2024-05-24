using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.U2D.Animation;

public class Companion : LivingEntity
{

    private SpriteLibrary spriteLibrary;
    public SpriteLibraryAsset[] sprites = new SpriteLibraryAsset[Enum.GetValues(typeof(Grade)).Length];
    public enum State { Wanderer, Companion, HoldPosition };
    public enum Grade { A, B, C, D }
    public int GradeLimitInt = Enum.GetValues(typeof(Grade)).Length;
    public Projectile projectilePrefab;
    [Header("Ally Status")]
    public State state;
    public Grade grade;
    [Header("Attack Status")]
    private Transform ProjectileSpawnPoint;
    //public CompanionStats Stats;

    public int damage = 15;
    public float pjSpeed = 30;
    public float attackRate = 2.8f;
    public float attackRange = 3f;
    public int[] damages = new int[Enum.GetValues(typeof(Grade)).Length];
    public float[] protSpeeds = new float[Enum.GetValues(typeof(Grade)).Length];
    public float[] attackRates = new float[Enum.GetValues(typeof(Grade)).Length];
    public float[] attackRanges = new float[Enum.GetValues(typeof(Grade)).Length];

    private float timeSinceLastAttack = 0f;
    [SerializeField]
    private LayerMask TargetLayer;
    [SerializeField] 
    private Transform attackTarget = null;
    [Header("Moving Status")]
    public float moveSpeed = 3f;
    public float avoidDistance = 1.25f;
    public float fleeSpeed = 1.5f;
    public float distanceThresholdForConstruction = 0.5f;
    public string Name;
    private BoxCollider2D box;
    private NavMeshAgent agent;

    private OffScreenIndicator offScreenIndicator; // OffScreenIndicator 스크립트를 참조할 변수
    private GameObject indicator; // 생성된 인디케이터를 저장할 변수
    [SerializeField]
    private bool isMoving = false;
    void Start()
    {
        if(!ProjectileSpawnPoint)
        {
            ProjectileSpawnPoint = transform.Find("Attack Point").GetComponent<Transform>();
        }
        agent = GetComponent<NavMeshAgent>();
        offScreenIndicator = FindObjectOfType<OffScreenIndicator>();
        if (offScreenIndicator != null)
        {
            indicator = Instantiate(offScreenIndicator.indicatorPrefab);
            offScreenIndicator.targetIndicator.Add(gameObject, indicator);
        }
        else
        {
            Debug.LogError("OffScreenIndicator를 찾을 수 없습니다.");
        }
        spriteLibrary = GetComponentInChildren<SpriteLibrary>();
        SetAndCalStatus();
        StatusSetting();

    }
    public override void OnEnable()
    {
        base.OnEnable();
        Builder.Instance.curpopulation += 1;
        ////int temp = GameManager.Instance.picker.PickGrade();
        //GrandeSetter(temp);
        gameObject.name = $"{Name} [{grade}]";
        //PopUpText.DisplayText($"{gameObject.name}를 뽑았다.");
    }
    public void GradeSetUp(int grade)
    {

    }
    private void SetAndCalStatus()
    {
        for (int i = 0; i < GradeLimitInt; i++)
        {
            damages[i] = damage + i*3;
            protSpeeds[i] = pjSpeed + i*2;
            attackRates[i] = attackRate - i*0.5f;
            attackRanges[i] = attackRange + i*0.5f;
        }
    }
    public void GradeUp()
    {
        if (this.grade < Grade.D)
        {
            grade += 1;
            gameObject.name = $"{Name} [{grade}]";
            StatusSetting();
        }
        else
        {
            Debug.Log("cant");
        }
    }
    public void StatusSetting()
    {
        damage = damages[(int)grade];
        pjSpeed = protSpeeds[(int)grade];
        attackRate = attackRates[(int)grade];
        attackRange = attackRanges[(int)grade];
        spriteLibrary.spriteLibraryAsset = sprites[(int)grade];
    }
    public float ReturnCurAttackRate()
    {
        return timeSinceLastAttack;
    }
    private void OnDestroy()
    {
        if (indicator != null)
        {
            Destroy(indicator);
        }
        if (offScreenIndicator != null && offScreenIndicator.targetIndicator.ContainsKey(gameObject))
        {
            offScreenIndicator.targetIndicator.Remove(gameObject);
        }
        if (Builder.Instance != null)
        {
            Builder.Instance.companions.Remove(this);
            Builder.Instance.curpopulation -= 1;
        }
        else
        {
            Debug.LogWarning("Builder.Instance is null when trying to remove companion.");
        }

    }
    private void OnDisable()
    {
        if (indicator != null)
        {
            Destroy(indicator);
        }
        if (offScreenIndicator != null && offScreenIndicator.targetIndicator.ContainsKey(gameObject))
        {
            offScreenIndicator.targetIndicator.Remove(gameObject);
        }

        if (Builder.Instance != null)
        {
            Builder.Instance.companions.Remove(this);
        }
        else
        {
            Debug.LogWarning("Builder.Instance is null when trying to remove companion.");
        }
        //Destroy(this.gameObject);
        Destroy(this);
    }
    //public void StatusSetup(CompanionStats stats)
    //{
    //    Stats = stats;
    //    damage = Stats.damage;
    //    attackRate = Stats.attackRate;
    //    attackRange = Stats.attackRange;
    //    pjSpeed = Stats.pjSpeed;
    //}
    public void AdjustStats()
    {
        //switch (index)
        //{
        //    case 0:
        //        grade = Grade.A;
        //        break;
        //    case 1:
        //        grade = Grade.B;
        //        break;
        //    case 2:
        //        grade = Grade.C;
        //        break;
        //    case 3:
        //        grade = Grade.D;
        //        break;
        //}

        damage += ((int)grade + 1) * 2;
        pjSpeed += +((int)grade + 1) * 2;
        attackRate -= (float)grade / 3;
        attackRange += ((int)grade + 1) * 2;

        // 스케일을 조절하기 위해 Vector3 객체를 생성하여 사용합니다.
        //Vector3 newScale = Vector3.one * ((int)grade + 1);
        //transform.localScale = newScale;


    }
    //public override void OnEnable()
    //{
    //    base.OnEnable();

    //}
    public void GrandeSetter(int _grade)
    {
        //switch (_grade)
        //{
        //    case 0:
        //        grade = Grade.A;
        //        break;
        //    case 1:
        //        grade = Grade.B;
        //        break;
        //    case 2:
        //        grade = Grade.C;
        //        break;
        //    case 3:
        //        grade = Grade.D;
        //        break;
        //}
        if (_grade > 3)
        {
            grade = Grade.D;
            Debug.Log("매개변수가 3을 초과했습니다. 확인 필요~");
        }
        else
        {
            grade = (Grade)_grade;
        }

    }
    public void Update()
    {
        if (!Dead)
        {
            transform.rotation = Quaternion.identity;
            Move();
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, TargetLayer);
            if (state == State.Wanderer) //초기 생성된 상태
            {
                if (hitColliders.Length > 0)
                {
                    // 가장 가까운 몬스터와의 거리 계산
                    float closestDistance = Mathf.Infinity;
                    Vector3 closestMonsterPosition = Vector3.zero;
                    foreach (Collider2D hitCollider in hitColliders)
                    {
                        float distanceToMonster = Vector3.Distance(transform.position, hitCollider.transform.position);
                        if (distanceToMonster < closestDistance)
                        {
                            closestDistance = distanceToMonster;
                            closestMonsterPosition = hitCollider.transform.position;
                        }
                    }
                    Vector3 fleeDirection = transform.position - closestMonsterPosition;
                    Vector3 targetPosition = transform.position + fleeDirection.normalized * avoidDistance;
                    agent.SetDestination(targetPosition);
                }
            }
            else if (state == State.Companion)
            {
                if (!isMoving)
                {
                    agent.SetDestination(GameManager.Instance.PlayerGetter().transform.position);
                }
                else
                {

                }
            }
            else if (state == State.HoldPosition)
            {
                if (timeSinceLastAttack < attackRate)
                {
                    timeSinceLastAttack += Time.deltaTime;
                }
                if (attackTarget != null)
                {
                    // 이미 타겟이 설정되어 있는 경우
                    if (timeSinceLastAttack >= attackRate)
                    {
                        animator.SetTrigger("Slash");
                        StartCoroutine(AttackFinishedChekcer());
                        timeSinceLastAttack = 0f;

                    }
                    float distanceToTarget = Vector2.Distance(transform.position, attackTarget.position);

                    // 타겟이 나의 공격 범위를 벗어나거나 타겟이 파괴되었을 경우
                    if (distanceToTarget > attackRange || attackTarget.gameObject == null)
                    {
                        FindNewTarget();
                    }
                }
                else
                {
                    FindNewTarget();
                }

                if (hitColliders.Length > 0)
                {
                    Collider2D closestCollider = hitColliders[0];
                    float closestDistance = Vector2.Distance(transform.position, closestCollider.transform.position);

                    for (int i = 1; i < hitColliders.Length; i++)
                    {
                        float distance = Vector2.Distance(transform.position, hitColliders[i].transform.position);
                        if (distance < closestDistance)
                        {
                            closestCollider = hitColliders[i];
                            closestDistance = distance;
                        }
                    }
                    attackTarget = closestCollider.transform;
                }
                else
                {
                    attackTarget = null;
                }
                RotateToTarget();
            }
            SortingLayerByHalf();
            //transform.localScale *= level;
        }
    }



    IEnumerator AttackFinishedChekcer()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0)
        .IsName("Slash"))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0)
        .normalizedTime < exitTime)
        {
            yield return null;
        }
        SpawnProjectile();
    }


    private void ClearAgentPath()
    {
        if (agent.hasPath)
        {
            agent.ResetPath();
        }
    }
    public void MoveToMouseClick()
    {
        // 마우스 클릭 위치로 이동
        StartCoroutine(WaitForMouseClick());
    }
    private IEnumerator WaitForMouseClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭
            {
                Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                MoveTo(clickPosition);
                yield break; // 코루틴 종료
            }
            yield return null;
        }
    }
    public void MoveTo(Vector2 _targetPos)
    {
        ClearAgentPath();
        agent.SetDestination(_targetPos);
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

    public void MoveCoroutine(Vector2 targetPos)
    {
        if (!isMoving) // 이동 중이 아닐 때만 코루틴을 시작합니다.
        {
            state = State.Companion;
            agent.SetDestination(targetPos);
            StartCoroutine(DistanceChecker(targetPos));
            Debug.Log("ALLY MOVE COROUTINE START");
        }
    }
    IEnumerator DistanceChecker(Vector2 targetPos)
    {
        isMoving = true; // 이동 중임을 나타내는 변수
        float maxTime = 2f; // 최대 시간 간격.
        float elapsedTime = 0f; // 경과 시간을 저장하는 변수
        while (Vector2.Distance(transform.position, targetPos) > distanceThresholdForConstruction && elapsedTime < maxTime)
        {
            //Debug.Log("이동중...");
            elapsedTime += Time.deltaTime; // 경과 시간을 누적
            yield return null;
        }
        Debug.Log("이동완료");
        isMoving = false;
        StopMove();
        yield return new WaitForEndOfFrame();
        state = State.HoldPosition;
    }
    void FindNewTarget()
    {
        Collider2D closestCollider = null;
        float closestDistance = float.MaxValue;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), attackRange, TargetLayer);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                float distance = Vector2.Distance(transform.position, colliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closestCollider = colliders[i];
                    closestDistance = distance;
                }
            }
        }
        // 가장 가까운 Collider2D의 Transform을 attackTarget에 할당
        attackTarget = closestCollider != null ? closestCollider.transform : null;
    }


    public void StateChanger()
    {
        if (state == State.Wanderer)
        {
            state = State.Companion;
            agent.speed = fleeSpeed;
        }
        else if (state == State.Companion)
        {
            state = State.HoldPosition;
            agent.speed = moveSpeed;
        }
        else if (state == State.HoldPosition)
        {
            state = State.Companion;
        }
    }
    private void Move()
    {
        if (agent.enabled == true)
        {
            animator.SetBool("Runing", agent.velocity.magnitude > 0);
            if (agent.velocity.x < 0)
            {
                this.transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z);
            }
            else
            {
                this.transform.localScale = new Vector3(PrevScale.x, PrevScale.y, PrevScale.z);
            }
        }
    }
    private void RotateToTarget()
    {
        if (attackTarget)
        {
            if (attackTarget.position.x < this.transform.position.x)
            {
                transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z);
            }
            else
            {
                transform.localScale = new Vector3(PrevScale.x, PrevScale.y, PrevScale.z);
            }
        }
    }
    private void SpawnProjectile()
    {
        if (attackTarget == null)
        {
            return;
        }
        Projectile clone = Instantiate(projectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().SetUp(attackTarget, Projectile.ProjectileType.Linear, damage, pjSpeed);
    }
}
