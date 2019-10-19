using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_testNew : EnemyMain
{

    Vector2 nowTarget;

    protected override void AIUpdate_undetect()
    {
        base.AIUpdate_undetect();
        float rand = GetAIRandaomNumver();
        switch (aiState)
        {
            case AISTATE.AISELECT:
                ResetRandFiexed();

                if (rand < 40) SetAIState(AISTATE.APPROACH_WALK, 5.0f);
                else if (rand < 80) SetAIState(AISTATE.WAIT, 5.0f);
                else SetAIState(AISTATE.ATTACK, 5.0f);
                break;
            case AISTATE.WAIT:
                break;
            case AISTATE.APPROACH_WALK:
                if (!IsFixedRandomNumber)
                {
                    SetRandFiexed(GetAIRandaomNumver());
                    nowTarget = transform.position + new Vector3((randomFixedNumber>50)?20:-20,0);
                }
                if (enemyCtrl.MoveToTarget_X(1.0f, nowTarget, enemyCtrl.MoveSpeed * 0.5f))
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
                if (rand < 40) SetAIState(AISTATE.APPROACH_WALK, 5.0f);
                else if (rand < 80) SetAIState(AISTATE.APPROACH_DASH, 5.0f);
                else
                {
                    SetAIState(AISTATE.WAIT,5.0f);
                }
                break;
            case AISTATE.APPROACH_WALK:
                if (enemyCtrl.MoveToPlayer_X(10.0f, enemyCtrl.MoveSpeed * 0.5f))
                {
                    SetAIState(AISTATE.ATTACK, 4.0f);
                }
                break;
            case AISTATE.APPROACH_DASH:
                if (enemyCtrl.MoveToPlayer_X(10.0f, enemyCtrl.MoveSpeed ))
                {
                    SetAIState(AISTATE.ATTACK, 4.0f);
                }
                break;
            case AISTATE.ATTACK:
                AttackUpdate();
                break;
            case AISTATE.WAIT:
                break;
        }
    }
    
    void AttackUpdate()
    {
        if (attackNow) return;
        float rand = GetAIRandaomNumver();
        if (rand < 40)
        {
            enemyCtrl.SetDirectionToPl();
            animator.SetTrigger("attack2");
            StartAttack();
        }else if (rand < 70)
        {
            if (!enemyCtrl.IsPlayerFront()) return;
            animator.SetTrigger("attack1");
            StartAttack();
        }
        else if (rand < 100)
        {
            if (enemyCtrl.IsPlayerFront()) return;
            animator.SetTrigger("attack3");
            StartAttack();
        }
    }
}
