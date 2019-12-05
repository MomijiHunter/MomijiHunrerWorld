using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using aojilu;

public class EnemyMain : MonoBehaviour,ReciveInterFace_damage,ReciveInterFace_mapChenge
{
    #region キャッシュ
    protected EnemyController EnemyCtrl { get; private set; }
    protected MonstarMapChengeCtrl mapChengeCtrl { get; private set; }
    protected Animator animator { get { return EnemyCtrl.animator; } }
    #endregion
    #region AI
    public enum AISTATE
    {
        AISELECT,
        APPROACH_WALK,
        APPROACH_DASH,
        ESCAPE_DASH,
        WAIT,
        ATTACK,
        MAPCHENGE
    }
    [SerializeField]protected AISTATE aiState;
    public AISTATE AiState { get { return aiState; } }
    float aiStartTime;//今のステイとになった時刻
    float aiWaitLength;//今のステイとを続ける時間

    protected bool StopAIAction { get; private set; }
    #region 

    List<AIActionOrganaizer> aiOrgList = new List<AIActionOrganaizer>();
    protected AIActionOrganaizer aiUpdateOrg_mapChenge = new AIActionOrganaizer();
    protected AIActionOrganaizer aiUpdateOrg_undetect = new AIActionOrganaizer();
    protected AIActionOrganaizer aiUpdateOrg_attack = new AIActionOrganaizer();
    protected AIActionOrganaizer aiUpdateOrg_detect = new AIActionOrganaizer();
    #endregion

    float aiProbabilityNumber;//AIの確率処理に使う番号
    #endregion
    #region ショートカット
    EnemyController.DETECTSTATE detectState { get { return EnemyCtrl.DetectState; } set { EnemyCtrl.SetDetectState(value); } }
    protected float moveSpeed { get { return EnemyCtrl.MoveSpeed; } }
    public bool MapChengeEnable { get { return mapChengeCtrl!=null&&mapChengeCtrl.MapChengeEnable; } }
    #endregion
    #region 乱数系
    [SerializeField] float? randomFixedNumber = null;//乱数を固定したときの値
    protected float? RandomFixedNumber { get { return randomFixedNumber; } }//乱数を固定したときの値
    protected bool IsFixedRandomNumber { get { return randomFixedNumber != null; } }
    #endregion

    protected bool attackNow { get; private set; }//攻撃中かどうか
    float? fixedSpeed;//nullじゃないなら速度を固定する

    #region MonoBehaviour
    protected virtual void Awake()
    {
        EnemyCtrl = GetComponent<EnemyController>();
        mapChengeCtrl = GetComponent<MonstarMapChengeCtrl>();
        InitAIUpdateAction();
        StopAIAction = false;
    }

    private void Update()
    {
        if (EnemyCtrl.IsDeadSelf()) return;
        if (fixedSpeed != null) EnemyCtrl.Move_force((float)fixedSpeed);
        AIUpdte();
        DetectUpdate();
    }
    #endregion
    #region AIタイミング関数
    #region preAI
    /// <summary>
    /// AI選択の前に行うアクション
    /// </summary>
    void PreAIAction()
    {
        PreAIAction_detect();
        PreAIAction_aiState();
    }


    void PreAIAction_detect()
    {
        switch (detectState)
        {
            case EnemyController.DETECTSTATE.UNDETECT:
                if (EnemyCtrl.IsInSight())
                {
                    detectState = EnemyController.DETECTSTATE.DETECT;
                    DetectAction();
                }
                break;
            case EnemyController.DETECTSTATE.PREDETECT:
                if (EnemyCtrl.IsInSight())
                {
                    detectState = EnemyController.DETECTSTATE.DETECT;
                    DetectAction();
                }
                break;
            case EnemyController.DETECTSTATE.DETECT:
                break;
        }
    }

    void PreAIAction_aiState()
    {
        ResetAiProbNum();
    }
    #endregion
    #region disturb
    /// <summary>
    /// AISelectの時に別の処理を加える
    /// </summary>
    void AISelectDisturb()
    {
        AISelectDisturb_detect();
        AISelectDisturb_aiOrg();
        AISelectDisturb_aiState_mapChenge();
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
                if (EnemyCtrl.IsEndPreDetect())
                {
                    detectState = EnemyController.DETECTSTATE.UNDETECT;
                }
                break;
            case EnemyController.DETECTSTATE.DETECT:
                if (!EnemyCtrl.IsInSight())
                {
                    detectState = EnemyController.DETECTSTATE.PREDETECT;
                }
                break;
        }
    }


    /// <summary>
    /// AIStateがAISELECTになった時に呼ばれる関数
    /// aiOrgの遷移処理を記述
    /// </summary>
    virtual protected void AISelectDisturb_aiOrg()
    {

    }

    /// <summary>
    /// マップ変更状態への遷移処理
    /// </summary>
    void AISelectDisturb_aiState_mapChenge()
    {
        if (mapChengeCtrl != null && mapChengeCtrl.MapChengeEnable)
        {
            SetAIState(AISTATE.MAPCHENGE, 60.0f);
        }
    }
    #endregion
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
                EnemyCtrl.PredetectUpdate();
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
        PreAIAction();

        if (StopAIAction) return;

        if (Time.fixedTime > aiStartTime + aiWaitLength)//呼び出し時の時間をオーバーした場合はAISelectに戻る
        {
            SetAIState(AISTATE.AISELECT, 1.0f);
            return;
        }

        if (mapChengeCtrl!=null&&aiState == AISTATE.MAPCHENGE)//map変更AI時
        {
            aiUpdateOrg_mapChenge.GetNowAction().Invoke();
            return;
        }

        switch (detectState)
        {
            case EnemyController.DETECTSTATE.UNDETECT:
            case EnemyController.DETECTSTATE.PREDETECT:
                //AIUpdate_undetect();//未発見
                aiUpdateOrg_undetect.GetNowAction().Invoke();
                break;
            case EnemyController.DETECTSTATE.DETECT:
                if (aiState == AISTATE.ATTACK)//攻撃
                {
                    if (attackNow) return;
                    aiUpdateOrg_attack.GetNowAction().Invoke();
                }
                else
                {
                    //非攻撃時、発見状態
                    aiUpdateOrg_detect.GetNowAction().Invoke();
                }
                break;
        }
    }
    #region AIUpdate
    protected virtual void AIUpdate_undetect()
    {

    }

    protected virtual void AIUpdate_detect()
    {

    }

    protected virtual void AIUpdate_attack()
    {

    }

    protected virtual void AIUpdate_mapChenge()
    {
    }
    #endregion
    protected virtual void InitAIUpdateAction()
    {
        aiOrgList.Add(aiUpdateOrg_attack);
        aiOrgList.Add(aiUpdateOrg_undetect);
        aiOrgList.Add(aiUpdateOrg_detect);
        aiOrgList.Add(aiUpdateOrg_mapChenge);

        aiUpdateOrg_attack.AddAction("default", ()=>AIUpdate_attack());
        aiUpdateOrg_attack.SetNowAction("default");
        aiUpdateOrg_detect.AddAction("default", ()=>AIUpdate_detect());
        aiUpdateOrg_detect.SetNowAction("default");
        aiUpdateOrg_undetect.AddAction("default", () => AIUpdate_undetect());
        aiUpdateOrg_undetect.SetNowAction("default");
        aiUpdateOrg_mapChenge.AddAction("default", () => AIUpdate_mapChenge());
        aiUpdateOrg_mapChenge.SetNowAction("default");
    }

    /// <summary>
    /// AIOrgの一括登録
    /// 単一処理のみ登録可
    /// </summary>
    protected void AIOrgAddAction(string key,UnityAction ua)
    {
        foreach(var act in aiOrgList)
        {
            act.AddAction(key,ua);
        }
    }

    /// <summary>
    /// AIOrgの一括変更
    /// </summary>
    /// <param name="key"></param>
    protected void AIOrgSetNowAction(string key)
    {
        foreach(var act in aiOrgList)
        {
            act.SetNowAction(key);
        }
    }
    
    /// <summary>
    /// trueならAI関数の呼び出しをやめる
    /// </summary>
    /// <param name="flag"></param>
    protected void SetAIStop(bool flag)
    {
        StopAIAction = flag;
        EnemyCtrl.StopMove();
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
    /// 確率の値を追加し、追加後の値を返す
    /// </summary>
    protected float AddAIProbNum(float f)
    {
        aiProbabilityNumber += f;
        return aiProbabilityNumber;
    }

    void ResetAiProbNum()
    {
        aiProbabilityNumber = 0;
    }
    /// <summary>
    /// 敵を発見したときの処理
    /// </summary>
    void DetectAction()
    {
        EnemyCtrl.DetectAction();
        SetAIState(AISTATE.WAIT, 3.0f);
    }
    public void ReciveMapChenge_aiState()
    {
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
    public virtual void EndAttack()
    {
        attackNow = false;
        animator.SetBool("attackEnd", true);
        ResetFixedSpeed();
        EnemyCtrl.StopMove();
        SetAIState(AISTATE.WAIT, 2.0f);
    }
    public void SetEndAttack(float f)
    {
        StartCoroutine( EnemyCtrl.WaitTimeAction(f, () => EndAttack()));
    }
    /// <summary>
    /// アニメーションが終了したときに呼ぶと特定の処理をする
    /// </summary>
    public virtual void EndAnimation()
    {

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
    protected float GetAIRandomNumver()
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

    public void OnReciveMapChenge(MapParent mapParent)
    {
        ReciveMapChenge_aiState();
    }
}
