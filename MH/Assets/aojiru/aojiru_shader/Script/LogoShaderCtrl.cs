using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GradientAlphaシェーダーの_posXを変更するスクリプト
/// </summary>
public class LogoShaderCtrl : MonoBehaviour
{
    [SerializeField] Image myImage;
    [SerializeField,Range(0,1)] float posx;
    float beforePosx;
    Material targetMaterial;

    private void Start()
    {
        targetMaterial = myImage.material;
        beforePosx = SetMaterialPosx(posx);
    }

    private void Update()
    {
        if (posx != beforePosx)
        {
            beforePosx=SetMaterialPosx(posx);
        }
    }

    /// <summary>
    /// GradientAlphaシェーダーの_posXをfの値に変更する
    /// _posxは透明化が始まる位置
    /// </summary>
    float SetMaterialPosx(float f)
    {
        targetMaterial.SetFloat("_PosX",f);
        return f;
    }
}
