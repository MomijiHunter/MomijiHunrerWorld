using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kubota;

namespace kubota
{
    public class DropMomiji : MonoBehaviour
    {
        public float timer;
        public bool canPick = false;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(timer);
            canPick = true;
        }
    }
}