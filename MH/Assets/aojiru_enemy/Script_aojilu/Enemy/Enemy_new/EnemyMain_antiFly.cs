using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController_antiFly))]
public class EnemyMain_antiFly : EnemyMain
{
    protected EnemyController_antiFly EnemyCtrl_antiFly { get; private set; }

    [SerializeField] bool isAntiArealMode;
    protected override void Awake()
    {
        base.Awake();
        EnemyCtrl_antiFly = GetComponent<EnemyController_antiFly>();
    }


    protected override void InitAIUpdateAction()
    {
        base.InitAIUpdateAction();
        aiUpdateOrg_attack.AddAction("antiFly", ()=>AIUpdate_antiFly_attack());
        aiUpdateOrg_detect.AddAction("antiFly", ()=>AIUpdate_antiFly_detect());

    }
    protected override void AISelectDisturb_aiState()
    {
        base.AISelectDisturb_aiState();

        if (aiState != AISTATE.MAPCHENGE)
        {
            if (EnemyCtrl_antiFly.IsEnable_AntiAirCraft())
            {
                aiUpdateOrg_attack.SetNowAction("antiFly");
                aiUpdateOrg_detect.SetNowAction("antiFly");
                isAntiArealMode = true;
            }
            else
            {
                aiUpdateOrg_attack.SetNowAction("default");
                aiUpdateOrg_detect.SetNowAction("default");
                isAntiArealMode = false;
            }
        }
    }
    

    virtual protected void AIUpdate_antiFly_attack()
    {

    }
    virtual protected void AIUpdate_antiFly_detect()
    {

    }
}
