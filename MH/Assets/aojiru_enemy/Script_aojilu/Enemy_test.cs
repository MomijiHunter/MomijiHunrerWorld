using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace aojilu
{
    public class Enemy_test : EnemyBase
    {

        [SerializeField] float aiStateNum_walk;
        [SerializeField] float aiStateNum_dash;

        protected override void AIAction()
        {
            float rand = Random.Range(0,100);
            switch (aiState)
            {
                case AISTATE.AISELECT:
                    if (rand < aiStateNum_walk) SetAIState(AISTATE.APPROACH_WALK, 3.0f);
                    else if (rand < aiStateNum_walk+aiStateNum_dash) SetAIState(AISTATE.APPROACH_DASH, 3.0f);
                    else SetAIState(AISTATE.WAIT,1.0f);
                    break;
                case AISTATE.APPROACH_WALK:
                    if (MoveToPlayer_X(2.0f, moveSpeed*0.5f))
                    {
                        SetAIState(AISTATE.ATTACK, 2.0f);
                    }
                    break;
                case AISTATE.APPROACH_DASH:
                    if (MoveToPlayer_X(2.0f, moveSpeed))
                    {
                        SetAIState(AISTATE.ATTACK, 2.0f);
                    }
                    break;
                case AISTATE.ATTACK:
                    Attack1();
                    break;
                case AISTATE.WAIT:
                    break;
            }
        }

        void Attack1()
        {
            animator.SetTrigger("Attack");
            SetAIState(AISTATE.WAIT, 2.0f);
        }
    }
}
