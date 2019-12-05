using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdditionalAction_aniamtion : UIAdditionalActon_base
{
    Animator anim;

    protected override void InitAction()
    {
        base.InitAction();
        if (myCanvas.IsUseAnimator)
        {
            anim = GetComponent<Animator>();
        }
    }

    public void SetTrrigerAnim(string key)
    {
        anim.SetTrigger(key);
    }
    public void SetBoolTrueAnim(string key)
    {
        anim.SetBool(key, true);
    }
    public void SetBoolFalseAnim(string key)
    {
        anim.SetBool(key, false);
    }
}
