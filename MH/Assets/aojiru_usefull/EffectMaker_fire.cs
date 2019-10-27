using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aojilu;

public class EffectMaker_fire : EffectMaker
{
    [SerializeField] animationUtil fire;
    [SerializeField] animationUtil fireBig;
    [SerializeField] Fire_sorairo_bunretu fireBunretu;
    [SerializeField] Vector2 fireVector;
    [SerializeField] float fireLength;

    void ShotCreat(animationUtil target)
    {
        animationUtil obj = Instantiate(target, effectPosition.position, Quaternion.identity);
        var vec = fireVector;
        vec.x *= Mathf.Sign(-transform.localScale.x);
        obj.SetVelocity(vec);
        obj.SetDedLength(fireLength);
    }

    public void FireCreate()
    {
        ShotCreat(fire);
    }
    public void FireCreate_big()
    {
        ShotCreat(fireBig);
    }


    public void FireCreate_buntetu()
    {
        Instantiate(fireBunretu, effectPosition.position, Quaternion.identity);
    }
}
