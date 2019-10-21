using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace aojilu
{
    /// <summary>
    /// 関数を登録して呼び出すもの
    /// </summary>
    public class AIActionOrganaizer
    {
        Dictionary<string, UnityAction> actionDic=new Dictionary<string, UnityAction>();
        UnityAction nowAction;

        public void AddAction(string key ,UnityAction ua)
        {
            actionDic.Add(key, ua);
        }

        public void SetNowAction(string key)
        {
            nowAction = actionDic[key];
        }

        public UnityAction GetNowAction()
        {
            return nowAction;
        }
    }
}
