using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_antiFly : EnemyController
{
    [SerializeField, Space(10)] float antiAirHeight;
    [SerializeField] float antiAirLength;
    [SerializeField]float arealTime;

    protected override void Init()
    {
        base.Init();
    }

    protected override void CharacterUpdate()
    {
        base.CharacterUpdate();
        AntiAirUpdate();
    }

    void AntiAirUpdate()
    {
        if (DetectState != DETECTSTATE.UNDETECT)
        {
            if (IsTargetUpSelf(antiAirHeight, plTr))
            {
                arealTime += Time.deltaTime;
            }
            else
            {
                arealTime -= Time.deltaTime * 0.5f;
                if (arealTime < 0) arealTime = 0;
            }
        }
    }

    bool IsTargetUpSelf(float length, Transform _targetTr)
    {
        return (_targetTr.position.y - tr.position.y) > length;
    }

    public bool IsEnable_AntiAirCraft()
    {
        bool result;
        if (arealTime > antiAirLength)
        {
            result = true;
            arealTime = antiAirLength;
        }
        else
        {
            result = false;
        }
        return result;
    }
}
