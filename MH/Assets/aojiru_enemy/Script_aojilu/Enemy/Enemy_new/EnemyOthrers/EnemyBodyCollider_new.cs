using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;

namespace aojilu
{
    public class EnemyBodyCollider_new : MonoBehaviour
    {
        [SerializeField] EnemyController enemyCtrl;

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
