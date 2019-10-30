using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_buttonDataBase
{

}

/// <summary>
/// ボタンとかのベース
/// </summary>
public abstract class  UI_selectComponentBase : MonoBehaviour
{
    [SerializeField] bool selectable;
    public bool Selectable { get { return selectable; }protected set { selectable = value; } }

    [SerializeField] bool sleep;
    public bool Sleep { get { return sleep; } protected set { sleep = value; } }

    protected UICanvas myCanbus { get; private set; }
    protected UIListController uiCtrl { get; private set; }

    public bool selected { get; private set; }

    //追加の処理　選択されたときに呼ばれる
    [SerializeField] UnityEvent additionalAction;
    public UnityEvent AdditionalAction { get { return additionalAction; } }

    virtual protected void Start()
    {
        myCanbus = GetComponentInParent<UICanvas>();
        uiCtrl = GetComponentInParent<UIListController>();
        AddAdditionalEvent();
    }

    virtual protected void Update()
    {
        SleepUpdate();
        SelectableUpdate();
    }

    public void SetSelectable(bool flag)
    {
        selectable = flag;
    }

    public virtual void SelectOn()
    {
        selected = true;
    }

    public virtual void SelectOff()
    {
        selected = false;
    }

    protected virtual void SleepUpdate()
    {
        bool beforeSleep = sleep;

        if (myCanbus.UIState != UICanvas.UISTATE.ACTIVE) sleep = true;
        else sleep = false;

        if (beforeSleep != sleep && !sleep) SleepEndAction();
    }

    protected virtual void SelectableUpdate()
    {

    }

    protected virtual void SleepEndAction()
    {

    }

    /// <summary>
    /// additionalEventの追加
    /// </summary>
    protected virtual void AddAdditionalEvent()
    {

    }

    #region Data
    public abstract T GetData<T>() where T : UI_buttonDataBase;
    #endregion
}
