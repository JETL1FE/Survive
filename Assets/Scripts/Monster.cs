using System.Collections;
using UnityEngine;

public class Monster : LivingEntity
{
    [Header("Enemy Status")]
    public Transform attackAnchor;
    public int damage = 10;
    public float speed = 3f;
    private float attackRate = 2f;
    private float curTime;
    public Vector2 attackRange;
    //public float attackRadius = 1f;
    public LayerMask TargetLayer;
    [SerializeField]
    private Transform AttackTarget = null;
    public GameObject HitParticle;

    [Header("Income Amount")]
    //public Item[] drops;
    public int killExp = 3;
    public int killGold = 5;

    private GameObject target;
    private Rigidbody2D rb;

    private bool canMove = true;
    private bool knockBackRunning = false;
    //private bool canAttack = true;
    //WaitForFixedUpdate wait;
    public override void OnEnable()
    {
        base.OnEnable();

    }
    private void Start()
    {
        //base.Start();
        GameManager.Instance.MonsterSpawnCount(+1);
        rb = GetComponent<Rigidbody2D>();
        //GameObject player = GameManager.Instance.PlayerGetter()?.gameObject;

        //if (player != null)
        //{
        //    target = player;
        //}
        //else
        //{
        //    target = null;
        //}
        if (GameManager.Instance.PlayerGetter())
        {
            target = GameManager.Instance.PlayerGetter().gameObject;

        }
        else
        {
            target = null;
        }
    }
    public override void Die()
    {
        base.Die();
        GameManager.Instance.MonsterSpawnCount(-1);
        //if (GameManager.Instance.IsAlive())
        //{

        //}
        GameManager.Instance.GetExp(killExp);
        GameManager.Instance.GainGold(killGold);
        if (GameManager.Instance.Items.Length > 0)
        {
            //int randomIndex = UnityEngine.Random.Range(0, GameManager.Instance.Items.Length); // drops �迭���� ������ �ε��� ����
            int randomIndex = UnityEngine.Random.Range(0, 10); // drops �迭���� ������ �ε��� ����
            //Instantiate(GameManager.Instance.Items[randomIndex], transform.position,Quaternion.identity);
            if (randomIndex < 4)
            {
                Instantiate(GameManager.Instance.Items[0], transform.position, Quaternion.identity);
            }
            else if (randomIndex < 8)
            {
                Instantiate(GameManager.Instance.Items[1], transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(GameManager.Instance.Items[2], transform.position, Quaternion.identity);
            }
        }
        //if (GameManager.Instance.Items.Length > 0)
        //{
        //    // ��� �����ۿ� ���� �� Ȯ���� ���
        //    float totalProbability = 0f;
        //    foreach (GameObject dropItem in GameManager.Instance.Items)
        //    {
        //        totalProbability += dropItem.GetComponent<Item>().dropProbability;
        //    }

        //    // ���� ���� ����
        //    float randomNum = Random.Range(0f, totalProbability);

        //    // �� �����ۿ� ���� Ȯ���� ���Ͽ� ������� ����
        //    foreach (GameObject dropItem in GameManager.Instance.Items)
        //    {
        //        if (randomNum < dropItem.GetComponent<Item>().dropProbability)
        //        {
        //            // �������� ���
        //            Instantiate(dropItem.item, transform.position, Quaternion.identity);
        //            break;
        //        }
        //        else
        //        {
        //            // ���� ���ڸ� �������� Ȯ����ŭ ����
        //            randomNum -= dropItem.GetComponent<Item>().dropProbability;
        //        }
        //    }
        //}
    }
    public void KnockOut(Vector2 hitpoint, float knockOutPower, float knockOutTime)
    {
        knockBackRunning = true;
        StartCoroutine(KnockBack(hitpoint, knockOutPower, knockOutTime));
    }
    IEnumerator KnockBack(Vector2 hitpoint, float knockOutPower, float knockOutTime)
    {
        Vector2 dirVec = new Vector2(transform.position.x, transform.position.y) - hitpoint;
        animator.SetTrigger("Hit");
        rb.AddForce(dirVec.normalized * knockOutPower * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockOutTime / 2);
        knockBackRunning = false;
    }
    IEnumerator KnockBack(float damage, Vector2 hitpoint)
    {
        canMove = false;
        Vector2 dirVec = new Vector2(transform.position.x, transform.position.y) - hitpoint;
        rb.AddForce(dirVec.normalized * damage / 4, ForceMode2D.Impulse);
        yield return new WaitForSeconds(damage / 6);
        canMove = true;
    }
    private void FixedUpdate()
    {
        if (!Dead)
        {
            if (GameManager.Instance.Invade)
            {
                if (target)
                {
                    if (!knockBackRunning)
                    {
                        Move();
                        FindAttackTarget();
                        MeleeAttack();
                    }
                }
            }
            else if (!GameManager.Instance.Invade || target == null)
            {
                Flee();
            }
            SortingLayerByHalf();
        }
    }
    //public override void OnDamage(float damage, Vector2 hitPoint, float knockback, float time)
    //{
    //    base.OnDamage(damage);

    //    StartCoroutine(KnockBack(hitPoint, knockback, time));
    //}
    public override void OnDamage(int damage, Vector2 hitPoint)
    {
        base.OnDamage(damage);
        StartCoroutine(KnockBack(damage, hitPoint));
    }
    public void MeleeAttack()
    {
        if (AttackTarget != null)
        {
            if (curTime <= 0)
            {
                animator.SetTrigger("Slash");
                StartCoroutine(AttackWithDelay());
                curTime = attackRate;
            }
            else
            {
                curTime -= Time.deltaTime;
            }
        }
    }
    IEnumerator AttackWithDelay()
    {
        yield return StartCoroutine(CheckAnimationState("Slash"));
        if (AttackTarget)
        {
            AttackTarget.GetComponent<LivingEntity>().OnDamage(damage);
            GameObject hitEffect = Instantiate(HitParticle, AttackTarget.position, Quaternion.identity);
        }
        else
        {
            yield break;
        }
    }
    private void FindAttackTarget()
    {
        Collider2D[] attackTargets = Physics2D.OverlapBoxAll(new Vector2(attackAnchor.position.x, attackAnchor.position.y),
            attackRange, 0, TargetLayer);
        if (attackTargets.Length > 0)
        {
            Collider2D closestCollider = attackTargets[0];
            float closestDistance = Vector2.Distance(transform.position, closestCollider.transform.position);
            for (int i = 1; i < attackTargets.Length; i++)
            {
                float distance = Vector2.Distance(transform.position, attackTargets[i].transform.position);
                if (distance < closestDistance)
                {
                    closestCollider = attackTargets[i];
                    closestDistance = distance;
                }
            }
            AttackTarget = closestCollider.transform;
            canMove = false;
        }
        else
        {
            canMove = true;
            AttackTarget = null;
        }
    }
    //private void FindAttackTarget()
    //{
    //    Collider2D[] attackTargets = new Collider2D[10]; // ����Ǵ� �ִ� ���� ��� ��
    //    int numTargets = Physics2D.OverlapCircleNonAlloc(new Vector2(attackAnchor.position.x, attackAnchor.position.y), attackRadius, attackTargets, TargetLayer);
    //    if (numTargets > 0)
    //    {
    //        Collider2D closestCollider = attackTargets[0];
    //        float closestDistance = Vector2.Distance(transform.position, closestCollider.transform.position);
    //        for (int i = 1; i < numTargets; i++)
    //        {
    //            float distance = Vector2.Distance(transform.position, attackTargets[i].transform.position);
    //            if (distance < closestDistance)
    //            {
    //                closestCollider = attackTargets[i];
    //                closestDistance = distance;
    //            }
    //        }
    //        AttackTarget = closestCollider.transform;
    //        canMove = false;
    //    }
    //    else
    //    {
    //        canMove = true;
    //        AttackTarget = null;
    //    }
    //}
    void Move()
    {
        if (canMove)
        {
            Vector2 direction = new Vector2(target.transform.position.x, target.transform.position.y) - rb.position;
            Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + nextVec);
            if (target.transform.position.x < rb.position.x)
            {
                transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z);
            }
            else
            {
                transform.localScale = PrevScale;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        animator.SetBool("Runing", canMove);
    }
    //void Flip()
    //{

    //}
    //void unFlee()
    //{
    //    Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    //    Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(screenCenter) - (Vector2)transform.position;
    //    Vector2 fleeDirection = direction.normalized;
    //    Vector2 nextVec = fleeDirection * speed * Time.fixedDeltaTime;
    //    rb.MovePosition(rb.position + nextVec);

    //    if (fleeDirection.x < 0)
    //    {
    //        transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z);
    //    }
    //    else
    //    {
    //        transform.localScale = PrevScale;
    //    }

    //    // �ִϸ����Ϳ� �̵� ���θ� �����մϴ�.
    //    animator.SetBool("Run", true);
    //}
    //void Flee()
    //{
    //    Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    //    Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(screenCenter) - (Vector2)transform.position;
    //    Vector2 fleeDirection = -direction.normalized;
    //    Vector2 nextVec = fleeDirection * speed * Time.fixedDeltaTime;
    //    rb.MovePosition(rb.position + nextVec);

    //    if (fleeDirection.x < 0)
    //    {
    //        transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z);
    //    }
    //    else
    //    {
    //        transform.localScale = PrevScale;
    //    }

    //    // �ִϸ����Ϳ� �̵� ���θ� �����մϴ�.
    //    animator.SetBool("Runing", true);
    //}
    void Flee()
    {
        // ���� ȭ���� �߾� ��ġ�� ����մϴ�.
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        // ȭ�� �߾����κ��� ���ͱ����� ������ ����մϴ�.
        Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(screenCenter) - (Vector2)transform.position;

        // ���� ���⿡�� �ణ�� ������ ������ ���� ���ο� ������ ����ϴ�.
        float randomAngle = Random.Range(-45f, 45f); // -45������ 45�� ������ ������ ����
        Vector2 fleeDirection = Quaternion.Euler(0f, 0f, randomAngle) * -direction.normalized;

        // ���ο� �������� �̵��մϴ�.
        Vector2 nextVec = fleeDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);

        // ������ �¿� ���� ���θ� �����մϴ�.
        if (fleeDirection.x < 0)
        {
            transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z);
        }
        else
        {
            transform.localScale = PrevScale;
        }

        // �ִϸ����Ϳ� �̵� ���θ� �����մϴ�.
        animator.SetBool("Runing", true);
    }
    //void Move(Vector2 targetPosition)
    //{
    //    if (canMove)
    //    {
    //        Vector2 direction = targetPosition - (Vector2)transform.position;
    //        rb.velocity = direction.normalized * speed;

    //        // �¿� ���� ��ȯ
    //        if (direction.x < 0)
    //        {
    //            transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z);
    //        }
    //        else
    //        {
    //            transform.localScale = PrevScale;
    //        }
    //    }
    //    else
    //    {
    //        rb.velocity = Vector2.zero;
    //    }
    //    animator.SetBool("Run", rb.velocity.magnitude > 0);
    //}

    //void Flee(Vector2 fleeDirection)
    //{
    //    if (canMove)
    //    {
    //        rb.velocity = fleeDirection.normalized * speed;

    //        // �¿� ���� ��ȯ
    //        if (fleeDirection.x < 0)
    //        {
    //            transform.localScale = new Vector3(-PrevScale.x, PrevScale.y, PrevScale.z);
    //        }
    //        else
    //        {
    //            transform.localScale = PrevScale;
    //        }
    //    }
    //    animator.SetBool("Run", canMove);
    //}
    public void Attack()
    {
        if (!canMove)
        {
            return;
        }
        animator.SetTrigger("Slash");
        canMove = false;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackRate);
        canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(attackAnchor.position.x, attackAnchor.position.y), attackRange);
        //Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
