using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_selectComp_button_uiData : UI_selectComp_text_image
{
    [SerializeField] int targetUIIndex;
    public int TargetUIIndex { get { return targetUIIndex; } }
}
