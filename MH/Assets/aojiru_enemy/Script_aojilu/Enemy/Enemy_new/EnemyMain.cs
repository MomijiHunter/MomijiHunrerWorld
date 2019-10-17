using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMain : MonoBehaviour
{
    EnemyController enemyController;

    EnemyController.DETECTSTATE detectState { get { return enemyController.DetectState; } set { enemyController.SetDetectState(value); } }
    

    public enum AISTATE
    {
        AISELECT,
        APPROACH_WALK,
        APPROACH_DASH,
        ESCAPE_DASH,
        WAIT,
        ATTACK
    }
    protected AISTATE aiState;
    public AISTATE AiState { get { return aiState; } }

    float aiStartTime;//今のステイとになった時刻
    float aiWaitLength;//今のステイとを続ける時間

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        AIUpdte();
        DetectUpdate();
    }

    void DetectUpdate()
    {
        switch (detectState)
        {
            case EnemyController.DETECTSTATE.UNDETECT:
                break;
            case EnemyController.DETECTSTATE.PREDETECT:
                break;
            case EnemyController.DETECTSTATE.DETECT:
                enemyController.PredetectUpdate();
                if (enemyController.IsChengedDetectState)
                {
                    enemyController.DetectAction();
                    SetAIState(AISTATE.WAIT, 3.0f);
                }
                break;
        }
    }

    /// <summary>
    /// AISELECTになった時に呼ばれる関数
    /// detectStateの変更が呼ばれるのはこことダメージの時だけ
    /// </summary>
    void ChengeAIStateAction_detect()
    {
        switch (detectState)
        {
            case EnemyController.DETECTSTATE.UNDETECT:
                if (enemyController.IsInSight())
                {
                    detectState = EnemyController.DETECTSTATE.DETECT;
                }
                break;
            case EnemyController.DETECTSTATE.PREDETECT:
                if (enemyController.IsEndPreDetect())
                {
                    detectState = EnemyController.DETECTSTATE.UNDETECT;
                }
                else if (enemyController.IsInSight())
                {
                    detectState = EnemyController.DETECTSTATE.DETECT;
                }
                break;
            case EnemyController.DETECTSTATE.DETECT:
                if (!enemyController.IsInSight())
                {
                    detectState = EnemyController.DETECTSTATE.PREDETECT;
                }
                break;
        }
    }
    #region AI

    void AIUpdte()
    {
        if (Time.fixedTime > aiStartTime + aiWaitLength)
        {
            SetAIState(AISTATE.AISELECT, 1.0f);
            return;
        }

        switch (detectState)
        {
            case EnemyController.DETECTSTATE.UNDETECT:
            case EnemyController.DETECTSTATE.PREDETECT:
                AIAction_unDetect();
                break;
            case EnemyController.DETECTSTATE.DETECT:
                AIAction_detect();
                break;
        }
    }

    protected virtual void AIAction_unDetect()
    {

    }
    protected virtual void AIAction_detect()
    {

    }

    /// <summary>
    /// AIStateがAISELECTになった時に呼ばれる関数
    /// AISELECTの時は常にアニメーションが終了している
    /// </summary>
    void ChengedAISelectAction()
    {
        if (aiState != AISTATE.AISELECT) return;
        ChengeAIStateAction_detect();
    }
    #endregion
    #region AIState
    /// <summary>
    /// aiStateの変更
    /// </summary>
    /// <param name="state"></param>
    /// <param name="t"></param>
    protected void SetAIState(AISTATE state, float t)
    {
        aiStartTime = Time.fixedTime;
        aiWaitLength = t;
        aiState = state;
        if (state == AISTATE.AISELECT)
        {
            ChengedAISelectAction();
        }
    }

    /// <summary>
    /// aistateの時間の延長
    /// </summary>
    /// <param name="t"></param>
    protected void ExtendStateTime(float t)
    {
        aiStartTime = Time.fixedTime;
        aiWaitLength = t;
    }
    #endregion
}
