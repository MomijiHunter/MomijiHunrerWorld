using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;
using UnityEngine.UI;

namespace kubota
{
    public class ManjuuIcon : MonoBehaviour
    {
        Player player;
        int numOfManjuu;
        [SerializeField] RectTransform manjuu;
        List<RectTransform> manjuuTransform = new List<RectTransform>();
        RectTransform thisRectTransform;

        private void Start()
        {
            thisRectTransform = GetComponent<RectTransform>();

            player = GameObject.Find("PlayerAll").GetComponent<Player>();
            numOfManjuu = player.remainItems;
            for (int i = 0; i < numOfManjuu; i++)
            {
                AddManjuu();
            }
        }

        public void AddManjuu()
        {
            var addedManjuu = Instantiate(manjuu);
            manjuuTransform.Add(addedManjuu);
            SetManjuu();
        }

        public void RemoveManjuu()
        {
            RectTransform removedManjuu = manjuuTransform[manjuuTransform.Count - 1];
            Destroy(removedManjuu.GetComponent<Image>());
            manjuuTransform.Remove(removedManjuu);
            Destroy(removedManjuu.gameObject);
            SetManjuu();
        }

        public void SetManjuu()
        {
            for (int i = 0; i < manjuuTransform.Count; i++)
            {
                manjuuTransform[i].position = new Vector3((thisRectTransform.position.x + 2) - (i + 1) * (4f / (manjuuTransform.Count + 1)), thisRectTransform.position.y, 0);
                manjuuTransform[i].transform.parent = transform;
            }
        }
    }
}
