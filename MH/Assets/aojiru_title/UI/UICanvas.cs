using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    public enum State
    {
        SLEEP,AWAKE,NONE,END
    }
    [SerializeField] State state;
    public State StateMode { get { return state; } }

    Canvas myCanvas;
    protected UIListCtrl uictrl { get; private set; }
    [SerializeField] bool inverseY;
    [SerializeField] List<int> indexRangeList;
    [SerializeField] List<UI_selectComponentBase> selectList_inspector;
    List<List<UI_selectComponentBase>> selectList_use=new List<List<UI_selectComponentBase>>();

    [SerializeField]Vector2Int index_now;
    
    Vector2Int index_before;

    float beforeInputTime;

    public int sortOrder { get {return myCanvas.sortingOrder; } }


    private void Awake()
    {

        myCanvas = GetComponent<Canvas>();
    }

    void Start()
    {
        uictrl = GetComponentInParent<UIListCtrl>();

        SetSelectComponentList();
    }

    private void Update()
    {
        StateUpdate();
        if (CheckInputEnable())
        {
            if (InputVertical()) ;
            else if (InputHorizontal()) ;
            else if (InputSubmit()) SubmitAction();
            else if (InputCancel()) CancelAction();

            IndexUpdate();
        }
    }

    void IndexUpdate()
    {
        selectList_use[index_before.x][index_before.y].SelectOff();
        selectList_use[index_now.x][index_now.y].SelectOn();
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

    protected virtual void CancelAction()
    {
        Debug.Log("cancel");
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
        AddIndex(0, (inverseY)? -i:i);
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
    protected virtual bool InputCancel()
    {
        if (Input.GetButtonDown("Cancel"))
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
        if (myCanvas == null) myCanvas = GetComponent<Canvas>();
        myCanvas.sortingOrder = sortOrder;
    }

    void SetSelectComponentList()
    {
        if (selectList_inspector.Count == 0) return;
        for(int i = 0; i < selectList_inspector.Count; i++)
        {
            if (i < indexRangeList.Count)
            {
                selectList_use.Add(new List<UI_selectComponentBase>());
            }
            int _index = i % (indexRangeList.Count);
            selectList_use[_index].Add(selectList_inspector[i]);
        }
    }


    #region state
    void StateUpdate()
    {
        switch (state)
        {
            case State.AWAKE:
                
                SetState(State.NONE);
                break;
            case State.NONE:
                break;
            case State.END:
                SetState(State.SLEEP);
                gameObject.SetActive(false);
                break;
            case State.SLEEP:
                break;
        }
    }

    void SetState(State st)
    {
        state = st;
    }
    

    public void SetStateSleep()
    {
        SetState(State.SLEEP);
    }

    public void SetStateEnd()
    {
        SetState(State.END);
    }

    public void SetStateAwake()
    {
        if (gameObject.activeInHierarchy)
        {
            SetState(State.NONE);
        }
        else
        {
            SetState(State.AWAKE);
            gameObject.SetActive(true);
        }
    }
    
    #endregion
}
