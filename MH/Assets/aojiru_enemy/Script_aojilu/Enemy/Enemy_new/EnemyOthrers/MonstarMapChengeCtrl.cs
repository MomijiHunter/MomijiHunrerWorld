using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class MonstarMapChengeCtrl : MonoBehaviour,ReciveInterFace_mapChenge
    {
        /// <summary>
        /// 次にマップを変更するまでの時間の範囲
        /// </summary>
        [SerializeField] Vector2 nextChengeLength;

        [SerializeField] bool mapChengeEnable;
        public bool MapChengeEnable { get { return mapChengeEnable; } }

        float nextMapChengeLength;
        float mapChengeStartTime;

        [SerializeField]MapParent nowMapParent;
        LoadObject nextLoadObject;

        private void Awake()
        {
            ResetMapChengeEnable();
        }

        private void Update()
        {
            if (Time.fixedTime > nextMapChengeLength + mapChengeStartTime)
            {
                SetMapChengeEnable(true);
            }
        }

        void SetMapChengeEnable(bool flag)
        {
            mapChengeEnable = flag;
        }

        void ResetMapChengeEnable()
        {
            mapChengeStartTime = Time.fixedTime;
            nextMapChengeLength = GetNextMapChengeTime();
            mapChengeEnable = false;
            nextLoadObject = null;
        }

        /// <summary>
        /// 次に移動するまでの時間の取得
        /// </summary>
        /// <returns></returns>
        float GetNextMapChengeTime()
        {
            return Random.Range(nextChengeLength.x, nextChengeLength.y);
        }

        /// <summary>
        /// マップ遷移する場所を返す
        /// </summary>
        public Vector2 GetNextLoadPosition()
        {
            if (nextLoadObject == null)
            {
                nextLoadObject = nowMapParent.GetRandomLoadObject();
            }
            return nextLoadObject.transform.position;
        }
        

        void SetMyMap(MapParent pa)
        {
            nowMapParent = pa;
        }


        public void OnReciveMapChenge(MapParent mapParent)
        {
            SetMyMap(mapParent);
            ResetMapChengeEnable();
        }
    }
}
