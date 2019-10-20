using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_tutiiroDoragon : EnemyMain
{
    Vector2 nowTarget;


    [SerializeField] float aiStateNum_walk_ud;
    [SerializeField] float aiStateNum_wait_ud;
    float AiStateNum_wait_ud { get { return aiStateNum_walk_ud + aiStateNum_wait_ud; } }
    [SerializeField] float aiStateNum_rest_ud;
     float AiStateNum_rest_ud { get { return AiStateNum_wait_ud + aiStateNum_rest_ud; } }

    [SerializeField] float aiStateNum_walk;
    [SerializeField] float aiStateNum_dash;
     float AiStateNum_dash { get { return aiStateNum_walk + aiStateNum_dash; } }
    [SerializeField] float aiStateNum_attack;
     float AiStateNum_attack { get { return AiStateNum_dash+ aiStateNum_attack; } }
    [SerializeField] float aiStateNum_escape;
     float AiStateNum_escape { get { return AiStateNum_attack + aiStateNum_escape; } }

    [SerializeField] float aiStateFrontPl_dash;
    [SerializeField] float aiStateFrontPl_head;
     float AiStateFrontPl_head { get { return aiStateFrontPl_dash + aiStateFrontPl_head; } }
    [SerializeField] float aiStateFrontPl_tail;
     float AiStateFrontPl_tail { get { return AiStateFrontPl_head + aiStateFrontPl_tail; } }

    protected override void AIUpdate_undetect()
    {
        base.AIUpdate_undetect();
        float rand = GetAIRandaomNumver();
        switch (aiState)
        {
            case AISTATE.AISELECT:
                ResetRandFiexed();

                if (rand < aiStateNum_walk_ud) SetAIState(AISTATE.APPROACH_WALK, 5.0f);
                else if (rand < AiStateNum_wait_ud) SetAIState(AISTATE.WAIT, 5.0f);
                else if(rand<AiStateNum_rest_ud) SetAIState(AISTATE.ATTACK, 3.0f);
                break;
            case AISTATE.WAIT:
                break;
            case AISTATE.APPROACH_WALK:
                if (!IsFixedRandomNumber)
                {
                    SetRandFiexed(GetAIRandaomNumver());
                    nowTarget = transform.position + new Vector3((RandomFixedNumber>50)?20:-20,0);
                }
                if (EnemyCtrl.MoveToTarget_X(1.0f, nowTarget, EnemyCtrl.MoveSpeed * 0.5f))
                {
                    SetAIState(AISTATE.WAIT, 2.0f);
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
                if (rand < aiStateNum_walk) SetAIState(AISTATE.APPROACH_WALK, 5.0f);
                else if (rand < AiStateNum_dash) SetAIState(AISTATE.APPROACH_DASH, 5.0f);
                else if (rand < AiStateNum_attack) SetAIState(AISTATE.ATTACK, 20.0f);
                else if (rand < AiStateNum_escape) SetAIState(AISTATE.ESCAPE_DASH, 4.0f);
                else
                {
                    SetAIState(AISTATE.WAIT, 3.0f);
                    animator.SetTrigger("rest");
                }
                break;
            case AISTATE.APPROACH_WALK:
                if (EnemyCtrl.MoveToPlayer_X(10.0f, EnemyCtrl.MoveSpeed * 0.5f))
                {
                    SetAIState(AISTATE.ATTACK, 4.0f);
                }
                break;
            case AISTATE.APPROACH_DASH:
                if (EnemyCtrl.MoveToPlayer_X(10.0f, EnemyCtrl.MoveSpeed ))
                {
                    SetAIState(AISTATE.ATTACK, 4.0f);
                }
                break;
            case AISTATE.ESCAPE_DASH:
                if(EnemyCtrl.EscapeToPlayer_X(60.0f, EnemyCtrl.MoveSpeed))
                {
                    SetAIState(AISTATE.WAIT, 3.0f);
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
        if (rand < aiStateFrontPl_dash)
        {
            EnemyCtrl.SetDirectionToPl();
            animator.SetTrigger("attack2");
            StartAttack();
        }
        else if (rand < AiStateFrontPl_head)
        {
            if (!EnemyCtrl.IsPlayerFront()) return;
            animator.SetTrigger("attack1");
            StartAttack();
        }
        else if (rand < AiStateFrontPl_tail)
        {
            if (EnemyCtrl.IsPlayerFront()) return;
            animator.SetTrigger("attack3");
            StartAttack();
        }
    }
}
