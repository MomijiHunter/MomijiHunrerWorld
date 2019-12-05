using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class TagCheckArea : AbstractCheckInternalArea
    {
        
        Vector2 size;
        Vector2 offset;

        [SerializeField] string[] tags;

        [SerializeField] bool debug_colEnable;//コライダーの確認用
        
        protected override void InitAction()
        {
            base.InitAction();
            BoxCollider2D col = GetComponent<BoxCollider2D>();
            size = col.size;
            offset = col.offset;
            col.enabled = (debug_colEnable)?true:false;
        }

        protected override bool CheckInternal()
        {
            Collider2D[] cols = Physics2D.OverlapBoxAll(tr.position + (Vector3)offset, size, 0);

            bool localDetect = false;
            foreach (var obj in cols)
            {
                if (CheckTag(obj.tag)) localDetect = true;
            }
            return localDetect;
        }
        

        bool CheckTag(string tag)
        {
            foreach (var data in tags)
            {
                if (data == tag) return true;
            }
            return false;
        }
    }
}
