using System.Collections.Generic;
using UnityEngine;

public class RepairTower : WallData
{
    public float RepairRange = 5f;
    public bool IsActive = false;
    public float ActivateDelay = 3f;
    public int Amount = 3;
    private int defalutValue;
    public LayerMask TargetLayer;
    public int upgradePay = 100;
    // 각 요소의 타이머와 그에 따른 상태를 추적하는 클래스
    [System.Serializable]
    private class TargetTimer
    {
        public LivingEntity Target;
        public float TimeLeft;
    }

    //[Serializable]
    [SerializeField]
    private List<TargetTimer> targetTimers = new List<TargetTimer>();

    void Start()
    {
        IsActive = true;
        defalutValue = Amount;
    }

    public override void Upgrade()
    {
        if (upgradePay <= GameManager.Instance.CurrentGoldGetter())
        {
            GameManager.Instance.SpendGold(upgradePay);
            Instantiate(GameManager.Instance.boomEffect, transform.position, Quaternion.identity);
            animator.SetTrigger("Heal");
            curLv++;
            Amount = defalutValue + curLv;
        }
    }
    void Update()
    {
        if (IsActive)
        {
            // 현재 타이머들 업데이트
            UpdateTimers();

            // 타이머가 만료된 요소들에 대해 RestoreHealth 호출
            ProcessExpiredTimers();

            // 새로운 요소들에 대한 타이머 생성
            CreateNewTimers();
        }
    }

    void UpdateTimers()
    {
        foreach (var timer in targetTimers)
        {
            timer.TimeLeft -= Time.deltaTime;
        }
    }

    void ProcessExpiredTimers()
    {
        for (int i = targetTimers.Count - 1; i >= 0; i--)
        {
            if (targetTimers[i].TimeLeft <= 0f)
            {
                targetTimers[i].Target.RestoreHealthByTower(Amount);
                targetTimers.RemoveAt(i);
            }
        }
    }
    void CreateNewTimers()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, RepairRange, TargetLayer);

        foreach (Collider2D collider in colliders)
        {
            LivingEntity livingEntity = collider.GetComponent<LivingEntity>();
            if (livingEntity != null && livingEntity != this && !HasTimerForTarget(livingEntity))
            {
                targetTimers.Add(new TargetTimer { Target = livingEntity, TimeLeft = ActivateDelay });
            }
        }
    }
    bool HasTimerForTarget(LivingEntity target)
    {
        foreach (var timer in targetTimers)
        {
            if (timer.Target == target)
            {
                return true;
            }
        }
        return false;
    }
}