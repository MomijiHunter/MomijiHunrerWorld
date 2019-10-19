using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using aojilu;

public class EnemyMain : MonoBehaviour,ReciveInterFace_damage
{
    protected EnemyController enemyCtrl { get; private set; }
    
    EnemyController.DETECTSTATE detectState { get { return enemyCtrl.DetectState; } set { enemyCtrl.SetDetectState(value); } }
    
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

    protected Animator animator { get { return enemyCtrl.animator; } }
    #region 乱数系
    protected float? randomFixedNumber { get; private set; }//乱数を固定したときの値
    protected bool IsFixedRandomNumber { get { return randomFixedNumber != null; } }
    #endregion

    protected bool attackNow { get; private set; }
    float? fixedSpeed;//nullじゃないなら速度を固定する

    #region MonoBehaviour
    private void Awake()
    {
        enemyCtrl = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (enemyCtrl.IsDeadSelf()) return;
        if (fixedSpeed != null) enemyCtrl.Move_force((float)fixedSpeed);
        PreAIAction();
        AIUpdte();
        DetectUpdate();
    }
    #endregion
    #region AIタイミング関数
    /// <summary>
    /// AI選択の前に行うアクション
    /// </summary>
    void PreAIAction()
    {
        PreAIAction_detect();
    }

    /// <summary>
    /// AISelectの時に別の処理を加える
    /// </summary>
    void AISelectDisturb()
    {
        AISelectDisturb_detect();
    }
    #endregion
    #region detect
    void DetectUpdate()
    {
        switch (detectState)
        {
            case EnemyController.DETECTSTATE.UNDETECT:
                break;
            case EnemyController.DETECTSTATE.PREDETECT:
                break;
            case EnemyController.DETECTSTATE.DETECT:
                enemyCtrl.PredetectUpdate();
                break;
        }
    }

    void PreAIAction_detect()
    {
        switch (detectState)
        {
            case EnemyController.DETECTSTATE.UNDETECT:
                if (enemyCtrl.IsInSight())
                {
                    detectState = EnemyController.DETECTSTATE.DETECT;
                    DetectAction();
                }
                break;
            case EnemyController.DETECTSTATE.PREDETECT:
                if (enemyCtrl.IsInSight())
                {
                    detectState = EnemyController.DETECTSTATE.DETECT;
                    DetectAction();
                }
                break;
            case EnemyController.DETECTSTATE.DETECT:
                break;
        }
    }

    /// <summary>
    /// AISELECTになった時に呼ばれる関数
    /// detectStateの変更が呼ばれるのはこことダメージの時だけ
    /// </summary>
    void AISelectDisturb_detect()
    {
        switch (detectState)
        {
            case EnemyController.DETECTSTATE.UNDETECT:
                break;
            case EnemyController.DETECTSTATE.PREDETECT:
                if (enemyCtrl.IsEndPreDetect())
                {
                    detectState = EnemyController.DETECTSTATE.UNDETECT;
                }
                break;
            case EnemyController.DETECTSTATE.DETECT:
                if (!enemyCtrl.IsInSight())
                {
                    detectState = EnemyController.DETECTSTATE.PREDETECT;
                }
                break;
        }
    }

    /// <summary>
    /// OnReciveDamageで呼ばれる
    /// </summary>
    void ReciveDamage_detect()
    {
        if(detectState==EnemyController.DETECTSTATE.UNDETECT
            || detectState == EnemyController.DETECTSTATE.PREDETECT)
        {
            detectState = EnemyController.DETECTSTATE.DETECT;
            DetectAction();
        }
    }
    #endregion
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
                AIUpdate_undetect();
                break;
            case EnemyController.DETECTSTATE.DETECT:
                AIUpdate_detect();
                break;
        }
    }
    /// <summary>
    /// AIStateがAISELECTになった時に呼ばれる関数
    /// AISELECTの時は常にアニメーションが終了している
    /// </summary>
    void ChengedAISelectAction()
    {
        if (aiState != AISTATE.AISELECT) return;
    }

    protected virtual void AIUpdate_undetect()
    {

    }

    protected virtual void AIUpdate_detect()
    {

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
            AISelectDisturb();
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

    /// <summary>
    /// 敵を発見したときの処理
    /// </summary>
    void DetectAction()
    {
        enemyCtrl.DetectAction();
        SetAIState(AISTATE.WAIT, 3.0f);
    }
    /// <summary>
    /// 攻撃開始
    /// </summary>
    public void StartAttack()
    {
        attackNow = true;
        animator.SetBool("attackEnd", false);
    }

    /// <summary>
    /// 攻撃終了
    /// </summary>
    public void EndAttack()
    {
        attackNow = false;
        animator.SetBool("attackEnd", true);
        ResetFixedSpeed();
        enemyCtrl.StopMove();
        SetAIState(AISTATE.WAIT, 1.0f);
    }

    public void SetEndAttack(float f)
    {
        StartCoroutine( enemyCtrl.WaitTimeAction(f, () => EndAttack()));
    }
    #endregion
    #region 動作関連
    public void SetFixedSpeed(float speed)
    {
        fixedSpeed= speed * Mathf.Sign(-transform.localScale.x);
    }

    public void ResetFixedSpeed()
    {
        fixedSpeed = null;
    }
    #endregion
    #region 乱数関連
    protected float GetAIRandaomNumver()
    {
        float result = 0.0f;
        if (randomFixedNumber == null)
        {
            result = Random.Range(0, 100);
        }
        else
        {
            result = (float)randomFixedNumber;
        }
        return result;
    }

    protected void SetRandFiexed(float f)
    {
        randomFixedNumber = f;
    }

    protected void ResetRandFiexed()
    {
        randomFixedNumber = null;
    }
    
    #endregion

    /// <summary>
    /// ダメージを食らったときにメッセージで呼び出される
    /// </summary>
    public void OnReciveDamage()
    {
        ReciveDamage_detect();
    }

}
