using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aojilu;

[RequireComponent(typeof(MonstarMapChengeCtrl))]
public class Enemy_sorairoDoragon : EnemyMain_antiFly
{
    
    [SerializeField,Space(10)] float aiStateNum_walk_ud;
    [SerializeField] float aiStateNum_wait_ud;
    [SerializeField] float aiStateNum_rest_ud;

    [SerializeField] float aiStateNum_walk;
    [SerializeField] float aiStateNum_dash;
    [SerializeField] float aiStateNum_attack;

    [SerializeField] float aiStateFrontPl_dash;
    [SerializeField] float aiStateFrontPl_head;
    [SerializeField] float aiStateFrontPl_tail;
    [SerializeField] float aiStateFrontPl_jump;
    [SerializeField] float aiStateFrontPl_fire;

    Vector2? nowTargetPos;

    [SerializeField,Space(10)] Vector2 fireVector;
    [SerializeField] float fireLength;
    [SerializeField] animationUtil fire;
    [SerializeField] Fire_sorairo_bunretu fireBunretu;
    [SerializeField] Transform effectPos;

    int jumpCounter=0;
    int beforeCounter=-1;

    #region AI
    protected override void AIUpdate_undetect()
    {
        base.AIUpdate_undetect();
        float rand = GetAIRandaomNumver();

        switch (aiState)
        {
            case AISTATE.AISELECT:
                if (rand <AddAIProbNum(aiStateNum_walk_ud)) SetAIState(AISTATE.APPROACH_WALK, 5.0f);
                else if (rand < AddAIProbNum(aiStateNum_wait_ud)) SetAIState(AISTATE.WAIT, 3.0f);
                else if (rand < AddAIProbNum(aiStateNum_rest_ud)) SetAIState(AISTATE.ATTACK, 5.0f);
                break;
            case AISTATE.WAIT:
                break;
            case AISTATE.APPROACH_WALK:
                if (nowTargetPos == null)
                {
                    rand = GetAIRandaomNumver();
                    nowTargetPos = (Vector2)transform.position + new Vector2((rand > 50) ? 20 : -20, 0);
                }
                if (EnemyCtrl_antiFly.MoveToTarget_X(3.0f, (Vector2)nowTargetPos, moveSpeed))
                {
                    SetAIState(AISTATE.WAIT, 2.0f);
                    nowTargetPos = null;
                }
                break;
            case AISTATE.ATTACK:
                animator.SetTrigger("rest");
                SetAIState(AISTATE.WAIT, 5.0f);
                break;
        }
    }

    protected override void AIUpdate_detect()
    {
        base.AIUpdate_detect();
        float rand = GetAIRandaomNumver();
        switch (aiState)
        {
            case AISTATE.AISELECT:
                if (rand < AddAIProbNum(aiStateNum_walk)) SetAIState(AISTATE.APPROACH_WALK, 6.0f);
                else if (rand < AddAIProbNum(aiStateNum_dash)) SetAIState(AISTATE.APPROACH_DASH, 6.0f);
                else if (rand <AddAIProbNum(aiStateNum_attack)) SetAIState(AISTATE.ATTACK, 4.0f);
                else
                {
                    SetAIState(AISTATE.WAIT, 4.0f);
                    animator.SetTrigger("rest");
                }
                break;
            case AISTATE.APPROACH_WALK:
                if (EnemyCtrl_antiFly.MoveToPlayer_X(10.0f, moveSpeed * 0.5f))
                {
                    SetAIState(AISTATE.ATTACK, 20.0f);
                }
                break;
            case AISTATE.APPROACH_DASH:
                if (EnemyCtrl_antiFly.MoveToPlayer_X(10.0f, moveSpeed))
                {
                    SetAIState(AISTATE.ATTACK, 4.0f);
                }
                break;
            case AISTATE.ATTACK:
                break;
            case AISTATE.WAIT:
                break;
        }
    }

    protected override void AIUpdate_attack()
    {
        base.AIUpdate_attack();
        float rand = GetAIRandaomNumver();
        if (rand < AddAIProbNum(aiStateFrontPl_dash))
        {
            EnemyCtrl_antiFly.SetDirectionToPl();
            animator.SetTrigger("attack2");
            StartAttack();
        }
        else if (rand < AddAIProbNum(aiStateFrontPl_head))
        {
            if (EnemyCtrl_antiFly.IsPlayerFront())
            {
                animator.SetTrigger("attack1");
                StartAttack();
            }
        }
        else if (rand <AddAIProbNum(aiStateFrontPl_tail))
        {
            if (!EnemyCtrl_antiFly.IsPlayerFront())
            {
                animator.SetTrigger("attack3");
                StartAttack();
            }
        }else if (rand < AddAIProbNum(aiStateFrontPl_jump))
        {
            if (EnemyCtrl_antiFly.GetDistancePlayer_X() > 10.0f && !IsFixedRandomNumber) return;
            if (jumpCounter == beforeCounter) return;
            if (jumpCounter >= 3)
            {
                animator.SetTrigger("fall1");
                ResetCounter();
                ResetRandFiexed();
                StartAttack();
            }
            else
            {
                animator.SetTrigger("jump");
                SetRandFiexed(rand);
                beforeCounter = jumpCounter;
                ExtendStateTime(3.0f);
            }
        }
        else if(rand<AddAIProbNum(aiStateFrontPl_fire))
        {
            if (EnemyCtrl_antiFly.GetDistancePlayer_X() > 5.0f)
            {
                EnemyCtrl_antiFly.SetDirectionToPl();
                animator.SetTrigger("attack4");
                StartAttack();
            }
        }
    }

    protected override void AIUpdate_antiFly_detect()
    {
        base.AIUpdate_antiFly_detect();
        switch (aiState)
        {
            case AISTATE.AISELECT:
                SetAIState(AISTATE.APPROACH_DASH, 3.0f);
                break;
            case AISTATE.APPROACH_DASH:
                if (EnemyCtrl_antiFly.MoveToPlayer_X(1.0f, moveSpeed))
                {
                    SetAIState(AISTATE.ATTACK, 4.0f);
                }
                break;
        }
    }

    protected override void AIUpdate_antiFly_attack()
    {
        base.AIUpdate_antiFly_attack();
        animator.SetTrigger("attack5");
        SetAIState(AISTATE.WAIT, 5.0f);
        StartAttack();
    }
    #endregion
    protected override void AIUpdate_mapChenge()
    {
        base.AIUpdate_mapChenge();
        EnemyCtrl_antiFly.MoveToTarget_X(0.0f, mapChengeCtrl.GetNextLoadPosition(),EnemyCtrl_antiFly.MoveSpeed);
    }


    public void FireCreate()
    {
        animationUtil obj = Instantiate(fire, effectPos.position, Quaternion.identity);
        var vec = fireVector;
        vec.x *= Mathf.Sign(-transform.localScale.x);
        obj.SetVelocity(vec);
        obj.SetDedLength(fireLength);
    }

    public void FireCreate_buntetu()
    {
        Instantiate(fireBunretu, effectPos.position, Quaternion.identity);
    }


    public override void EndAnimation()
    {
        base.EndAnimation();
        jumpCounter++;
    }

    void ResetCounter()
    {
        jumpCounter = 0;
        beforeCounter = -1;
    }
}
