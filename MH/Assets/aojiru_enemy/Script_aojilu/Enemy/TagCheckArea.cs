using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class TagCheckArea : MonoBehaviour
    {
        [SerializeField] bool internalTarget;
        public bool InternalTarget { get { return internalTarget; } }

        Vector2 size;
        Vector2 offset;
        Transform tr;

        [SerializeField] string[] tags;

        private void Awake()
        {
            internalTarget = false;
            BoxCollider2D col = GetComponent<BoxCollider2D>();
            size = col.size;
            offset = col.offset;
            //col.enabled = false;
            tr = transform;
        }

        private void Update()
        {

            Collider2D[] cols = Physics2D.OverlapBoxAll(tr.position+(Vector3)offset, size, 0);

            bool localDetect = false;
            foreach (var obj in cols)
            {
                if (CheckTag(obj.tag)) localDetect = true;
            }
            internalTarget = localDetect;
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
