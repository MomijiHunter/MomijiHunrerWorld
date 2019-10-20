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
        aiUpdateOrg_detect.AddAction("fly_not", () => AIUpdate_notSuitableFLy());

    }

    virtual protected void AIUpdate_fly_detect()
    {

    }

    virtual protected void AIUpdate_fly_attack()
    {

    }

    /// <summary>
    /// 飛行フラグと飛行状態があっていないときの処理
    /// </summary>
    virtual protected void AIUpdate_notSuitableFLy()
    {

    }

    protected override void AISelectDisturb_aiState()
    {
        base.AISelectDisturb_aiState();
        if (aiState != AISTATE.MAPCHENGE)
        {
            if (swichFlyMode)//飛行遷移処理
            {
                aiUpdateOrg_detect.SetNowAction("fly_not");
            }
             else if (flyMode)//飛行処理
            { 
                aiUpdateOrg_attack.SetNowAction("fly");
                aiUpdateOrg_detect.SetNowAction("fly");
            }
            else//地上処理
            {

                aiUpdateOrg_attack.SetNowAction("attack");
                aiUpdateOrg_detect.SetNowAction("detect");
            }
        }
    }

    /// <summary>
    /// 飛行状態の開始　animation待ちをする
    /// </summary>
    protected void StartFlyMode()
    {
        animator.SetTrigger("takeOff");
        SetRandFiexed(110);
        EnemyCtrl_fly.StopMove();
    }

    /// <summary>
    /// 飛行状態の終了　遷移処理に移行する
    /// </summary>
    protected void EndFlyMode()
    {
        flyMode = false;
        EnemyCtrl_fly.StopMove();
        SetSwichFlyMode(true);
    }

    void SetSwichFlyMode(bool flag)
    {
        swichFlyMode = flag;
    }

    protected void EndSwichFlyMode()
    {
        SetSwichFlyMode(false);
    }

    #region animationEvent

    public void AnimEvent_StartFlyMode()
    {
        EnemyCtrl_fly.GravityOff();
        SetSwichFlyMode(true);
        flyMode = true;
        SetAIState(AISTATE.AISELECT, 3.0f);
        ResetRandFiexed();
    }

    public void AnimEvent_EndFlyMode()
    {
        EnemyCtrl_fly.GravityOn();
        SetSwichFlyMode(false);
        ResetRandFiexed();
        SetAIState(AISTATE.AISELECT, 3.0f);
    }
    
    #endregion
}
