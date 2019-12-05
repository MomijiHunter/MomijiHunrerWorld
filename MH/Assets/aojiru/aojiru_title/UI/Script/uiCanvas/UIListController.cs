using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListController : MonoBehaviour
{
    [SerializeField] List<UICanvas> UIList;

    [SerializeField] UICanvas nowUICanvas;
    public UICanvas NowUICanvas { get { return nowUICanvas; } }
    
    [SerializeField] int firstUIIndex = -1;


    [SerializeField] float inputSensitive;
    public float InputSensitive { get { return inputSensitive; } }
    [SerializeField] float repeatDelay;
    public float RepeatDelay { get { return repeatDelay; } }

    private void Awake()
    {
        SetUIList();

        if (firstUIIndex >= 0)
        {
            OpenUICanvas(firstUIIndex, 10);
        }
    }

    private void Update()
    {
        InputUpdate();
    }

    /// <summary>
    /// 入力によって何かを開く場合に記述
    /// </summary>
    protected virtual void InputUpdate()
    {

    }

    void SetUIList()
    {
        UIList = new List<UICanvas>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cv = transform.GetChild(i).GetComponent<UICanvas>();
            if (cv != null)
            {
                UIList.Add(cv);
                cv.SetMyUIIndex(UIList.Count - 1);
            }
        }
    }


    void SetNowUICanvas(int index)
    {
        SetNowUICanvas(UIList[index]);
    }
    void SetNowUICanvas(UICanvas cv)
    {
        nowUICanvas = cv;
    }

    public void OpenUICanvas(int index, int sortOrder = 0)
    {
        UIList[index].OpenUICanvas(sortOrder);
        SetNowUICanvas(index);
    }

    public void CloseUICanvas(int index)
    {
        UIList[index].CloseUICanvas();
    }

    public void SleepUICanvas(int index)
    {
        UIList[index].SleepUICanvas();
    }
    
}
