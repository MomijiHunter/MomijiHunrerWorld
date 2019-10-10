using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UICanvas_YesNo : UICanvas
{
    [SerializeField] List<UI_selectComponentBase> yesButtonList;
    protected override void SubmitAction()
    {
        if (CheckYesButton())
        {
            YesAction();
        }
        else
        {
            NoAction();
        }
    }

    protected override void CancelAction()
    {
        NoAction();
    }

    bool CheckYesButton()
    {
       foreach(var obj in yesButtonList)
        {
            if (obj.Equals(nowSelectComponent)) return true;
        }
        return false;
    }

    protected abstract void YesAction();
    protected abstract void NoAction();
}
