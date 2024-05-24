using System;
using System.Collections;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    [Header("Basic Status")]
    [SerializeField] private int startingHealth = 100;
    [SerializeField] private int _health;
    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }
    public int MaxHealth
    {
        get { return startingHealth;}  
        set { startingHealth = value; }
    }

    public bool Dead { get; set; }
    public SpriteRenderer Body;
    protected float exitTime = 0.8f;
    protected Vector3 PrevScale;
    protected Vector3 lastDirection;
    protected Animator animator;
    public Action onDeath { get; private set; }
    public virtual void OnEnable()
    {
        Dead = false;
        Health = startingHealth;
        animator = GetComponentInChildren<Animator>();
        Body = GetComponentInChildren<SpriteRenderer>();
        PrevScale = transform.localScale;
        SortingLayerByHalf();
        onDeath = Die;
    }
    public virtual void OnDamage(int damage, Vector2 hitPoint, Vector2 hitNomal)
    {
        Health -= damage;
    }
    public virtual void OnDamage(int damage, Vector2 hitPoint, float knockback, float time)
    {

    }
    public virtual void OnDamage(int damage, Vector2 hitPoint)
    {
        
    }
    public IEnumerator CheckAnimationState(string clipname)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(clipname))
        {
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
    }
    public virtual void OnDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0 && !Dead)
        {
            Die();
        }
        animator.SetTrigger("Hit");
    }
    public void Trigger(string clip)
    {
        animator.SetTrigger(clip);
    }
    public void StateBool(string name,bool _bool)
    {
        animator.SetBool(name, _bool);
    }
    public virtual void RestoreHealth(int plusHealth)
    {
        if (Dead)
        {
            return;
        }
        else if (startingHealth == Health)
        {
            Debug.Log("체력이 가득차서 수리가 불필요하다");
            return;
        }
        else
        {
            Health += plusHealth;
            if (Health > startingHealth)
            {
                Health = startingHealth;
            }
            animator.SetTrigger("Heal");
        }

    }
    public virtual void RestoreHealthByTower(int plusHealth)
    {
        if (Dead)
        {
            return;
        }
        else if (startingHealth == Health)
        {
            Debug.Log("체력이 가득차서 수리가 불필요하다");
            return;
        }
        else
        {
            Instantiate(GameManager.Instance.HealEffect, gameObject.transform.position, Quaternion.identity);
            Health += plusHealth;
            if (Health > startingHealth)
            {
                Health = startingHealth;
            }
            animator.SetTrigger("Heal");
        }

    }
    
    public virtual void Die()
    {
        Dead = true;
        if(gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger("Dead");
            StartCoroutine(Death());
        }

    }
    public IEnumerator Death()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
    public void SortingLayerByHalf()
    {
        Body.sortingOrder = Mathf.RoundToInt(transform.position.y * 2) * -1;
    }
}
