using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ボタンを使用するUICanvas
/// </summary>
public class UICanvas_button : UICanvas
{
    [SerializeField] bool inverseY;
    [SerializeField] List<int> indexRangeList;
    protected List<int> indexRangeList_use;
    [SerializeField] Transform buttonParent;//selectButtonの親をしているTｒ
    [SerializeField] List<UI_selectComponentBase> selectList_inspector;
    List<List<UI_selectComponentBase>> selectList_use = new List<List<UI_selectComponentBase>>();

    public UI_selectComponentBase nowSelectComponent { get { return selectList_use[index_now.x][index_now.y]; } }

    [SerializeField] Vector2Int index_now;

    Vector2Int index_before;
    
    #region OverRide
    protected override void Start()
    {
        base.Start();
        if (buttonParent == null) buttonParent = transform;
        SetIndexRangeList();
        SetSelectComponentList_inspector();
        SetSelectComponentList();
    }

    protected override void Update()
    {
        base.Update();
        IndexUpdate();
    }

    protected override void HorizontalAction(int input)
    {
        base.HorizontalAction(input);
        AddIndex(input, 0);
    }

    protected override void VerticalAction(int input)
    {
        base.VerticalAction(input);
        AddIndex(0, (inverseY) ? -input : input);
    }

    protected override void InitChengeUIStateAction()
    {
        base.InitChengeUIStateAction();
        AddChengeUIStateAction(UISTATE.ACTIVE, () =>DelayAction_frame(1, ()=>ChengeIndexAction()));
    }
    #endregion
    #region uiSelectComponentの処理
    /// <summary>
    /// indexの範囲を使用しやすい形に書き換える
    /// </summary>
    void SetIndexRangeList()
    {
        indexRangeList_use = new List<int>(indexRangeList);
        for (int i = 0; i < indexRangeList_use.Count; i++)
        {
            indexRangeList_use[i] -= 1;
        }
    }
    /// <summary>
    /// inspectorが空の場合自動で読み込む
    /// </summary>
    void SetSelectComponentList_inspector()
    {
        if (selectList_inspector.Count > 0) return;
        selectList_inspector = new List<UI_selectComponentBase>();
        for (int i = 0; i < buttonParent.childCount; i++)
        {
            var data = buttonParent.GetChild(i).GetComponent<UI_selectComponentBase>();
            if (data == null) continue;
            selectList_inspector.Add(data);
        }
    }
    /// <summary>
    /// 使いやすい形のselectList_useに入れなおす
    /// </summary>
    void SetSelectComponentList()
    {
        if (selectList_inspector.Count == 0) return;
        for (int i = 0; i < selectList_inspector.Count; i++)
        {
            if (i < indexRangeList_use.Count)
            {
                selectList_use.Add(new List<UI_selectComponentBase>());
            }
            int _index = i % (indexRangeList_use.Count);
            selectList_use[_index].Add(selectList_inspector[i]);
        }
    }


    #endregion
    #region Index処理
    void IndexUpdate()
    {
        if (selectList_use.Count == 0) return;

        if (index_now != index_before)
        {
            ChengeIndexAction();
        }
    }
    /// <summary>
    /// indexの値が変更になった時の処理
    /// </summary>
    virtual protected void ChengeIndexAction()
    {
        selectList_use[index_before.x][index_before.y].SelectOff();
        selectList_use[index_now.x][index_now.y].SelectOn();
    }


    protected void AddIndex(int x, int y)
    {
        SetIndex(index_now.x + x, index_now.y + y);
    }

    protected void SetIndex(int x, int y)
    {
        index_before = index_now;
        index_now = new Vector2Int(x, y);
        CheckIndexOver();
    }
    #endregion
    #region Indexの値が範囲を超えた時の処理
    /// <summary>
    /// indexの値が適切であるかを確認し、だめなら適切な関数を呼び出す
    /// </summary>
    void CheckIndexOver()
    {
        if (index_now.x < 0) LowerIndexAction_x();
        else if (index_now.x >= indexRangeList_use.Count) OverIndexAction_x();
        if (index_now.y > indexRangeList_use[index_now.x]) OverIndexAction_y();
        else if (index_now.y < 0) LowerIndexAction_y();

    }

    virtual protected void OverIndexAction_y()
    {
        if (index_before.y == indexRangeList_use[index_now.x]
            && index_before.x == index_now.x) index_now.y = 0;//行が変わってなくて、一番下にあるなら一番上へるーぶ
        else index_now.y = indexRangeList_use[index_now.x];
    }

    virtual protected void OverIndexAction_x()
    {
        if (index_before.x == indexRangeList_use.Count - 1
            && index_before.y == index_now.y) index_now.x = 0;
        else index_now.x = indexRangeList_use.Count - 1;
    }

    virtual protected void LowerIndexAction_y()
    {
        if (index_before.y == 0) index_now.y = indexRangeList_use[index_now.x];
        else index_now.y = 0;
    }
    virtual protected void LowerIndexAction_x()
    {
        if (index_before.x == 0) index_now.x = indexRangeList_use.Count - 1;
        else index_now.x = 0;
    }
    #endregion
}
