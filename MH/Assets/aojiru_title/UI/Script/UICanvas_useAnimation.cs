using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas_useAnimation : UICanvas
{
    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void StateUpdate()
    {
        switch (StateMode)
        {
            case State.AWAKE:
                break;
            case State.NONE:
                break;
            case State.END:
                break;
            case State.SLEEP:
                break;
        }
    }

    public override void SetStateEnd()
    {
        base.SetStateEnd();
        animator.SetTrigger("close");
    }

    public void StateAnimEvent_awakeEnd()
    {
        SetState(State.NONE);
    }

    public void StateAnimEvent_CloseEnd()
    {
        SetState(State.SLEEP);
        gameObject.SetActive(false);
    }
}
