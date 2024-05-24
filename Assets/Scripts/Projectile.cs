using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Projectile : MonoBehaviour
{

    private Transform target;
    public enum ProjectileType { Linear, Guided };
    private Rigidbody2D rb;
    private Vector3 initialDirection;

    [Header("Projectile")]
    private ProjectileType type;
    public int damage = 3;
    public float speed = 3f;
    //public Particles explosion = null;
    public GameObject hitParticle;
    public float rotationSpeed = 300f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke(nameof(DestroyProjectile), 3f);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            if (type == ProjectileType.Linear)   // 직선 이동
            {
                Vector3 moveDistance = speed * Time.deltaTime * initialDirection;
                rb.MovePosition(rb.position + (Vector2)moveDistance);
            }
            else if (type == ProjectileType.Guided)  // 추적 이동
            {
                Vector3 direction = (target.position - transform.position).normalized;
                Vector3 moveDistance = speed * Time.deltaTime * direction;
                rb.MovePosition(rb.position + (Vector2)moveDistance);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
    public void SetUp(Transform target, ProjectileType projectileType, int _damage)
    {
        this.target = target;
        this.type = projectileType;
        this.damage = _damage;
        if (type == ProjectileType.Linear)
        {
            initialDirection = (target.position - transform.position).normalized;
        }
    }
    public void SetUp(Transform target, ProjectileType projectileType, int _damage, float _speed)
    {
        this.damage = _damage;
        this.speed = _speed;
        this.target = target;
        this.type = projectileType;
        if (type == ProjectileType.Linear)
        {
            initialDirection = (target.position - transform.position).normalized;
        }
    }
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        ContactPoint2D contact2D = collision.contacts[0];
        Vector2 contactVector = contact2D.point;
        //collision.gameObject.GetComponent<Monster>().OnDamage(damage, contactVector);
        collision.gameObject.GetComponent<Monster>().OnDamage(damage);
        //explosion = Instantiate(explosion, contactVector, Quaternion.identity);
        hitParticle = Instantiate(hitParticle, contactVector, Quaternion.identity);
        Destroy(gameObject);
    }
}
