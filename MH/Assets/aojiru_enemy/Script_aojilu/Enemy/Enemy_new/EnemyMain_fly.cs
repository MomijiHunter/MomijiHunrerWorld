using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController_fly))]
public class EnemyMain_fly : EnemyMain
{
    protected new EnemyController_fly EnemyCtrl { get; private set; }

    [SerializeField] bool flyMode;
    protected bool FlyMode { get { return flyMode; } }
    [SerializeField] float flyHight;
    protected float FlyHight { get { return flyHight; } }
    [SerializeField] float flyHight_land;
    protected float FlyHight_land { get { return flyHight_land; } }

    protected float MoveSpeedY { get { return EnemyCtrl.MoveSpeedY; } }

    bool swichFlyMode = false;

    protected override void Awake()
    {
        base.Awake();
        EnemyCtrl = GetComponent<EnemyController_fly>();
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
            if (swichFlyMode)
            {
                aiUpdateOrg_detect.SetNowAction("fly_not");
            }
             else if (flyMode)
            { 
                aiUpdateOrg_attack.SetNowAction("fly");
                aiUpdateOrg_detect.SetNowAction("fly");
            }
            else
            {

                aiUpdateOrg_attack.SetNowAction("attack");
                aiUpdateOrg_detect.SetNowAction("detect");
            }
        }
    }

    protected void StartFlyMode()
    {
        animator.SetTrigger("takeOff");
        SetRandFiexed(110);
        EnemyCtrl.StopMove();
    }

    protected void EndFlyMode()
    {
        flyMode = false;
        EnemyCtrl.StopMove();
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
        EnemyCtrl.GravityOff();
        SetSwichFlyMode(true);
        flyMode = true;
        SetAIState(AISTATE.AISELECT, 3.0f);
        ResetRandFiexed();
    }

    public void AnimEvent_EndFlyMode()
    {
        EnemyCtrl.GravityOn();
        EndSwichFlyMode();
        ResetRandFiexed();
        SetAIState(AISTATE.AISELECT, 3.0f);
    }
    
    #endregion
}
