using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpViewer : MonoBehaviour
{
    private LivingEntity entity;
    private Slider hpSlider;

    public void SetUp(LivingEntity _entity)
    {
        this.entity = _entity;
        hpSlider = GetComponent<Slider>();
    }
    void Update()
    {
        float normalizedHealth = (float)entity.Health / entity.MaxHealth;
        hpSlider.value = normalizedHealth;
    }
}
