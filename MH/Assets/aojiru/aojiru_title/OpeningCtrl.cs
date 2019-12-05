using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 起動時のアニメーションとUIの遷移を指定するクラス
/// </summary>
public class OpeningCtrl : MonoBehaviour
{
    static int OpenigTime;//最初のシーンを通った回数

    [SerializeField] Animator logoAnim;
    [SerializeField] UIListController uiListCtrl;

    private void Start()
    {
         //初回
        if (OpenigTime == 0)
        {
            logoAnim.SetTrigger("Start_op");
            uiListCtrl.OpenUICanvas(5,10);
        }
        else//2回目以降
        {
            logoAnim.SetTrigger("Start_end");
            uiListCtrl.OpenUICanvas(0,10);
        }
        OpenigTime++;
    }
}
