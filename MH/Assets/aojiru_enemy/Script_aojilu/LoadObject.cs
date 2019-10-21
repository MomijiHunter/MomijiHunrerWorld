using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        private void Awake()
        {
            load = GameObject.FindGameObjectWithTag("LoadPanel").GetComponent<LoadCtrl>();
        }



        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                var plTr = col.transform;
                load.AddBlackAction("Load", () => ChengeToTarget(plTr));
                load.FadeAction();
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
