using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CharacterStats
{
    public int level = 1;
    public int damage = 10;
    public float pjSpeed = 20f;
    public float attackRate = 1f;
    public float attackRange = 5f;
}

public class Stats : MonoBehaviour
{
    public enum Grade
    {
        GradeA,
        GradeB,
        GradeC
    }

    public Grade characterGrade;
    public CharacterStats[] statsByGrade;

    private CharacterStats currentStats;

    //void Start()
    //{
    //    // ��޿� ���� ���� ����
    //    currentStats = statsByGrade[(int)characterGrade];
    //    ApplyStats();
    //}

    //void ApplyStats()
    //{
    //    damage = currentStats.damage;
    //}
}