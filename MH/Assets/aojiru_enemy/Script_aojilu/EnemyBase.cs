using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;
using UnityEngine.Events;

namespace aojilu
{
    public class EnemyBase : CharBase
    {
        #region Debug

        [SerializeField] bool stateLog;//AIstateのログを出す
        [SerializeField] bool nonActive;//処理の停止
        #endregion

        public enum BATTLETYPE
        {
            DETECT_ATTACK,
            DAMAGED_ATTACK,
            //DETECT_ESCAPE,
            //DAMAGED_ESCAPE
        }
        [SerializeField] protected BATTLETYPE battleType = BATTLETYPE.DETECT_ATTACK;

        public enum DETECTSTATE
        {
            DETECT,UNDETECT,PREDETECT
        }
        [SerializeField] DETECTSTATE detectState=DETECTSTATE.UNDETECT;
        public DETECTSTATE DetectState { get { return detectState; } }
        protected float preDetectTime;//最後に遭遇したタイミング
        [SerializeField] float preDetectLength;//警戒状態が解かれるまでの時間



        public enum AISTATE
        {
            AISELECT,
            APPROACH_WALK,
            APPROACH_DASH,
            ESCAPE_DASH,
            ATTACK,
            MAPCHENGE,
            WAIT
        }
        protected AISTATE aiState;
        public AISTATE AiState { get { return aiState; } }
        
        
        float aiStartTime;//今のステイとになった時刻
        float aiWaitLength;//今のステイとを続ける時間

        //現在使用しているAI関数のリスト
        List<UnityAction> AIActionList = new List<UnityAction>();
        int AIactionIndex;

        //=========================
        Player player;
        Transform tr;
        protected  Animator animator { get; private set; }
        protected Transform plTr;
        AudioSource audioSource;
        protected AnimatorStateInfo stateInfo;
        [SerializeField] Transform spriteBodyTr;
        [SerializeField] BoxCollider2D footCollider;
        [SerializeField] protected Transform effectPos;
        [SerializeField] GameObject[] effects;
        [SerializeField] AudioClip[] SEs;


        //発見エリア
        [SerializeField] protected TagCheckArea eyeCheckArea;
        [SerializeField] protected TagCheckArea eyeCheckArea_canHide;
        [SerializeField] protected TagCheckArea detectCheckArea;

        [SerializeField] MomijiAnim myMomiji;
        
        //========================

        [SerializeField] protected float moveSpeed;
        float? dashModeSpeed=null;//nullじゃないなら、強制移動。アニメーションは別途
        public bool isGroundead { get; private set; }

        [SerializeField] bool chengeAreaEnable;//マップ移動フラグ
        public bool ChengeAreaEnable { get { return chengeAreaEnable; } }

        [SerializeField] string nowMapName;//現在のマップ名

        
        float mapChengeStartTime;//前回移動した時間
        [SerializeField] Vector2 nextChengeLengthArea;//次移動するまでの時間の最大値と最小値
        float nextChengeLength;//次移動するまでの時間
        [SerializeField]protected LoadObject nextLordObj;//次に移動する場所

        //乱数系
        float? randomFixedNumber = null;//乱数を固定したときの値
        protected bool IsFixedRandomNumber { get { return randomFixedNumber != null; } }


        #region 便利系
        [SerializeField] float upPlTime;
        protected float UpPlTime { get { return upPlTime; } }
        #endregion

        #region monoBehaviour
        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        protected virtual void Init()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            tr = transform;
            plTr = player.transform;
            animator = GetComponent<Animator>();
            footCollider.enabled = false;
            AIInit();
            mapChengeStartTime = Time.fixedTime;
            preDetectTime = -preDetectLength;
            audioSource = GetComponent<AudioSource>();

            nextChengeLength = GetNextMapChengeTime();
            if (nextLordObj != null)
            {
                SetNowMapName(nextLordObj.MyMapName);
            }
        }

        protected override void CharacterUpdate()
        {
            if (nonActive||plTr==null) return;

            stateInfo = animator.GetCurrentAnimatorStateInfo(0);//アニメーション情報の取得
            if (IsDead())//死亡判定
            {
                DeadAction();
                return;
            }
            ChengeMapStateUpdate();//マップ遷移するかの確認
            isGroundead = CheckGranudead();
            animator.SetBool("isGrounded", isGroundead);
            CheckAction();//発見処理
            AIUpdate();

            UtilUpdate();

            if (dashModeSpeed != null)
            {
                Move((float)dashModeSpeed);
            }

            if (!stateInfo.IsTag("attack"))
            {
                animator.SetBool("attackEnd", false);
            }
        }

        #endregion
        #region 便利変数

        /// <summary>
        /// 便利変数の更新処理
        /// </summary>
        void UtilUpdate()
        {
            UpPlTimeUpdate();
        }

        /// <summary>
        /// プレイヤーが自分より高い位置にいる時間を計測
        /// </summary>
        void UpPlTimeUpdate()
        {
            if (detectState == DETECTSTATE.DETECT)
            {
                if (IsTargetUpSelf(4.0f, plTr))
                {
                    upPlTime += Time.deltaTime;
                }
                else
                {
                    upPlTime = 0;
                }
            }
            else
            {
                upPlTime = 0;
            }
        }

        protected void ResetUpPlTime()
        {
            upPlTime = 0; ;
        }

        public bool GetIsDead()
        {
            return IsDead();
        }
        #endregion
        #region mapの移動関連
        /// <summary>
        /// 既定の時間が過ぎていたら移動フラグを立てる
        /// </summary>
        void ChengeMapStateUpdate()
        {
            if (nextChengeLength < 0) return;
            if (Time.fixedTime > mapChengeStartTime + nextChengeLength)
            {
                SetChengeAreaEnable(true);
            }
            else
            {
                SetChengeAreaEnable(false);
            }
        }

        void SetChengeAreaEnable(bool flag)
        {
            chengeAreaEnable = flag;
        }

        /// <summary>
        /// 移動した際の次に移動する場所の設定、移動したときに呼ばれる
        /// </summary>
        /// <param name="obj"></param>
        public void SetChengeMapData(LoadObject obj)
        {
            nextLordObj = obj;
            mapChengeStartTime = Time.fixedTime;
            SetAIState(AISTATE.AISELECT, 1.0f);

            nextChengeLength = GetNextMapChengeTime();
            SetNowMapName(obj.MyMapName);
        }


        /// <summary>
        /// 次に移動するまでの時間の取得
        /// </summary>
        /// <returns></returns>
        float GetNextMapChengeTime()
        {
            return Random.Range(nextChengeLengthArea.x, nextChengeLengthArea.y);
        }

        /// <summary>
        /// 現在いるマップの名前の設定
        /// </summary>
        /// <param name="s"></param>
        void SetNowMapName(string s)
        {
            nowMapName = s;
        }
        #endregion

        #region AI
        /// <summary>
        /// AI関連の初期化関数
        /// </summary>
        void AIInit()
        {
            AIActionList.Add(() => AIAction_undetect());
            AIActionList.Add(() => AIAction_detect());
            AIactionIndex = 0;
        }

        /// <summary>
        /// AI関連のアップデート処理
        /// </summary>
        void AIUpdate()
        {
            if (aiState == AISTATE.AISELECT)
            {
                switch (DetectState)
                {
                    case DETECTSTATE.DETECT:
                        SetAIIndex(1);
                        break;
                    case DETECTSTATE.UNDETECT:
                    case DETECTSTATE.PREDETECT:
                        SetAIIndex(0);
                        break;
                }

                if (chengeAreaEnable)
                {
                    MoveToTarget_X(1.0f, (Vector2)nextLordObj.transform.position, moveSpeed);
                    return;
                }


            }

            AIActionList[AIactionIndex].Invoke();//AI処理
        }
        /// <summary>
        /// aiの内部処理。子で実装
        /// </summary>
        protected virtual void AIAction_detect()
        {
            if (Time.fixedTime > aiStartTime + aiWaitLength)
            {
                aiStartTime = 0;
                SetAIState(AISTATE.AISELECT, 1.0f);
            }
        }
        /// <summary>
        /// 未発見時のai
        /// </summary>
        protected virtual void AIAction_undetect()
        {
            if (Time.fixedTime > aiStartTime + aiWaitLength)
            {
                aiStartTime = 0;
                SetAIState(AISTATE.AISELECT, 1.0f);
            }
        }
        

        /// <summary>
        /// aiStateの変更
        /// </summary>
        /// <param name="state"></param>
        /// <param name="t"></param>
        protected void SetAIState(AISTATE state, float t)
        {
            aiStartTime = Time.fixedTime;
            aiWaitLength = t;
            aiState = state;
            if (stateLog)
            {
                Debug.Log(state);
            }
        }
        
        /// <summary>
        /// aistateの時間の延長
        /// </summary>
        /// <param name="t"></param>
        protected void ExtendStateTime(float t)
        {
            aiStartTime = Time.fixedTime;
            aiWaitLength = t;
        }


        void SetAIIndex(int i)
        {
            AIactionIndex = i;
        }

        #region 乱数関連
        protected float GetAIRandaomNumver()
        {
            float result=0.0f;
            if (randomFixedNumber == null)
            {
                result = Random.Range(0, 100);
            }
            else
            {
                result = (float)randomFixedNumber;
            }
            return result;
        }

        protected void SetRandFiexed(float f)
        {
            randomFixedNumber = f;
        }

        protected void ResetRandFiexed()
        {
            randomFixedNumber = null;
        }
        #endregion
        #endregion
        #region 死亡時など特定の状況で呼ばれる関数
        /// <summary>
        /// 死んだときのアクション
        /// </summary>
        protected virtual void DeadAction()
        {
            if (!stateInfo.IsTag("Dead")) animator.SetTrigger("dead");
            //Destroy(gameObject);
            myMomiji.BreakMomiji();
        }
        

        public void DamageAction(int damage)
        {
            Damage(damage);
            animator.SetTrigger("damage");
            DamageDetectAction();
        }

        #endregion
        #region detectStateのコントロール

        /// <summary>
        /// ダメージを受けた場合のDetect処理
        /// </summary>
        void DamageDetectAction()
        {
            if (detectState == DETECTSTATE.UNDETECT || detectState == DETECTSTATE.PREDETECT)
            {
                detectState = DETECTSTATE.DETECT;
                SetDirectionToPl();
                animator.SetTrigger("detect");
                StopMove();
            }
        }

        /// <summary>
        /// detectの変更
        /// </summary>
        protected virtual void CheckAction()
        {
            if (chengeAreaEnable)
            {
                detectState = DETECTSTATE.PREDETECT;
                return;
            }
            switch (battleType)
            {
                case BATTLETYPE.DAMAGED_ATTACK://攻撃されたら戦闘モード
                    switch (detectState)
                    {
                        case DETECTSTATE.DETECT:
                            if (!detectCheckArea.InternalTarget) detectState = DETECTSTATE.UNDETECT;
                            break;
                    }
                    break;
                case BATTLETYPE.DETECT_ATTACK://発見したら戦闘モード
                    switch (detectState)
                    {
                        #region DETECT_ATTACT
                        case DETECTSTATE.UNDETECT:
                            if (eyeCheckArea.InternalTarget
                                || (eyeCheckArea_canHide.InternalTarget && !player.hiding))
                            {
                                detectState = DETECTSTATE.DETECT;
                                animator.SetTrigger("detect");
                            }
                            break;
                        case DETECTSTATE.PREDETECT:
                            if (Time.fixedTime > preDetectLength + preDetectTime)
                            {
                                detectState = DETECTSTATE.UNDETECT;
                            }
                            else
                            {
                                if (detectCheckArea.InternalTarget && !player.hiding
                                    || eyeCheckArea.InternalTarget)
                                {
                                    detectState = DETECTSTATE.DETECT;
                                    animator.SetTrigger("detect");
                                }
                            }
                            break;
                        case DETECTSTATE.DETECT:
                            if (!detectCheckArea.InternalTarget)
                            {
                                preDetectTime = Time.fixedTime;
                                detectState = DETECTSTATE.PREDETECT;
                            }
                            break;
                            #endregion
                    }
                    break;
            }

        }
        #endregion
        #region 動作関連
        protected override void Move(float speed)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            animator.SetFloat("move", Mathf.Abs( speed / moveSpeed));
        }

        protected void StopMove()
        {
            Move(0);
        }

        /// <summary>
        /// プレイヤーのほうを向く
        /// </summary>
        public void SetDirectionToPl(bool escape=false)
        {
            if (!IsPlayerFront()&&!escape)
            {
                tr.localScale = new Vector2(tr.localScale.x * -1, tr.localScale.y);
            }else if (IsPlayerFront() && escape)
            {
                tr.localScale = new Vector2(tr.localScale.x * -1, tr.localScale.y);
            }
        }

        public void SetDirectionToTarget(Vector2 pos)
        {
            if (!IsTargetFront(pos))
            {
                tr.localScale = new Vector2(tr.localScale.x * -1, tr.localScale.y);
            }
        }

        public bool MoveToPlayer_X(float distance_target, float speed, bool isEscape = false)
        {
            float distance =GetDistancePlayer_X();

            if ((distance < distance_target && !isEscape)
                || (distance > distance_target && isEscape))
            {
                Move(0);
                return true;
            }
            else
            {
                SetDirectionToPl(escape: isEscape);
                float dir = Mathf.Sign(-tr.localScale.x);
                Move(speed * dir);

                return false;
            }
        }

        public bool MoveToTarget_X(float distanceTarget,Vector2 targetPos,float speed)
        {
            float distance = Mathf.Abs( targetPos.x - tr.position.x);
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

        protected float GetDistancePlayer_X()
        {
            return Mathf.Abs(plTr.position.x - tr.position.x);
        }

        /// <summary>
        /// spriteのグローバル座標をそのままに、ローカル座標を元に戻す
        /// </summary>
        protected void ReplaceBodyPosition()
        {
            Vector2 temp = spriteBodyTr.position;
            tr.position += spriteBodyTr.localPosition;
            spriteBodyTr.position = temp;
        }

        #region Check系
        bool CheckGranudead()
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(footCollider.transform.position, footCollider.size, 0);
            foreach (var obj in cols)
            {
                if (obj.tag == "Ground") return true;
            }
            return false;
        }

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

        /// <summary>
        /// lengthよりターゲットが高い位置にいるか
        /// </summary>
        /// <param name="length"></param>
        /// <param name="_targetTr"></param>
        /// <returns></returns>
        public bool IsTargetUpSelf(float length,Transform _targetTr)
        {
            return (_targetTr.position.y - tr.position.y) > length;
        }
        #endregion
        #endregion
        #region アニメーション系
        /// <summary>
        /// 攻撃アニメーションを終了するためのフラグをonにする
        /// 攻撃アニメーションの最後に呼ぶ
        /// </summary>
        public void AttackEnd()
        {
            animator.SetBool("attackEnd",true);
            dashModeSpeed = null;
            StopMove();
            SetAIState(AISTATE.WAIT, 1.0f);
        }

        /// <summary>
        /// AttackEndを呼ぶまでの時間を設定する。
        /// ダッシュなどのループする攻撃で使用
        /// </summary>
        /// <param name="time"></param>
        public void SetAttackEnd(float time)
        {
            StartCoroutine( WaitTimeAction(time, () => AttackEnd()));
        }

        public void SetDashModeFront(float speed)
        {
            dashModeSpeed = speed*Mathf.Sign(-tr.localScale.x);
        }


        /// <summary>
        /// 自分の子としてエフェクトを作成
        /// </summary>
        /// <param name="i"></param>
        public void InstantEffect_setParent(int i)
        {
            GameObject obj = Instantiate(effects[i], effectPos.position, Quaternion.identity);
            var scale = obj.transform.localScale;
            scale.x*= Mathf.Sign(tr.localScale.x);
            obj.transform.localScale = scale;
            obj.transform.SetParent(effectPos);
        }
        public void InstantEffect(int i)
        {
            GameObject obj = Instantiate(effects[i], effectPos.position, Quaternion.identity);
            var scale = obj.transform.localScale;
            scale.x*= Mathf.Sign(tr.localScale.x);
            obj.transform.localScale = scale;
        }

        /// <summary>
        /// 保持しているエフェクトアニメーションの破壊
        /// </summary>
        public void BreakAnim()
        {
            foreach(Transform obj in effectPos)
            {
                Destroy(obj.gameObject);
            }
        }
        /// <summary>
        /// 指定時間後に破壊
        /// </summary>
        /// <param name="f"></param>
        public void BreakAnim_setTime(float f)
        {
            StartCoroutine(WaitTimeAction(f, () => BreakAnim()));
        }

        public void PlaySE_oneShot(int i)
        {
            audioSource.PlayOneShot(SEs[i]);
        }
        #endregion

        /// <summary>
        /// 指定時間後に関数を実行
        /// </summary>
        /// <param name="f"></param>
        /// <param name="ua"></param>
        /// <returns></returns>
        protected IEnumerator WaitTimeAction(float f, UnityAction ua)
        {
            yield return new WaitForSeconds(f);
            ua.Invoke();
        }

    }
}
