using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class UIAdditonalAction_fade : UIAdditionalActon_base
{
    LoadCtrl loadCtrl;
    [SerializeField] UnityEvent blackEvent;
    [SerializeField] UnityEvent blackEvent2;
    List<UnityAction> actionList;
    List<UnityAction> actionList2;

    protected override void InitAction()
    {
        base.InitAction();
        loadCtrl = FindObjectOfType<LoadCtrl>();
        actionList = CreatActionList(blackEvent);
        actionList2 = CreatActionList(blackEvent2);
    }

    public void LoadAction_withEvent()
    {
        if (actionList != null)
        {
            int i = 0;
            foreach (var act in actionList)
            {
                i++;
                loadCtrl.AddBlackAction("ui_additinal : " + i, act);
            }
        }
        LoadAction();
    }
    public void LoadAction_withEvent2()
    {
        if (actionList2 != null)
        {
            int i = 0;
            foreach (var act in actionList2)
            {
                i++;
                loadCtrl.AddBlackAction("ui_additinal : " + i, act);
            }
        }
        LoadAction();
    }
    public void LoadAction()
    {
        loadCtrl.FadeAction();
    }
    

    /// <summary>
    /// UnityEventをUnityActionのListにして返す
    /// </summary>
    List<UnityAction> CreatActionList(UnityEvent ue)
    {
        int count = ue.GetPersistentEventCount();
        var m_getActionList = new List<UnityAction>();
        for (int i = 0; i < count; ++i)
        {
            //CreateDelegateでUnityActionを作っている
            UnityAction action = (UnityAction)System.Delegate.CreateDelegate(typeof(UnityAction), ue.GetPersistentTarget(i), ue.GetPersistentMethodName(i));
            m_getActionList.Add(action);
        
        }
        return m_getActionList;
    }
}
