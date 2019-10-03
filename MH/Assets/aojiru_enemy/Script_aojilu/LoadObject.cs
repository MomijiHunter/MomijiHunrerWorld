using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace aojilu {
    public class LoadObject : MonoBehaviour
    {
        [SerializeField] LoadCtrl load;
        [SerializeField] Transform frontTr;
        public Transform FrontTr { get { return frontTr; } }

        [SerializeField] LoadObject chengeTarget;
        [SerializeField] LoadObject[] nextObj;

        CinemachineVirtualCamera cm;

        private void Awake()
        {
            load = GameObject.FindGameObjectWithTag("LoadPanel").GetComponent<LoadCtrl>();
        }

        private void Start()
        {
            cm = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        }



        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                var plTr = col.transform;
                load.AddBlackAction("Load", () => ChengeToTarget(plTr));
                load.AddBlackAction("Load2", () => StartCoroutine(MoveCameraOnLoad()));
                load.FadeAction();
            }
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.tag == "Enemy")
            {
                var eb = col.gameObject.GetComponent<EnemyBase>();
                if (eb.ChengeAreaEnable && eb.AiState == EnemyBase.AISTATE.MAPCHENGE)
                {
                    ChengeToTarget(col.transform);
                    eb.SetChengeMapData(RandomObj());
                }
            }
        }

        void ChengeToTarget(Transform tr)
        {
            tr.position = chengeTarget.frontTr.position;
        }

        LoadObject RandomObj()
        {
            int r = (int)Random.Range(0, nextObj.Length);
            return nextObj[r];
        }

        IEnumerator MoveCameraOnLoad()
        {
            cm.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
            cm.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
            yield return new WaitForSeconds(0.1f);
            cm.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1;
            cm.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 3.3f;
        }
    }
}
