using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_sorairoDoragon : EnemyMain
{
    Vector2? nowTargetPos;

    [SerializeField] Vector2 fireVector;
    [SerializeField] float fireLength;
    [SerializeField] animationUtil fire;
    [SerializeField] Transform effectPos;

    int jumpCounter=0;
    int beforeCounter=-1;

    protected override void AIUpdate_undetect()
    {
        base.AIUpdate_undetect();
        float rand = GetAIRandaomNumver();

        switch (aiState)
        {
            case AISTATE.AISELECT:
                if (rand < 40.0f) SetAIState(AISTATE.APPROACH_WALK, 5.0f);
                else if (rand < 80.0f) SetAIState(AISTATE.WAIT, 3.0f);
                else if (rand < 100.0f) SetAIState(AISTATE.ATTACK, 5.0f);
                break;
            case AISTATE.WAIT:
                break;
            case AISTATE.APPROACH_WALK:
                if (nowTargetPos == null)
                {
                    rand = GetAIRandaomNumver();
                    nowTargetPos = (Vector2)transform.position + new Vector2((rand > 50) ? 20 : -20, 0);
                }
                if (enemyCtrl.MoveToTarget_X(3.0f, (Vector2)nowTargetPos, moveSpeed))
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
                if (rand < 40.0f) SetAIState(AISTATE.APPROACH_WALK, 6.0f);
                else if (rand < 80.0f) SetAIState(AISTATE.APPROACH_DASH, 6.0f);
                else if (rand < 95.0f) SetAIState(AISTATE.ATTACK, 4.0f);
                else
                {
                    SetAIState(AISTATE.WAIT, 4.0f);
                    animator.SetTrigger("rest");
                }
                break;
            case AISTATE.APPROACH_WALK:
                if (enemyCtrl.MoveToPlayer_X(10.0f, moveSpeed * 0.5f))
                {
                    SetAIState(AISTATE.ATTACK, 20.0f);
                }
                break;
            case AISTATE.APPROACH_DASH:
                if (enemyCtrl.MoveToPlayer_X(10.0f, moveSpeed))
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
        if (rand < 20.0f)
        {
            enemyCtrl.SetDirectionToPl();
            animator.SetTrigger("attack2");
            StartAttack();
        }
        else if (rand < 40.0f)
        {
            if (enemyCtrl.IsPlayerFront())
            {
                animator.SetTrigger("attack1");
                StartAttack();
            }
        }
        else if (rand < 60.0f)
        {
            if (!enemyCtrl.IsPlayerFront())
            {
                animator.SetTrigger("attack3");
                StartAttack();
            }
        }else if (rand < 80.0f)
        {
            if (enemyCtrl.GetDistancePlayer_X() > 10.0f && !IsFixedRandomNumber) return;
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
        else
        {
            if (enemyCtrl.GetDistancePlayer_X() > 5.0f)
            {
                enemyCtrl.SetDirectionToPl();
                animator.SetTrigger("attack4");
                StartAttack();
            }
        }
    }

    protected override void AIUpdate_mapChenge()
    {
        base.AIUpdate_mapChenge();
        enemyCtrl.MoveToTarget_X(0.0f, mapChengeCtrl.GetNextLoadPosition(),enemyCtrl.MoveSpeed);
    }


    public void FireCreate()
    {
        animationUtil obj = Instantiate(fire, effectPos.position, Quaternion.identity);
        var vec = fireVector;
        vec.x *= Mathf.Sign(-transform.localScale.x);
        obj.SetVelocity(vec);
        obj.SetDedLength(fireLength);
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
