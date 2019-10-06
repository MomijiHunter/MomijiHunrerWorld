using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListCtrl : MonoBehaviour
{
    [SerializeField]List<UICanvas> UIList;

    [SerializeField] UICanvas nowUICanvas;
    public UICanvas NowUICanvas { get { return nowUICanvas; } }
    public UICanvas nextUICanvas { get; private set; }
    [SerializeField] int firstUIIndex = -1;

    [SerializeField] float inputSensitive;
    public float InputSensitive { get { return inputSensitive; } }
    [SerializeField] float repeatDelay;
    public float RepeatDelay { get { return repeatDelay; } }

    private void Start()
    {
        SetUIList();

        if (firstUIIndex >= 0)
        {
            OpenUICanvas(firstUIIndex,10);
        }
    }

    private void Update()
    {
        if (nextUICanvas != null)
        {
            if (nowUICanvas==null||nowUICanvas.StateMode == UICanvas.State.SLEEP)
            {
                SetNowUICanvas(nextUICanvas);
                nextUICanvas.SetStateAwake();
                nextUICanvas = null;
            }
        }
    }

    void SetUIList()
    {
        UIList = new List<UICanvas>();
        for(int i = 0; i < transform.childCount; i++)
        {
            var cv = transform.GetChild(i).GetComponent<UICanvas>();
            if (cv != null)
            {
                UIList.Add(cv);
                cv.SetMyUIIndex(UIList.Count-1);
            }
        }
    }

    void SetNextUICanvas(int index)
    {
        SetNextUICanvas(UIList[index]);
    }
    void SetNextUICanvas(UICanvas cv)
    {
        nextUICanvas = cv;
    }

    void SetNowUICanvas(int index)
    {
        SetNowUICanvas(UIList[index]);
    }
    void SetNowUICanvas(UICanvas cv)
    {
        nowUICanvas = cv;
    }

    public void OpenUICanvas(int index,int sortOrder=0)
    {
        SetNextUICanvas(index);
        UIList[index].SetSortOrder(sortOrder);
    }

    public void CloseUICanvas(int index)
    {
        UIList[index].SetStateEnd();
    }
    
    public void SleepUICanvas(int index)
    {
        UIList[index].SetStateSleep();
    }
}
