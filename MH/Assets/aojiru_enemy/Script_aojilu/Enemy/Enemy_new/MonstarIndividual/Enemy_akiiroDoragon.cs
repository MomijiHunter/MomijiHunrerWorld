using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aojilu;
[RequireComponent(typeof(MonstarMapChengeCtrl))]
public class Enemy_akiiroDoragon : EnemyMain_fly
{
    Vector2? nowTargetPos;
    #region AI確率変数
    [SerializeField, Space(10)] float aiStateNum_ud_walk;
    [SerializeField] float aiStateNum_ud_wait;
    [SerializeField] float aiStateNum_ud_rest;

    [SerializeField, Space(10)] float aiStateNum_walk;
    [SerializeField] float aiStateNum_dash;
    [SerializeField] float aiStateNum_attack;
    [SerializeField] float aiStateNum_fly;


    [SerializeField,Space(10)] float aiStateFrontPl_dash;
    [SerializeField] float aiStateFrontPl_head;
    [SerializeField] float aiStateFrontPl_tail;
    
    #endregion
    #region AI
    protected override void AIUpdate_undetect()
    {
        base.AIUpdate_undetect();
        float rand = GetAIRandaomNumver();
        switch (aiState)
        {
            case AISTATE.AISELECT:
                if (rand < AddAIProbNum(aiStateNum_ud_walk)) SetAIState(AISTATE.APPROACH_WALK, 4.0f);
                else if (rand < AddAIProbNum(aiStateNum_ud_wait)) SetAIState(AISTATE.WAIT, 4.0f);
                else if (rand < AddAIProbNum(aiStateNum_ud_rest)) SetAIState(AISTATE.ATTACK, 5.0f);
                break;
            case AISTATE.WAIT:
                break;
            case AISTATE.APPROACH_WALK:
                if (nowTargetPos == null)
                {
                    rand = GetAIRandaomNumver();
                    nowTargetPos = (Vector2)transform.position + new Vector2((rand > 50) ? 20 : -20, 0);
                }
                if (EnemyCtrl_fly.MoveToTarget_X(3.0f, (Vector2)nowTargetPos, moveSpeed))
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
                if (rand < AddAIProbNum(aiStateNum_walk)) SetAIState(AISTATE.APPROACH_WALK, 4.0f);
                else if (rand < AddAIProbNum(aiStateNum_dash)) SetAIState(AISTATE.APPROACH_DASH, 4.0f);
                else if (rand < AddAIProbNum(aiStateNum_attack)) SetAIState(AISTATE.ATTACK, 20.0f);
                else if (rand < AddAIProbNum(aiStateNum_fly))
                {
                    StartFlyMode();
                }
                else if(rand<100)
                {
                    SetAIState(AISTATE.WAIT, 4.0f);
                    animator.SetTrigger("rest");
                }
                break;
            case AISTATE.APPROACH_WALK:
                if (EnemyCtrl_fly.MoveToPlayer_X(10.0f, moveSpeed * 0.5f))
                {
                    SetAIState(AISTATE.ATTACK, 20.0f);
                }
                break;
            case AISTATE.APPROACH_DASH:
                if (EnemyCtrl_fly.MoveToPlayer_X(10.0f, moveSpeed))
                {
                    SetAIState(AISTATE.ATTACK, 20.0f);
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
            EnemyCtrl_fly.SetDirectionToPl();
            animator.SetTrigger("At_dash");
            StartAttack();
        }
        else if(rand<AddAIProbNum(aiStateFrontPl_head))
        {
            if (EnemyCtrl_fly.IsPlayerFront())
            {
                animator.SetTrigger("At_head");
                StartAttack();
            }
        }else if (rand < AddAIProbNum(aiStateFrontPl_tail))
        {
            if (!EnemyCtrl_fly.IsPlayerFront())
            {
                animator.SetTrigger("At_tail");
                StartAttack();
            }
        }
    }

    protected override void AIUpdate_notSuitableFLy()
    {
        base.AIUpdate_notSuitableFLy();
        if (IsFixedRandomNumber) return;
        if (FlyMode)
        {
            if (EnemyCtrl_fly.MoveToHigher(FlyHight, MoveSpeedY))
            {
                EndSwichFlyMode();
            }
        }
        else
        {
            if (EnemyCtrl_fly.MoveToLower(FlyHight_land, MoveSpeedY))
            {
                animator.SetTrigger("landing");
                SetRandFiexed(110.0f);
            }
        }
    }

    protected override void AIUpdate_fly_detect()
    {
        base.AIUpdate_fly_detect();
        float rand = GetAIRandaomNumver();
        switch (aiState)
        {
            case AISTATE.AISELECT:
                if (rand < AddAIProbNum(30.0f)) SetAIState(AISTATE.APPROACH_DASH, 3.0f);
                //else if (rand < AddAIProbNum(30.0f)) SetAIState(AISTATE.ESCAPE_DASH, 3.0f);
                else if (rand < AddAIProbNum(30.0f))
                {
                    EndFlyMode();
                }
                break;
            case AISTATE.APPROACH_DASH:
                if (EnemyCtrl_fly.MoveToPlayer_X(10.0f, moveSpeed))
                {
                    SetAIState(AISTATE.WAIT, 1.0f);
                }
                break;
            case AISTATE.ESCAPE_DASH:
                if (nowTargetPos == null)
                {
                    nowTargetPos = EnemyCtrl_fly.GetPlPosition() + new Vector2(10.0f, 0);
                }
                if (EnemyCtrl_fly.MoveToTarget_X_reverse(1.0f,(Vector2)nowTargetPos, moveSpeed))
                {
                    nowTargetPos = null;
                    SetAIState(AISTATE.WAIT, 1.0f);
                }
                break;
        }
    }

    protected override void AIUpdate_fly_attack()
    {
        base.AIUpdate_fly_attack();
    }
    #endregion
}
