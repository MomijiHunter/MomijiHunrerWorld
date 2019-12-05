using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class LayCastObject : MonoBehaviour
    {
        [SerializeField] Transform stratPos;
        [SerializeField] Vector2 direction;
        [SerializeField] float distance;
        RaycastHit2D hit;
        #region Debug
        [SerializeField] bool isDrawRay;
        [SerializeField] bool useBeforeHit;//何にも衝突できなかった時に代理の者を返すかどうか
        [SerializeField] bool useBeforeHitLog;
        #endregion
        [SerializeField] string[] tags;
        [SerializeField] LayerMask mask;

        Vector2? beforeHit;//前回衝突したTr

        private void Update()
        {
            if (isDrawRay)
            {
                Debug.DrawRay(stratPos.position, direction * distance, Color.blue);
            }
        }
        /// <summary>
        /// rayを発射してぶつかったtransformを返す
        /// </summary>
        /// <returns></returns>
        public Vector2? GetLayTransform()
        {
            hit = Physics2D.Raycast(stratPos.position, direction, distance, mask);
            if (CheckTags(hit.collider))
            {
                beforeHit = hit.point;
                return hit.point;
            }
            else
            {
                if (useBeforeHitLog && useBeforeHit) Debug.Log(gameObject.name+" : layCastObj use beforeHit");
                return (useBeforeHit)?beforeHit:null;
            }
        }

        bool CheckTags(Collider2D col)
        {
            if (col == null) return false;
            bool result = false;
            foreach (var tag in tags)
            {
                if (col.tag == tag) result = true;
            }
            return result;
        }
    }
}
