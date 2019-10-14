using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_buttonData_nextUIData : UI_buttonDataBase
{
    [SerializeField] int targetUIIndex;
    public int TargetUIIndex { get { return targetUIIndex; } }

}

public class UI_selectComp_button_uiData : UI_selectComp_text_image
{
    [SerializeField] UI_buttonData_nextUIData myData;


    public override T GetData<T>()
    {
        return myData as T;
    }
}
