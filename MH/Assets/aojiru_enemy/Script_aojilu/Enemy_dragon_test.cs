using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class Enemy_dragon_test : EnemyBase
    {

        [SerializeField] float aiStateNum_walk_ud;
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

        [SerializeField] float aiStateOther_upFire;//特殊状況の時に呼ばれる可能性

        //float? randFixed = null;

        int counter;

        Vector2? nowTargetPos = null;

        [SerializeField] animationUtil fire;
        [SerializeField] Fire_sorairo_bunretu fireBunretu;
        [SerializeField] Vector2 fireVector;
        [SerializeField] float fireLength;


        protected override void AIAction_undetect()
        {
            base.AIAction_undetect();

            //float rand = (randFixed == null) ? Random.Range(0, 100) : (float)randFixed;
            float rand = GetAIRandaomNumver();

            if (detectState == DETECTSTATE.DETECT)
            {
                SetAIState(AISTATE.WAIT, 3.0f);
                SetAIIndex(1);
                return;
            }
            switch (aiState)
            {
                case AISTATE.AISELECT:
                    if (chengeAreaEnable) SetAIState(AISTATE.MAPCHENGE,30.0f);
                    else if (rand < aiStateNum_walk_ud) SetAIState(AISTATE.APPROACH_WALK, 5.0f);
                    else if (rand < aiStateNum_walk_ud + aiStateNum_wait_ud) SetAIState(AISTATE.WAIT, 3.0f);
                    else if (rand < aiStateNum_walk_ud + aiStateNum_wait_ud + aiStateNum_rest_ud) SetAIState(AISTATE.ATTACK, 5.0f);
                    break;
                case AISTATE.WAIT:
                    nowTargetPos = null;
                    break;
                case AISTATE.ATTACK:
                    animator.SetTrigger("rest");
                    SetAIState(AISTATE.WAIT,5.0f);
                    break;
                case AISTATE.APPROACH_WALK:
                    if (nowTargetPos == null)
                    {
                        rand = Random.Range(0, 100);
                        nowTargetPos = (Vector2)transform.position + new Vector2((rand > 50) ? 20 : -20, 0);
                    }
                    if (MoveToTarget_X(5.0f,(Vector2) nowTargetPos, moveSpeed * 0.5f))
                    {
                        SetAIState(AISTATE.WAIT, 2.0f);
                    }
                    break;
                case AISTATE.MAPCHENGE:
                    MoveToTarget_X(1.0f, (Vector2)nextLordObj.transform.position, moveSpeed);
                    break;
            }
        }

        protected override void AIAction()
        {
            base.AIAction();

            //float rand = (randFixed==null)?Random.Range(0, 100):(float)randFixed;
            float rand = GetAIRandaomNumver();
            switch (aiState)
            {
                case AISTATE.AISELECT:
                    if (chengeAreaEnable)
                    {
                        detectState = DETECTSTATE.UNDETECT;
                        preDetectTime = Time.fixedTime;
                    }

                    if (detectState == DETECTSTATE.UNDETECT)
                    {
                        SetAIIndex(0);
                        return;
                    }

                    if (rand < aiStateNum_walk) SetAIState(AISTATE.APPROACH_WALK, 6.0f);
                    else if (rand < aiStateNum_walk + aiStateNum_dash) SetAIState(AISTATE.APPROACH_DASH, 6.0f);
                    else if (rand < aiStateNum_walk + aiStateNum_dash+aiStateNum_attack) SetAIState(AISTATE.ATTACK, 4.0f);
                    else
                    {
                        SetAIState(AISTATE.WAIT, 4.0f);
                        animator.SetTrigger("rest");
                    }
                    break;
                case AISTATE.APPROACH_WALK:
                    if (UpPlTime > 5.0f)
                    {
                        if (MoveToPlayer_X(1.0f, moveSpeed*0.5f))
                        {
                            SetAIState(AISTATE.ATTACK, 4.0f);
                        }
                    }
                    else if (MoveToPlayer_X(10.0f, moveSpeed * 0.5f))
                    {
                        SetAIState(AISTATE.ATTACK, 4.0f);
                    }
                    break;
                case AISTATE.APPROACH_DASH:
                    if (UpPlTime > 5.0f)
                    {
                        if(MoveToPlayer_X(1.0f, moveSpeed))
                        {
                            SetAIState(AISTATE.ATTACK, 4.0f);
                        }
                    }
                    else if (MoveToPlayer_X(10.0f, moveSpeed))
                    {
                        SetAIState(AISTATE.ATTACK, 4.0f);
                    }
                    break;
                case AISTATE.ATTACK:
                    if (stateInfo.IsTag("attack")&&!IsFixedRandomNumber) SetAIState(AISTATE.AISELECT,1.0f);

                    if (UpPlTime>5.0f&& rand<aiStateOther_upFire)
                    {
                        if (GetDistancePlayer_X() > 2.0f)
                        {
                            SetAIState(AISTATE.APPROACH_DASH, 3.0f);
                            break;
                        }
                        animator.SetTrigger("attack5");
                        SetAIState(AISTATE.WAIT, 5.0f);
                        ResetUpPlTime();
                    }
                    else if (rand < aiStateFrontPl_dash)
                    {
                        Attack2();
                    }else if (rand < aiStateFrontPl_dash + aiStateFrontPl_head)
                    {
                        if(IsPlayerFront()) Attack1();
                    }else if (rand < aiStateFrontPl_dash + aiStateFrontPl_head + aiStateFrontPl_tail)
                    {
                        if (!IsPlayerFront()) Attack3();
                    }else if(rand< aiStateFrontPl_dash + aiStateFrontPl_head + aiStateFrontPl_tail + aiStateFrontPl_jump)
                    {
                        if (GetDistancePlayer_X() > 10.0f&&!IsFixedRandomNumber) return;
                        if (counter >= 3)
                        {
                            animator.SetTrigger("fall1");
                            SetAIState(AISTATE.WAIT, 3.0f);
                            ResetRandFiexed();
                            counter = 0;
                        }
                        else
                        {
                            animator.SetTrigger("jump");
                            SetRandFiexed(rand);
                            //randFixed = rand;
                            ExtendStateTime(3.0f);
                        }
                    }
                    else if(rand < aiStateFrontPl_dash + aiStateFrontPl_head + aiStateFrontPl_tail + aiStateFrontPl_jump+
                        aiStateFrontPl_fire)
                    {
                        if (GetDistancePlayer_X() < 5.0f) return;
                        SetDirectionToPl();
                        animator.SetTrigger("attack4");
                        SetAIState(AISTATE.WAIT, 3.0f);
                    }
                    break;
                case AISTATE.WAIT:
                    break;
            }
        }

        protected override void DamageDetectAction()
        {
            base.DamageDetectAction();
            SetDirectionToPl();
            StopMove();

        }

        void Attack1()
        {
            animator.SetTrigger("attack1");
            SetAIState(AISTATE.WAIT, 20.0f);
        }

        void Attack2()
        {
            SetDirectionToPl();
            animator.SetTrigger("attack2");
            SetAIState(AISTATE.WAIT, 20.0f);
        }

        void Attack3()
        {
            animator.SetTrigger("attack3");
            SetAIState(AISTATE.WAIT, 20.0f);
        }
        
        void AddCounter()
        {
            counter++;
        }

        public void FireCreate()
        {
            animationUtil obj= Instantiate(fire, effectPos.position, Quaternion.identity) ;
            var vec = fireVector;
            vec.x *= Mathf.Sign(-transform.localScale.x);
            obj.SetVelocity(vec);
            obj.SetDedLength(fireLength);
        }

        public void FireCreate_buntetu()
        {
            Instantiate(fireBunretu, effectPos.position, Quaternion.identity);
        }
    }
}
