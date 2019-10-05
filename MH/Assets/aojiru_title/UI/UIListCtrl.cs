using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListCtrl : MonoBehaviour
{
    [SerializeField]List<UICanvas> UIList;

    [SerializeField] UICanvas nowUICanvas;
    public UICanvas NowUICanvas { get { return nowUICanvas; } }

    [SerializeField] float inputSensitive;
    public float InputSensitive { get { return inputSensitive; } }
    [SerializeField] float repeatDelay;
    public float RepeatDelay { get { return repeatDelay; } }

    void SetUIList()
    {
        UIList = new List<UICanvas>();
        for(int i = 0; i < transform.childCount; i++)
        {
            var cv = transform.GetChild(i).GetComponent<UICanvas>();
            if (cv != null)
            {
                UIList.Add(cv);
            }
        }
    }

    public void SetNowUICanvas(int index)
    {
        nowUICanvas = UIList[index] ;
    }

    public void SetUIActive(int index,bool active,int sortOrder=0)
    {
        UIList[index].gameObject.SetActive(active);
        UIList[index].SetSortOrder(sortOrder);
    }
    
}
