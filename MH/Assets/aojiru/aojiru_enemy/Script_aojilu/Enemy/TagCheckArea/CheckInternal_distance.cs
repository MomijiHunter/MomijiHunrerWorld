using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace aojilu
{
    /// <summary>
    /// 距離で判定する
    /// </summary>
    public class CheckInternal_distance : AbstractCheckInternalArea
    {
        [SerializeField] BoxCollider2D checkCollider;
        [SerializeField] Vector2 checkDistance;//使用する距離
        [SerializeField] Transform checkTarget;//調べる対象
        [SerializeField] bool isTargetPlayer;
        [SerializeField] bool debug_colliderEnable;

        protected override void InitAction()
        {
            base.InitAction();
            checkDistance = checkCollider.size / 2 * checkCollider.transform.localScale;
            if (isTargetPlayer)
            {
                checkTarget = GameObject.Find("PlayerAll").transform;
            }
            checkCollider.enabled = (debug_colliderEnable) ? true : false;
        }

        protected override bool CheckInternal()
        {
            bool result = false;

            Vector2 distance=tr.position-checkTarget.position;
            distance.x = Mathf.Abs(distance.x);
            distance.y = Mathf.Abs(distance.y);
            if (distance.x < checkDistance.x 
                && distance.y < checkDistance.y)
            {
                result = true;
            }

            return result;
        }
    }
}
