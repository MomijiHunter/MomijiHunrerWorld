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

        [SerializeField] bool isDrawRay;

        [SerializeField] string[] tags;
        [SerializeField] LayerMask mask;


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
                
                return hit.point;
            }
            else
            {
                return null;
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
