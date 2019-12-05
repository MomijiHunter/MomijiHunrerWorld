using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class MomijiAnim : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] Transform makePos;
        [SerializeField] GameObject momijiPrefab;
        

        private void Awake()
        {
        }

        public void BreakMomiji()
        {
            animator.SetTrigger("break");
        }

        public void DestSelf()
        {
            Destroy(this.gameObject);
        }

        public void InstantMomijiFall()
        {
            GameObject obj = Instantiate(momijiPrefab, makePos.position, Quaternion.identity);
            obj.transform.localScale = transform.lossyScale;
            obj.GetComponent<Animator>().SetTrigger("fall");
        }

       
    }
}
