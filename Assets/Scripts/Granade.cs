using System.Collections;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [Header("Enemy Status")]
    public GameObject bombParticle;
    public int damage = 3;
    public float speed = 300f;
    public float radius = 3f;
    public float rotationSpeed = 100f;
    public float knockOutTime = 2;
    public float knockOutDistance = 2;
    public float timeToBoom = 3f;
    public LayerMask Target;
    private Rigidbody2D rb;
    private Vector3 target;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SpinAndWait());
        //Invoke("SpinAndWait", 0.1f);
    }
    IEnumerator SpinAndWait()
    {
        yield return new WaitForSeconds(timeToBoom);
        GameObject tempParticle = Instantiate(bombParticle, transform.position, Quaternion.identity);
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius, Target);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider is BoxCollider2D boxCollider)
            {
                Monster tempEnemy = boxCollider.GetComponent<Monster>();
                tempEnemy.OnDamage(damage);
                tempEnemy.KnockOut(transform.position, knockOutDistance, knockOutTime);
            }
        }
        Destroy(gameObject);
    }
    public void SetUp(Vector3 _target, float _speed)
    {
        target = _target;
        speed = _speed;
    }
    public void SetUp(Vector3 _target, int _damage, float _speed)
    {
        target = _target;
        damage = _damage;
        speed = _speed;
    }
    private void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        Vector3 direction = (target - transform.position).normalized;
        Vector3 moveDistance = speed * Time.deltaTime * direction;
        rb.MovePosition(rb.position + (Vector2)moveDistance);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
