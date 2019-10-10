using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GetDataText_base<T> : MonoBehaviour
{
    [SerializeField] UICanvas myUIcanvas;
    protected UI_selectComponentBase targetButton { get; private set; }
    UI_selectComponentBase beforeTargetButton;
    protected T myData { get; private set; }

    private void Update()
    {
        UpdateTargetButton();
        if (beforeTargetButton != targetButton)
        {
            TargetChengeAction();
        }
    }

    void UpdateTargetButton()
    {
        beforeTargetButton = targetButton;
        targetButton = myUIcanvas.nowSelectComponent;
    }
    protected virtual void TargetChengeAction()
    {
        SetMydata();
        if (targetButton!=null&&myData != null)
        {
            DisplayData();
        }
        else
        {
            DisplayData_nullData();
        }
    }

    protected virtual void SetMydata()
    {
        if (targetButton != null) myData = GetMydata();
    }
    protected abstract T GetMydata();
    protected abstract void DisplayData();
    protected abstract void DisplayData_nullData();
}
