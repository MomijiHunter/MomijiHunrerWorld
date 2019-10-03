using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class Enemy_zakoDoragon : EnemyBase
    {
        [SerializeField] bool attackMode = false;

        [SerializeField] float aiStateNum_walk_ud;
        [SerializeField] float aiStateNum_wait_ud;
        [SerializeField] float aiStateNum_rest_ud;


        [SerializeField] float aiStateNum_walk;
        [SerializeField] float aiStateNum_dash;
        [SerializeField] float aiStateNum_attack;
        [SerializeField] float aiStateNum_escape;

        [SerializeField] float aiStateFrontPl_dash;
        [SerializeField] float aiStateFrontPl_head;
        [SerializeField] float aiStateFrontPl_tail;

        float? randFixed = null;
        Vector2? nowTargetPos = null;
        protected override void CheckAction()
        {
            if (attackMode)
            {
                base.CheckAction();
            }
            else
            {
                if (detectState == DETECTSTATE.DETECT)
                {
                    if (!detectCheckArea.InternalTarget) detectState = DETECTSTATE.UNDETECT;
                }
            }
        }

        protected override void DeadAction()
        {
            base.DeadAction();
            if (stateInfo.IsTag("Dead")) return;
            StartCoroutine(WaitTime(10.0f,() => Destroy(this.gameObject)));
            MakeDeadItem();
        }

        protected override void AIAction_undetect()
        {
            base.AIAction_undetect();

            float rand = (randFixed == null) ? Random.Range(0, 100) : (float)randFixed;

            if (detectState == DETECTSTATE.DETECT)
            {
                SetAIState(AISTATE.WAIT, 3.0f);
                SetAIIndex(1);
                return;
            }
            switch (aiState)
            {
                case AISTATE.AISELECT:
                    if (rand < aiStateNum_walk_ud) SetAIState(AISTATE.APPROACH_WALK, 5.0f);
                    else if (rand < aiStateNum_walk_ud + aiStateNum_wait_ud) SetAIState(AISTATE.WAIT, 3.0f);
                    else if (rand < aiStateNum_walk_ud + aiStateNum_wait_ud+aiStateNum_rest_ud) SetAIState(AISTATE.ATTACK, 3.0f);
                    break;
                case AISTATE.WAIT:
                    nowTargetPos = null;
                    break;
                case AISTATE.APPROACH_WALK:
                    if (nowTargetPos == null)
                    {
                        rand = Random.Range(0, 100);
                        nowTargetPos = (Vector2)transform.position + new Vector2((rand > 50) ? 20 : -20, 0);
                    }
                    if (MoveToTarget(5.0f, (Vector2)nowTargetPos, moveSpeed * 0.5f))
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

        protected override void AIAction()
        {
            base.AIAction(); float rand = (randFixed == null) ? Random.Range(0, 100) : (float)randFixed;
            switch (aiState)
            {
                case AISTATE.AISELECT:
                    if (detectState == DETECTSTATE.UNDETECT)
                    {
                        SetAIIndex(0);
                        return;
                    }

                    if (rand < aiStateNum_walk) SetAIState(AISTATE.APPROACH_WALK, 6.0f);
                    else if (rand < aiStateNum_walk + aiStateNum_dash) SetAIState(AISTATE.APPROACH_DASH, 6.0f);
                    else if (rand < aiStateNum_walk + aiStateNum_dash + aiStateNum_attack) SetAIState(AISTATE.ATTACK, 4.0f);
                    else if (rand < aiStateNum_walk + aiStateNum_dash + aiStateNum_attack + aiStateNum_escape) SetAIState(AISTATE.ESCAPE_DASH, 4.0f);
                    else
                    {
                        SetAIState(AISTATE.WAIT, 3.0f);
                        animator.SetTrigger("rest");
                    }
                    break;
                case AISTATE.APPROACH_WALK:
                    if (MoveToPlayer_X(10.0f, moveSpeed * 0.5f))
                    {
                        SetAIState(AISTATE.ATTACK, 4.0f);
                    }
                    break;
                case AISTATE.APPROACH_DASH:
                    if (MoveToPlayer_X(10.0f, moveSpeed))
                    {
                        SetAIState(AISTATE.ATTACK, 4.0f);
                    }
                    break;
                case AISTATE.ESCAPE_DASH:
                    if (MoveToPlayer_X(60.0f, moveSpeed, isEscape: true)) SetAIState(AISTATE.WAIT, 3.0f);
                    break;
                case AISTATE.ATTACK:
                    if (stateInfo.IsTag("attack") && randFixed == null) SetAIState(AISTATE.AISELECT, 1.0f);
                    if (rand < aiStateFrontPl_dash)
                    {
                        SetDirectionToPl();
                        animator.SetTrigger("attack2");
                        SetAIState(AISTATE.WAIT, 20.0f);
                    }
                    else if (rand < aiStateFrontPl_dash + aiStateFrontPl_head)
                    {
                        if (!IsPlayerFront()) return;
                        animator.SetTrigger("attack1");
                        SetAIState(AISTATE.WAIT, 20.0f);
                    }
                    else if (rand < aiStateFrontPl_dash + aiStateFrontPl_head + aiStateFrontPl_tail)
                    {
                        if (IsPlayerFront()) return;
                        animator.SetTrigger("attack3");
                        SetAIState(AISTATE.WAIT, 20.0f);
                    }
                    break;
                case AISTATE.WAIT:
                    break;
            }
        }
    }
}
