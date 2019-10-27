using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController_fly))]
public class EnemyMain_fly : EnemyMain
{
    protected EnemyController_fly EnemyCtrl_fly { get; private set; }

    [SerializeField] bool flyMode;//飛行状態フラグ
    protected bool FlyMode { get { return flyMode; } }
    [SerializeField] float flyHight;//上昇する高さ
    protected float FlyHight { get { return flyHight; } }
    [SerializeField] float flyHight_land;//landingする高さ
    protected float FlyHight_land { get { return flyHight_land; } }

    protected float MoveSpeedY { get { return EnemyCtrl_fly.MoveSpeedY; } }

    /// <summary>
    /// trueの時遷移処理Updateが呼ばれるようになる
    /// </summary>
    bool swichFlyMode = false;
   [SerializeField] bool heightModify = false;

    protected override void Awake()
    {
        base.Awake();
        EnemyCtrl_fly = GetComponent<EnemyController_fly>();
    }

    protected override void InitAIUpdateAction()
    {
        base.InitAIUpdateAction();
        aiUpdateOrg_attack.AddAction("fly", () => AIUpdate_fly_attack());
        aiUpdateOrg_detect.AddAction("fly", () => AIUpdate_fly_detect());
        aiUpdateOrg_undetect.AddAction("fly", () => AIUpdate_fly_unDetect());

        aiUpdateOrg_attack.AddAction("fly_swich", () => AIUpdate_swichFly());
        aiUpdateOrg_detect.AddAction("fly_swich", () => AIUpdate_swichFly());
        aiUpdateOrg_undetect.AddAction("fly_swich", () => AIUpdate_swichFly());

        aiUpdateOrg_attack.AddAction("fly_modify", () => AIUpdate_modifiHeight());
        aiUpdateOrg_detect.AddAction("fly_modify", () => AIUpdate_modifiHeight());
        aiUpdateOrg_undetect.AddAction("fly_modify", () => AIUpdate_modifiHeight());

        aiUpdateOrg_mapChenge.AddAction("fly", () => AIUpdate_mapchengeFly());
    }

    virtual protected void AIUpdate_fly_unDetect()
    {

    }

    virtual protected void AIUpdate_fly_detect()
    {

    }

    virtual protected void AIUpdate_fly_attack()
    {
    }

    /// <summary>
    /// 飛行状態の遷移処理
    /// </summary>
    virtual protected void AIUpdate_swichFly()
    {

    }

    virtual protected void AIUpdate_mapchengeFly()
    {

    }

    void AIUpdate_modifiHeight()
    {
        if (EnemyCtrl_fly.MoveToTarget_Y(flyHight, MoveSpeedY))
        {
            SetAIState(AISTATE.AISELECT, 3.0f);
            heightModify = false;
        }
    }

    protected override void AISelectDisturb_aiState()
    {
        base.AISelectDisturb_aiState();

        if (Mathf.Abs( EnemyCtrl_fly.GetDistanceGround()- flyHight) > 1.0f)
        {
            heightModify = true;
        }

        if (aiState != AISTATE.MAPCHENGE)
        {
            if (swichFlyMode)//飛行遷移処理
            {
                AIOrgSetNowAction("fly_swich");
            }
            else if (flyMode)//飛行処理
            {
                if (heightModify)
                {
                    AIOrgSetNowAction("fly_modify");
                }
                else
                {
                    AIOrgSetNowAction("fly");
                }
            }
            else//地上処理
            {
                AIOrgSetNowAction("default");
            }
        }
    }

    /// <summary>
    /// 飛行状態の開始　遷移処理に移行する
    /// </summary>
    protected void FlyMode_start()
    {
        flyMode = true;
        StartSwichFlyMode();
    }

    /// <summary>
    /// 飛行状態の終了　遷移処理に移行する
    /// </summary>
    protected void FlyMode_end()
    {
        flyMode = false;
        StartSwichFlyMode();
    }

    /// <summary>
    /// swichFlyModeの終了処理
    /// FlyModeの変更時に呼ばれる
    /// </summary>
    void StartSwichFlyMode()
    {
        swichFlyMode = true;
        SetAIState(AISTATE.AISELECT, 3.0f);//swichFlyのAI関数に入るための処理
        EnemyCtrl_fly.StopMove();
        if (flyMode)//上昇時は最初にanimationを呼ぶ
        {
            StartFlyAnimation();
        }
    }

    /// <summary>
    /// swichFlyModeの終了処理
    /// AIで条件を判定して呼ぶ
    /// </summary>
    protected void EndSwichFlyMode()
    {
        swichFlyMode = false;
        EnemyCtrl_fly.StopMove();
        EnemyCtrl_fly.StopMove_Y();
        if (flyMode)//上昇時はここで飛行処理終了
        {
            SetAIState(AISTATE.AISELECT, 3.0f);
        }
        else//下降時は最後にanimationを呼ぶ
        {
            StartFlyAnimation();
        }
    }

    #region animationEvent

    /// <summary>
    /// flyAnimationの開始
    /// </summary>
    void StartFlyAnimation()
    {
        SetAIStop(true);
        if (flyMode)
        {
            animator.SetTrigger("takeOff");
        }
        else
        {
            animator.SetTrigger("landing");
        }
    }

    /// <summary>
    /// flyAnimationの終了処理　
    /// animationから呼ぶ
    /// </summary>
    public void EndFlyAnimation()
    {
        StartCoroutine( EnemyCtrl.WaitFrameAction(1,()=> SetAIStop(false)));
        EnemyCtrl.AnimEvent_ReplaceBodyPosition();
        if (flyMode)
        {
            EnemyCtrl_fly.GravityOff();
        }
        else//下降時時はここで飛行処理終了
        {
            SetAIState(AISTATE.AISELECT, 3.0f);
            EnemyCtrl_fly.GravityOn();
        }
    }
    
    #endregion
}
