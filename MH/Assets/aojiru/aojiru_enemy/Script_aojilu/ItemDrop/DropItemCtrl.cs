using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class DropItemCtrl : MonoBehaviour
    {
        [SerializeField] EnemyController myCharcter;
        [SerializeField] Transform dropPos;
        [SerializeField] GameObject dropItem;

        private void Awake()
        {
            myCharcter = GetComponent<EnemyController>();
        }

        bool droped=false;
        private void Update()
        {
            if (dropItem == null) return;
            if (IsDead_target()&&!droped)
            {
                Drop();
            }
        }

        void Drop()
        {
            GameObject obj = Instantiate(dropItem, dropPos.position, Quaternion.identity);
            droped = true;
        }

        bool IsDead_target()
        {
            return myCharcter.IsDeadSelf();
        }
    }
}
