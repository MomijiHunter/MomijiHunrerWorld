using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;
using UnityEngine.Events;

namespace aojilu
{
    public class EnemyBase : CharBase
    {
        public enum DETECTSTATE
        {
            DETECT,UNDETECT
        }
        [SerializeField]protected DETECTSTATE detectState=DETECTSTATE.UNDETECT;
        public DETECTSTATE DetectState { get { return detectState; } }
        [SerializeField] protected PlCheckArea eyeCheckArea;
        [SerializeField] protected PlCheckArea eyeCheckArea_canHide;
        [SerializeField] protected PlCheckArea detectCheckArea;
        [SerializeField] MomijiAnim myMomiji;

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

        [SerializeField] protected float moveSpeed;

        //=========================
        Player player;
        Transform tr;
        protected  Animator animator { get; private set; }
        Transform plTr;
        AudioSource audioSource;
        protected AnimatorStateInfo stateInfo;
        [SerializeField] BoxCollider2D footCollider;
        [SerializeField] protected Transform effectPos;
        [SerializeField] GameObject[] effects;
        [SerializeField] AudioClip[] SEs;
        //========================
        float? dashModeSpeed=null;
        public bool isGroundead { get; private set; }
        [SerializeField] bool nonActive;
        [SerializeField] bool stateLog;
        [SerializeField] protected bool chengeAreaEnable;
        public bool ChengeAreaEnable { get { return chengeAreaEnable; } }

        List<UnityAction> AIActionList=new List<UnityAction>();
        int AIactionIndex;

        [SerializeField]protected  GameObject deadItem;
        [SerializeField] protected Transform dropPos;
        int deadItemCount;

        protected float preDetectTime;
        [SerializeField]float preDetectLength;

        float mapChengeStartTime;
        [SerializeField]float nextChengeLength;
        [SerializeField]protected LoadObject nextLordObj;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        protected virtual void Init()
        {
            player = GameObject.Find("PlayerAll").GetComponent<Player>();
            tr = transform;
            plTr = player.transform;
            animator = GetComponent<Animator>();
            footCollider.enabled = false;
            AIActionList.Add(() => AIAction_undetect());
            AIActionList.Add(() => AIAction());
            AIactionIndex = 0;
            mapChengeStartTime = Time.fixedTime;
            preDetectTime = -preDetectLength;
            audioSource = GetComponent<AudioSource>();
        }

        protected override void CharacterUpdate()
        {
            if (nonActive||plTr==null) return;

            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (IsDead())
            {
                DeadAction();
                return;
            }
            ChengeMap();
            isGroundead = CheckGranudead();
            CheckAction();
            animator.SetBool("isGrounded", isGroundead);
            AIActionList[AIactionIndex].Invoke();


            if (dashModeSpeed != null)
            {
                Move((float)dashModeSpeed);
            }

            if (!stateInfo.IsTag("attack"))
            {
                animator.SetBool("attackEnd", false);
            }
        }

        void ChengeMap()
        {
            if (nextChengeLength < 0) return;
            if (Time.fixedTime > mapChengeStartTime + nextChengeLength)
            {
                chengeAreaEnable = true;
            }
            else
            {
                chengeAreaEnable = false;
            }
        }

        public void SetChengeMapData(LoadObject obj)
        {
            nextLordObj = obj;
            mapChengeStartTime = Time.fixedTime;
            SetAIState(AISTATE.AISELECT, 1.0f);
            chengeAreaEnable = false;
        }

        bool CheckGranudead()
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(footCollider.transform.position, footCollider.size,0);
            foreach(var obj in cols)
            {
                if (obj.tag == "Ground") return true;
            }
            return false;
        }
        /// <summary>
        /// aiの内部処理。子で実装
        /// </summary>
        protected virtual void AIAction()
        {
            if (Time.fixedTime > aiStartTime + aiWaitLength)
            {
                aiStartTime = 0;
                SetAIState(AISTATE.AISELECT, 1.0f);
            }
        }

        protected virtual void AIAction_undetect()
        {
            if (Time.fixedTime > aiStartTime + aiWaitLength)
            {
                aiStartTime = 0;
                SetAIState(AISTATE.AISELECT, 1.0f);
            }
        }
        
        protected void SetAIIndex(int i)
        {
            AIactionIndex = i;
        }

        /// <summary>
        /// 死んだときのアクション
        /// </summary>
        protected virtual void DeadAction()
        {
            if (!stateInfo.IsTag("Dead")) animator.SetTrigger("dead");
            //Destroy(gameObject);
            myMomiji.BreakMomiji();
        }

        protected virtual void MakeDeadItem()
        {
            if (deadItemCount != 0||deadItem==null) return;
            Instantiate(deadItem, dropPos.position, Quaternion.identity);
            deadItemCount++;
        }

        protected virtual void CheckAction()
        {
            if (Time.fixedTime < preDetectTime + preDetectLength)
            {
                if (detectState == DETECTSTATE.UNDETECT)
                {
                    if (chengeAreaEnable) return;
                    if (detectCheckArea.InternalTarget&&!player.hiding)
                    {
                        detectState = DETECTSTATE.DETECT;
                        animator.SetTrigger("detect");
                    }
                }
            }
            else
            {

                if (detectState == DETECTSTATE.UNDETECT)
                {
                    if (chengeAreaEnable) return;
                    if (eyeCheckArea.InternalTarget
                        ||(eyeCheckArea_canHide.InternalTarget&&!player.hiding))
                    {
                        detectState = DETECTSTATE.DETECT;
                        animator.SetTrigger("detect");
                    }
                }
                else if (detectState == DETECTSTATE.DETECT)
                {
                    if (!detectCheckArea.InternalTarget)
                    {
                        preDetectTime = Time.fixedTime;
                        detectState = DETECTSTATE.UNDETECT;
                    }
                }
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
            /*if (stateLog)
            {
                Debug.Log(state);
            }*/
        }

        protected void ExtendStateTime(float t)
        {
            aiStartTime = Time.fixedTime;
            aiWaitLength = t;
        }
        
        protected override void Move(float speed)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            animator.SetFloat("move", Mathf.Abs( speed / moveSpeed));
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

        public bool MoveToTarget(float distanceTarget,Vector2 targetPos,float speed)
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

        protected void StopMove()
        {
            Move(0);
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

        public void DamageAction(int damage)
        {
            Damage(damage);
            animator.SetTrigger("damage");
            if (detectState == DETECTSTATE.UNDETECT)
            {
                DamageDetectAction();
                animator.SetTrigger("detect");
            }
        }

        /// <summary>
        /// 敵を未発見でダメージを受けた場合
        /// </summary>
        protected virtual void DamageDetectAction()
        {
            detectState = DETECTSTATE.DETECT;
        }

        public void AttackEnd()
        {
            animator.SetBool("attackEnd",true);
            dashModeSpeed = null;
            StopMove();
            SetAIState(AISTATE.WAIT, 1.0f);
        }

        public void SetAttackEnd(float time)
        {
            StartCoroutine( WaitTime(time, () => AttackEnd()));
        }

        public void SetDashModeFront(float speed)
        {
            dashModeSpeed = speed*Mathf.Sign(-tr.localScale.x);
        }

         protected IEnumerator WaitTime(float f, UnityAction ua)
        {
            yield return new WaitForSeconds(f);
            ua.Invoke();
        }

        public void InstantAnim_setParent(int i)
        {
            GameObject obj = Instantiate(effects[i], effectPos.position, Quaternion.identity);
            obj.transform.localScale *= Mathf.Sign(tr.localScale.x);
            obj.transform.SetParent(effectPos);
        }
        public void InstantAnim(int i)
        {
            GameObject obj = Instantiate(effects[i], effectPos.position, Quaternion.identity);
            var scale = obj.transform.localScale;
            scale.x*= Mathf.Sign(tr.localScale.x);
            obj.transform.localScale = scale;
        }

        public void BreakAnim()
        {
            foreach(Transform obj in effectPos)
            {
                Destroy(obj.gameObject);
            }
        }

        public void BreakAnim_setTime(float f)
        {
            StartCoroutine(WaitTime(f, () => BreakAnim()));
        }

        public void PlaySE_oneShot(int i)
        {
            audioSource.PlayOneShot(SEs[i]);
        }
    }
}
