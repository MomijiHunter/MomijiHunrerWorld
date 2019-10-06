using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_selectComponentBase : MonoBehaviour
{
    [SerializeField] bool selectable;
    public bool Selectable { get { return selectable; }protected set { selectable = value; } }

    [SerializeField] bool sleep;
    public bool Sleep { get { return sleep; } protected set { sleep = value; } }

    protected UICanvas myCanbus { get; private set; }

    public bool selected { get; private set; }

    virtual protected void Start()
    {
        myCanbus = GetComponentInParent<UICanvas>();
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

        if (myCanbus.StateMode != UICanvas.State.NONE) sleep = true;
        else sleep = false;

        if (beforeSleep != sleep && !sleep) SleepEndAction();
    }

    protected virtual void SelectableUpdate()
    {

    }

    protected virtual void SleepEndAction()
    {

    }
}
