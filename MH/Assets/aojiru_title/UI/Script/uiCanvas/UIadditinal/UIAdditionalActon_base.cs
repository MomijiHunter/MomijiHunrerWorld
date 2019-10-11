using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UICanvasでinspectorからsubmitActionなどに追加する処理を書くところ
/// ココの関数以外のものは原則入れない
/// </summary>
public class UIAdditionalActon_base : MonoBehaviour
{
    protected UIListController uiCtrl { get; private set; }
    protected UICanvas myCanvas { get; private set; }

    private void Start()
    {
        uiCtrl = GetComponentInParent<UIListController>();
        myCanvas = GetComponent<UICanvas>();
        InitAction();
    }
    
    virtual protected void InitAction()
    {

    }
}
