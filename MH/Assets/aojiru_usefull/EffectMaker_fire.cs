using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aojilu;

public class EffectMaker_fire : EffectMaker
{
    [SerializeField] animationUtil fire;
    [SerializeField] Fire_sorairo_bunretu fireBunretu;
    [SerializeField] Vector2 fireVector;
    [SerializeField] float fireLength;

    public void FireCreate()
    {
        animationUtil obj = Instantiate(fire, effectPosition.position, Quaternion.identity);
        var vec = fireVector;
        vec.x *= Mathf.Sign(-transform.localScale.x);
        obj.SetVelocity(vec);
        obj.SetDedLength(fireLength);
    }


    public void FireCreate_buntetu()
    {
        Instantiate(fireBunretu, effectPosition.position, Quaternion.identity);
    }
}
