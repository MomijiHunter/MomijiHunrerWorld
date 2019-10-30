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
        /// <summary>
        /// 登録済みの関数を利用する場合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defKey">既に定義されているキー</param>
        public void AddAction(string key,string defKey)
        {
            actionDic.Add(key, actionDic[defKey]);
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
