using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace aojilu
{
    public class Enemy_akiDOragon : EnemyBase
    {
        [SerializeField] float aiStateNum_walk;
        [SerializeField] float aiStateNum_run;
        [SerializeField] float aiStateNum_attack;


        [SerializeField] float aiStateFrontPl_dash;
        [SerializeField] float aiStateFrontPl_head;
        [SerializeField] float aiStateFrontPl_tail;

        protected override void AIAction_undetect()
        {
            base.AIAction_undetect();
            float rand = GetAIRandaomNumver();

            if (DetectState == DETECTSTATE.DETECT)
            {
                SetAIState(AISTATE.WAIT, 3.0f);
                //SetAIIndex(1);
                return;
            }
        }

        protected override void AIAction_detect()
        {
            base.AIAction_detect();

            float rand = GetAIRandaomNumver();
            switch (aiState)
            {
                case AISTATE.AISELECT:
                    /*if (chengeAreaEnable)
                    {
                        detectState = DETECTSTATE.UNDETECT;
                        preDetectTime = Time.fixedTime;
                    }

                    if (detectState == DETECTSTATE.UNDETECT)
                    {
                        SetAIIndex(0);
                        return;
                    }*/

                    if (rand < aiStateNum_walk) SetAIState(AISTATE.APPROACH_WALK, 6.0f);
                    else if (rand < aiStateNum_walk + aiStateNum_run) SetAIState(AISTATE.APPROACH_DASH, 6.0f);
                    else if (rand < aiStateNum_walk + aiStateNum_run + aiStateNum_attack) SetAIState(AISTATE.ATTACK, 4.0f);
                    else
                    {
                        SetAIState(AISTATE.WAIT, 4.0f);
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
                case AISTATE.ATTACK:
                    CaseAttack(rand);
                    break;
                case AISTATE.WAIT:
                    break;
            }
        }

        void CaseAttack(float rand)
        {
            if (stateInfo.IsTag("attack") && !IsFixedRandomNumber) SetAIState(AISTATE.AISELECT, 1.0f);

            if (rand < aiStateFrontPl_dash)
            {
                SetDirectionToPl();
                animator.SetTrigger("At_dash");
                SetAIState(AISTATE.WAIT, 20.0f);
            }else if (rand < aiStateFrontPl_dash + aiStateFrontPl_head)
            {
                if (IsPlayerFront())
                {
                    animator.SetTrigger("At_head");
                    SetAIState(AISTATE.WAIT, 20.0f);
                }
            }else if (rand < aiStateFrontPl_dash + aiStateFrontPl_head + aiStateFrontPl_tail)
            {
                if (!IsPlayerFront())
                {
                    animator.SetTrigger("At_tail");
                    SetAIState(AISTATE.WAIT, 20.0f);
                }
            }
        }
    }
}
