using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// キャンバスの基本処理を行う.
/// 開閉や描画順の指定など
/// </summary>
public class UICanvas : MonoBehaviour
{
    /// <summary>
    /// UIの状態
    /// </summary>
    public enum UISTATE
    {
        SLEEP,ACTIVE,ACTIVEEND,DISACTIVE
    }
    [SerializeField] UISTATE uiState;
    public UISTATE UIState { get { return uiState; } }


    Canvas myCanvas;
    Animator animator;
    LoadCtrl loadCtrl;
    protected UIListController uictrl { get; private set; }

    float beforeInputTime;
    public int SortOrder { get { return myCanvas.sortingOrder; } }
    public int MyUIIndex { get; private set; }//uiCtrlの呼び出し番号
    public bool IsUseAnimator { get { return animator != null; } }

    //入力可能かどうか
    [SerializeField]bool inputEnable;



    #region 入力時に追加で呼ばれる関数　畳みたい
    [SerializeField] UnityEvent inputAction_submit;
    [SerializeField] UnityEvent inputAction_cancel;
    [SerializeField] UnityEvent inputAction_ver;
    [SerializeField] UnityEvent inputAction_hor;
    #endregion
    #region　uiStateが変更されたときに呼ばれる関数　畳みたい
    [SerializeField] UnityEvent chengeUIstate_sleep;
    [SerializeField] UnityEvent chengeUIstate_active;
    [SerializeField] UnityEvent chengeUIstate_activeEnd;
    [SerializeField] UnityEvent chengeUIstate_disActive;
    Dictionary<UISTATE, UnityEvent> chengeUIStateActionDic;
    #endregion
    #region MonoBehaviour
    protected virtual void Awake()
    {
        myCanvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();
        loadCtrl = FindObjectOfType<LoadCtrl>();
        InitChengeUIStateAction();
    }

    virtual protected void Start()
    {
        uictrl = GetComponentInParent<UIListController>();
    }

    virtual protected void Update()
    {
        if (inputEnable&&!loadCtrl.IsFadeNow)
        {
            InputUpdate();
        }
    }
    #endregion
    /// <summary>
    /// 入力の受付と入力時の処理の呼び出し
    /// </summary>
    void InputUpdate()
    {
        if (Time.fixedTime < beforeInputTime + uictrl.RepeatDelay) return;
        bool anyInput = true;
        int hor = InputHorizontal();
        int ver = InputVertical();
        if (hor!=0)
        {
            HorizontalAction(hor);
            inputAction_hor.Invoke();
        }
        else if (ver!=0)
        {
            VerticalAction(ver);
            inputAction_ver.Invoke();
        }
        else if (InputSubmit())
        {
            SubmitAction();
            inputAction_submit.Invoke();
        }
        else if (InputCancel())
        {
            CancelAction();
            inputAction_cancel.Invoke();
        }
        else
        {
            anyInput = false;
        }

        if (anyInput)
        {
            AnyInputAction();
        }
    }

    #region input
    int GetInputAxisInt(float f)
    {
        int result = 0;
        if (Mathf.Abs(f) > uictrl.InputSensitive)
        {
            result = (int)Mathf.Sign(f);
        }
        return result;
    }

     int InputHorizontal()
    {
        var i = GetInputAxisInt(Input.GetAxisRaw("Horizontal"));
        return i;
    }
     int InputVertical()
    {
        var i = GetInputAxisInt(Input.GetAxisRaw("Vertical"));
        return i;
    }
     bool InputSubmit()
    {
        if (Input.GetButtonDown("Submit"))
        {
            return true;
        }
        return false;
    }
     bool InputCancel()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            return true;
        }
        return false;
    }
    #endregion
    #region inputAction　入力確定時に呼ばれる関数

    /// <summary>
    /// どれかの入力が受け付けられたとき　
    /// 処理が存在しなくても呼び出されるので要改善
    /// </summary>
    protected virtual void AnyInputAction()
    {
        beforeInputTime = Time.fixedTime;
    }

    protected virtual void HorizontalAction(int input)
    {

    }

    protected virtual void VerticalAction(int input)
    {

    }

    protected virtual void SubmitAction()
    {
       // Debug.Log(gameObject.name+"submit");
    }

    protected virtual void CancelAction()
    {
        //Debug.Log(gameObject.name+"cancel");
    }
    #endregion
    #region 外部使用
    /// <summary>
    /// uiCtrlに登録されている番号の設定
    /// </summary>
    /// <param name="i"></param>
    public void SetMyUIIndex(int i)
    {
        MyUIIndex = i;
    }

    /// <summary>
    /// uiを起動する　
    /// animationがついている場合はanimationで入力開始を指定する
    /// </summary>
    /// <param name="i"></param>
    public void OpenUICanvas(int i)
    {
        SetSortOrder(i);
        gameObject.SetActive(true);
        if (!IsUseAnimator)
        {
            DelayAction_frame(1,()=>inputEnable=true);
        }
        SetUIState(UISTATE.ACTIVE);
    }

    /// <summary>
    /// uiを閉じる
    /// animatinがついている場合はactiveの設定はアニメーションでする
    /// </summary>
    public void CloseUICanvas()
    {
        if (IsUseAnimator)
        {
            SetUIState(UISTATE.ACTIVEEND);
            inputEnable = false;
        }
        else
        {
            SetUIState(UISTATE.DISACTIVE);
            gameObject.SetActive(false);
            inputEnable = false;
        }
    }
    /// <summary>
    /// アニメーションで呼ばれるアクティブの処理
    /// </summary>
    void CloseUICanvas_Animation()
    {
        SetUIState(UISTATE.DISACTIVE);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// スリープ状態にする
    /// </summary>
    public void SleepUICanvas()
    {
        inputEnable = false;
        SetUIState(UISTATE.SLEEP);
    }
   /// <summary>
   /// 描画順の指定
   /// </summary>
   /// <param name="sortOrder"></param>
    public void SetSortOrder(int sortOrder)
    {
        if (myCanvas == null) myCanvas = GetComponent<Canvas>();
        myCanvas.sortingOrder = sortOrder;
    }
    #endregion
    #region UIState
    void SetUIState(UISTATE state)
    {
        if (uiState != state)
        {
            ChengeUIStateAction(state);
            uiState = state;
        }
    }
    
    /// <summary>
    /// ui変更時に対応する処理を呼ぶ
    /// </summary>
    /// <param name="state"></param>
    void ChengeUIStateAction(UISTATE state)
    {
        chengeUIStateActionDic[state].Invoke();
    }
    /// <summary>
    /// uiStateに関係するものの初期化処理
    /// </summary>
    protected virtual void InitChengeUIStateAction()
    {
        chengeUIStateActionDic = new Dictionary<UISTATE, UnityEvent>();
        chengeUIStateActionDic.Add(UISTATE.SLEEP, chengeUIstate_disActive);
        chengeUIStateActionDic.Add(UISTATE.ACTIVE, chengeUIstate_active);
        chengeUIStateActionDic.Add(UISTATE.ACTIVEEND, chengeUIstate_activeEnd);
        chengeUIStateActionDic.Add(UISTATE.DISACTIVE, chengeUIstate_disActive);
    }

    protected void AddChengeUIStateAction(UISTATE state,UnityAction ua)
    {
        chengeUIStateActionDic[state].AddListener(ua);
    }
    
    #endregion
    #region AnimationEvent　animationEventで呼ぶ関数　他では使用禁止
    public void AnimationEV_ActiveInput()
    {
        inputEnable = true;
    }
    public void AnimationEV_DisActiveInput()
    {
        inputEnable = false;
    }

    public void AnimationEV_CloseCanvas()
    {
        CloseUICanvas_Animation();
    }
    #endregion

     protected void DelayAction_frame(int frame,UnityAction ua)
    {
        StartCoroutine(DelayActionColutine_frame(frame,ua));
    }

    IEnumerator DelayActionColutine_frame(int frame,UnityAction ua)
    {
        for(int i = 0; i < frame; i++)
        {
            yield return null;
        }
        ua.Invoke();
    }
}
