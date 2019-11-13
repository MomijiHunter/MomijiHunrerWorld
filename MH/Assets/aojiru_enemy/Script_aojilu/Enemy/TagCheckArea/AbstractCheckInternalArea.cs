using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace aojilu
{
    /// <summary>
    /// ターゲットが特定の範囲の中にいるかどうかを取得するクラス
    /// </summary>
    public abstract class AbstractCheckInternalArea : MonoBehaviour
    {
        [SerializeField] bool checkActive=true;//スリープ状態の判定

        [SerializeField] bool internalTarget;
        public bool InternalTarget { get { return internalTarget; } }

        protected Transform tr { get; private set; }


        private void Awake()
        {
            internalTarget = false;
            tr = transform;
            InitAction();
        }

        virtual protected void InitAction()
        {
        }

        private void Update()
        {
            if (checkActive)
            {
                internalTarget = CheckInternal();
            }
            else
            {
                internalTarget = false;
            }
        }

        /// <summary>
        /// 内部にあるかどうかの判定を送る
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckInternal();

        /// <summary>
        /// スリープ状態にするかどうか
        /// </summary>
        /// <param name="awake"></param>
        public virtual void SetSleepState(bool sleep)
        {
            checkActive = !sleep;
        }
    }
}
