using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace aojilu
{
    /// <summary>
    /// マップ1つごとにつける奴
    /// </summary>
    public class MapParent : MonoBehaviour
    {
        [SerializeField] LoadObject[] loadObjects;
        [SerializeField] public Transform centerPos;

        /// <summary>
        /// 自分の所持しているマップオブジェクトからランダムに1つ返す
        /// </summary>
        /// <returns></returns>
        public LoadObject GetRandomLoadObject()
        {
            int r = (int)Random.Range(0, loadObjects.Length);
            return loadObjects[r];
        }

        public LoadObject GetTargetObj(int n)
        {
            return loadObjects[n];
        }
    }
}
