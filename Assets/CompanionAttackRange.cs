using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAttackRange : MonoBehaviour
{
    
    private void Awake()
    {
        OffAttackRange();
    }

    public void OnAttackRange(Vector2 position, float range)
    {
        gameObject.SetActive(true);
        float diameter = range * 2.0f;
        transform.localScale = Vector3.one * diameter;
        transform.position = position;
    }
    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }
}
