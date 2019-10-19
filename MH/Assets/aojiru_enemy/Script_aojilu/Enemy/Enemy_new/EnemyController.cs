using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using kubota;
using aojilu;

public class EnemyController : CharBase
{
    #region Debug

    [SerializeField] bool stateLog;//AIstateのログを出す
    [SerializeField] bool nonActive;//処理の停止
    #endregion

    public enum BATTLETYPE
    {
        DETECT_ATTACK,
        DAMAGED_ATTACK,
    }
    [SerializeField] protected BATTLETYPE battleType = BATTLETYPE.DETECT_ATTACK;

    public enum DETECTSTATE
    {
        DETECT, UNDETECT, PREDETECT
    }
    [SerializeField] DETECTSTATE detectState = DETECTSTATE.UNDETECT;
    public DETECTSTATE DetectState { get { return detectState; } }

    protected float preDetectTime;//最後に遭遇したタイミング
    [SerializeField] float preDetectLength;//警戒状態が解かれるまでの時間
    

    #region キャッシュ
    Player player;
    Transform tr;
    Transform plTr;
    public Animator animator { get; private set; }
    AnimatorStateInfo stateInfo;
    AudioSource audioSource;
    #endregion

    #region TagCheckArea
    [SerializeField] protected TagCheckArea eyeCheckArea;
    [SerializeField] protected TagCheckArea eyeCheckArea_canHide;
    [SerializeField] protected TagCheckArea detectCheckArea;
    #endregion

    [SerializeField] protected float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }


    [SerializeField] MomijiAnim myMomiji;
    #region　外部用フラグ
    public bool IsChengedDetectState { get; private set; }
    [SerializeField] bool attackNow = false;
    public bool AttackNow { get {return attackNow; } }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        plTr = player.transform;
        tr = transform;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void CharacterUpdate()
    {
        if (nonActive || plTr == null) return;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);//アニメーション情報の取得
        if (IsDead())
        {
            DeadAction();
            return;
        }
    }
    #region detect
    /// <summary>
    /// detectStateの値の書き換え
    /// </summary>
    /// <param name="state"></param>
    public void SetDetectState(DETECTSTATE state)
    {
        if (detectState != state)
        {
            IsChengedDetectState = true;
            StartCoroutine(WaitFrameAction(1, () => IsChengedDetectState = false));
        }
        detectState = state;
    }

    /// <summary>
    /// 発見時に呼ばれる関数
    /// </summary>
    public void DetectAction()
    {
        SetDirectionToPl();
        animator.SetTrigger("detect");
        StopMove();
    }

    /// <summary>
    /// 視界の中にいるかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsInSight()
    {
        bool result = false;
        switch (battleType)
        {
            case BATTLETYPE.DAMAGED_ATTACK://攻撃されたら戦闘モード
                switch (detectState)
                {
                    #region DAMAFE_ATTACK
                    case DETECTSTATE.UNDETECT:
                    case DETECTSTATE.PREDETECT:
                        break;
                    case DETECTSTATE.DETECT:
                        if (detectCheckArea.InternalTarget) result = true;
                        break;
                        #endregion
                }
                break;
            case BATTLETYPE.DETECT_ATTACK://発見したら戦闘モード
                switch (detectState)
                {
                    #region DETECT_ATTACT
                    case DETECTSTATE.UNDETECT:
                        if (eyeCheckArea.InternalTarget
                            || (eyeCheckArea_canHide.InternalTarget && !player.hiding)) result = true;
                        break;
                    case DETECTSTATE.PREDETECT:
                        if (detectCheckArea.InternalTarget && !player.hiding
                            || eyeCheckArea.InternalTarget) result = true;
                        break;
                    case DETECTSTATE.DETECT:
                        if (detectCheckArea.InternalTarget) result = true;
                        break;
                        #endregion
                }
                break;
        }
        return result;
    }

    /// <summary>
    /// 警戒状態が解かれる時間かどうか
    /// </summary>
    /// <returns></returns>
    public bool IsEndPreDetect()
    {
        return Time.fixedTime > preDetectLength + preDetectTime;
    }
    
    /// <summary>
    /// 前回発見した時間の更新
    /// </summary>
    public void PredetectUpdate()
    {
        preDetectTime = Time.fixedTime;
    }
    #endregion
    #region 動作関連
    #region 動作系
    protected override void Move(float speed)
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        animator.SetFloat("move", Mathf.Abs(speed / moveSpeed));
    }

    public void Move_force(float speed)
    {
        Move(speed);
    }
    public void StopMove()
    {
        Move(0);
    }

    public bool MoveToPlayer_X(float distance_target, float speed)
    {
        return MoveToTarget_X(distance_target,plTr.position,speed);
    }

    public bool MoveToTarget_X(float distanceTarget, Vector2 targetPos, float speed)
    {
        float distance = Mathf.Abs(targetPos.x - tr.position.x);
        if (distance < distanceTarget)
        {
            Move(0);
            return true;
        }
        else
        {
            SetDirectionToTarget(targetPos);
            float dir = Mathf.Sign(-tr.localScale.x);
            Move(speed * dir);

            return false;
        }
    }
    /// <summary>
    /// プレイヤーのほうを向く
    /// </summary>
    public void SetDirectionToPl()
    {
        SetDirectionToTarget(plTr.position);
    }
    public void SetDirectionToTarget(Vector2 pos)
    {
        if (!IsTargetFront(pos))
        {
            tr.localScale = new Vector2(tr.localScale.x * -1, tr.localScale.y);
        }
    }
    /// <summary>
    /// spriteのグローバル座標をそのままに、ローカル座標を元に戻す
    /// </summary>
    protected void ReplaceBodyPosition()
    {
       /* Vector2 temp = spriteBodyTr.position;
        tr.position += spriteBodyTr.localPosition;
        spriteBodyTr.position = temp;*/
    }
    #endregion
    #region Get系
    protected float GetDistancePlayer_X()
    {
        return Mathf.Abs(plTr.position.x - tr.position.x);
    }
    #endregion
    #region bool
    /// <summary>
    /// プレイヤーが自分が向いている方向にいるか
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerFront()
    {
        return IsTargetFront(plTr.position);
    }

    public bool IsTargetFront(Vector2 pos)
    {
        float dir = Mathf.Sign(pos.x - tr.position.x);

        if (dir == 1)
        {
            if (tr.localScale.x < 0) return true;
            else return false;
        }
        else
        {
            if (tr.localScale.x < 0) return false;
            else return true;
        }
    }
    #endregion
    #endregion
    #region 特定状況の関数
    #region dead
    /// <summary>
    /// 死んだときのアクション
    /// </summary>
    protected virtual void DeadAction()
    {
        if (!stateInfo.IsTag("Dead"))
        {
            animator.SetTrigger("dead");
            myMomiji.BreakMomiji();
            StopMove();
        }
    }

    public bool IsDeadSelf()
    {
        return IsDead();
    }
    #endregion
    #region Damage

    public void DamageAction(int damage)
    {
        Damage(damage);
        animator.SetTrigger("damage");
        //DamageDetectAction();
        SendMessage_OnReciveDamage();
    }
    #endregion
    #endregion
    /// <summary>
    /// 指定時間後に関数を実行
    /// </summary>
    public IEnumerator WaitTimeAction(float f, UnityAction ua)
    {
        yield return new WaitForSeconds(f);
        ua.Invoke();
    }
    /// <summary>
    /// 指定時間後に関数を実行
    /// </summary>
    public IEnumerator WaitFrameAction(int n, UnityAction ua)
    {
        for(int i = 0; i < n; i++)
        {
            yield return null;
        }
        ua.Invoke();
    }
    #region sendMessage
    void SendMessage_OnReciveDamage()
    {
        ExecuteEvents.Execute<ReciveInterFace_damage>(
            target: gameObject,
            eventData: null,
            functor: (reciever, eventData) => reciever.OnReciveDamage()
            );
    }
    
    #endregion
}
