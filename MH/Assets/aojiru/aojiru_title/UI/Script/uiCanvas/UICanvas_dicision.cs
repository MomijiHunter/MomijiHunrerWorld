using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 決定、非決定の処理が行える
/// 選択しごとの処理に拡張したい
/// </summary>
public class UICanvas_dicision : UICanvas_button
{
    /// <summary>
    /// 決定ボタン　複数設定可能
    /// </summary>
    [SerializeField] List<UI_selectComponentBase> dicisionList;

    [SerializeField] UnityEvent dicisionAction;
    [SerializeField] UnityEvent notDicisionAction;

    protected override void SubmitAction()
    {
        base.SubmitAction();
        if (CheckDicision())
        {
            dicisionAction.Invoke();
        }
        else
        {
            notDicisionAction.Invoke();
        }
    }

    /// <summary>
    /// 選択されたボタンが決定ボタンかどうかを返す
    /// </summary>
    /// <returns></returns>
    bool CheckDicision()
    {
        bool result = false;
        foreach(var data in dicisionList)
        {
            if (data.Equals(nowSelectComponent))
            {
                result = true;
            }
        }
        return result;
    }
}
