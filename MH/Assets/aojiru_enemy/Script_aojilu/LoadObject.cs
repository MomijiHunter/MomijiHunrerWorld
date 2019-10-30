using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

namespace aojilu {
    public class LoadObject : MonoBehaviour
    {
        [SerializeField] LoadCtrl load;
        [SerializeField] Transform frontTr;
        public Transform FrontTr { get { return frontTr; } }

        [SerializeField] LoadObject chengeTarget;
        //[SerializeField] LoadObject[] nextObj;

        [SerializeField] MapParent myMap;
        public MapParent MyMap { get { return myMap; } }
        public string MyMapName { get { return myMap.name; } }

        CinemachineVirtualCamera playerCamera;

        private void Awake()
        {
            load = GameObject.FindGameObjectWithTag("LoadPanel").GetComponent<LoadCtrl>();
            playerCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        }



        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                var plTr = col.transform;
                load.AddBlackAction("Load", () => ChengeToTarget(plTr));
                load.FadeAction();
                StartCoroutine(SetPlayerCamera());
            }
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.tag == "Enemy")
            {
                var eb = col.gameObject.GetComponent<EnemyMain>();
                if (eb.MapChengeEnable && eb.AiState == EnemyMain.AISTATE.MAPCHENGE)
                {
                    ChengeToTarget(col.transform);
                    //eb.SetChengeMapData(RandomObj());
                    SendMessage_OnReciveMapChenge(col.gameObject);
                }
            }
        }

        void ChengeToTarget(Transform tr)
        {
            tr.position = chengeTarget.frontTr.position;
        }

        private IEnumerator SetPlayerCamera()
        {
            playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
            playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
            yield return new WaitForSeconds(0.5f);
            playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1;
            playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 3.3f;
        }

        /*LoadObject RandomObj()
        {
            int r = (int)Random.Range(0, nextObj.Length);
            return nextObj[r];
        }*/

        void SendMessage_OnReciveMapChenge(GameObject obj)
        {
            ExecuteEvents.Execute<ReciveInterFace_mapChenge>(
                target: obj,
                eventData: null,
                functor: (reciever, eventData) => reciever.OnReciveMapChenge(chengeTarget.MyMap)
                );
        }

    }
}
