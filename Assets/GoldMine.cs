using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GoldMine : WallData
{
    public int Amount = 10;
    public float maketime = 3f;
    [SerializeField]
    private float curTime;
    private int depot = 3;

    public GameObject Particle;
    void Start()
    {        
        //curTime = maketime;
        //Builder.Instance.populationLimit += depot;
    }
    public override void SetUp()
    {
        base.SetUp();
        curTime = maketime;
        Builder.Instance.populationLimit += depot;
    }
    public override void OnEnable()
    {
        base.OnEnable();
    }
    private void OnDestroy()
    {
        Builder.Instance.populationLimit -= depot;
    }
    // Update is called once per frame
    void Update()
    {
        //if(!GameManager.Instance.PlayerGetter().Dead && GameManager.Instance.StartLine)
        //{

        //}
        if (curTime <= 0)
        {

            MakeGold();
            curTime = maketime;
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    void MakeGold()
    {
        GameManager.Instance.GainGold(Amount);
        Instantiate(Particle, transform.position, Quaternion.identity);
        animator.SetTrigger("Brr");
    }
    public float ReturnCur()
    {
        return curTime;
    }
}
