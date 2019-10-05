using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public enum State
    {
        SLEEP,AWAKE,NONE,END
    }
    [SerializeField] State state;

    Canvas myCanvas;
    UIListCtrl uictrl;

    [SerializeField] List<int> indexRangeList;
    [SerializeField]Vector2Int index_now;
    Vector2Int index_before;

    float beforeInputTime;
    

    void Start()
    {
        myCanvas = GetComponent<Canvas>();
        uictrl = GetComponentInParent<UIListCtrl>();
    }

    private void Update()
    {
        if (CheckInputEnable())
        {
            if (InputVertical()) ;
            else if (InputHorizontal()) ;
            else if (InputSubmit()) SubmitAction();
        }
    }

    bool CheckInputEnable()
    {
        bool result=true;
        if (state!=State.NONE||
            Time.fixedTime < beforeInputTime + uictrl.RepeatDelay) result = false;
        return result;
    }

    protected virtual void SubmitAction()
    {
        Debug.Log("submit");
    }
    #region input
    int GetInputAxisInt(float f)
    {
        int result = 0;
        if (Mathf.Abs(f) > uictrl.InputSensitive)
        {
            result = (int)Mathf.Sign(f);
        }
        return result;
    }

    protected virtual bool InputHorizontal()
    {
        var i = GetInputAxisInt(Input.GetAxis("Horizontal"));
        if (i == 0) return false;
        AddIndex(i, 0);
        return true;
    }
    protected virtual bool InputVertical()
    {
        var i = GetInputAxisInt(Input.GetAxis("Vertical"));
        if (i == 0) return false;
        AddIndex(0, i);
        return true;
    }

    protected virtual bool InputSubmit()
    {
        if (Input.GetButtonDown("Submit"))
        {
            return true;
        }
        return false;
    }
    #endregion
    protected void AddIndex(int x,int y)
    {
        SetIndex(index_now.x+x,index_now.y+y);
    }

    protected void SetIndex(int x,int y)
    {
        index_before = index_now;
        index_now = new Vector2Int(x,y);
        CheckIndexOver();
        beforeInputTime = Time.fixedTime;
    }
    #region Indexの値が範囲を超えた時の処理
    /// <summary>
    /// indexの値が適切であるかを確認し、だめなら適切な関数を呼び出す
    /// </summary>
    void CheckIndexOver()
    {
        if (index_now.x < 0) LowerIndexAction_x();
        else if (index_now.x >= indexRangeList.Count) OverIndexAction_x();
        if (index_now.y > indexRangeList[index_now.x]) OverIndexAction_y();
        else if (index_now.y < 0) LowerIndexAction_y();

    }

    virtual protected void OverIndexAction_y()
    {
        if (index_before.y == indexRangeList[index_now.x]
            &&index_before.x==index_now.x) index_now.y=0;//行が変わってなくて、一番下にあるなら一番上へるーぶ
        else index_now.y = indexRangeList[index_now.x];
    }

    virtual protected void OverIndexAction_x()
    {
        if (index_before.x == indexRangeList.Count-1
            && index_before.y == index_now.y) index_now.x = 0;
        else index_now.x = indexRangeList.Count-1;
    }

    virtual protected void LowerIndexAction_y()
    {
        if (index_before.y == 0) index_now.y = indexRangeList[index_now.x];
        else index_now.y = 0;
    }
    virtual protected void LowerIndexAction_x()
    {
        if (index_before.x == 0) index_now.x = indexRangeList.Count-1;
        else index_now.x = 0;
    }
    #endregion
    public void SetSortOrder(int sortOrder)
    {
        myCanvas.sortingOrder = sortOrder;
    }
}
