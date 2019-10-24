using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace aojilu
{
    public class EffectMaker : MonoBehaviour
    {
        [SerializeField] protected Transform effectPosition;

        [SerializeField] GameObject[] effectPrefabs;

        public GameObject InstantEffect(int i)
        {
            if (effectPrefabs.Length > i)
            {
                GameObject obj = Instantiate(effectPrefabs[i], effectPosition.position, Quaternion.identity);
                var scale = obj.transform.localScale;
                scale.x *= Mathf.Sign(transform.localScale.x);
                obj.transform.localScale = scale;
                return obj;
            }
            else
            {
                Debug.Log(gameObject.name + " : EffectMaker out of range");
                return null;
            }
        }

        public GameObject InstantEffect_setParent(int i)
        {
            var obj = InstantEffect(i);
            obj.transform.SetParent(effectPosition);
            return obj;
        }

        public void BreakAnim()
        {
            foreach (Transform obj in effectPosition)
            {
                Destroy(obj.gameObject);
            }
        }
        /// <summary>
        /// 指定時間後に破壊
        /// </summary>
        public void BreakAnim_setTime(float f)
        {
            StartCoroutine(WaitTimeAction(f, () => BreakAnim()));
        }

        /// <summary>
        /// 指定時間後に関数を実行
        /// </summary>
        protected IEnumerator WaitTimeAction(float f, UnityAction ua)
        {
            yield return new WaitForSeconds(f);
            ua.Invoke();
        }

    }
}
