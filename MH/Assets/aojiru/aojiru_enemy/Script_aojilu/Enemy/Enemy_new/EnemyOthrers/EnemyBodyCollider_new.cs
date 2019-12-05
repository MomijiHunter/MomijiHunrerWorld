using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;

namespace aojilu
{
    public class EnemyBodyCollider_new : MonoBehaviour
    {
        [SerializeField] EnemyController enemyCtrl;
        Collider2D mycollider;

        private void Start()
        {
            mycollider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (enemyCtrl.IsDeadSelf()) {
                mycollider.enabled = false;
            }

        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.tag== "PlayerAttack")
            {
                var data = col.GetComponent<PlayerAttack>();
                if (data == null)
                {
                    enemyCtrl.DamageAction(1);
                }
                else
                {
                    enemyCtrl.DamageAction(data.attackPower);
                }
            }
        }
    }
}
