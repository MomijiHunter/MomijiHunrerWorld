using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdditionalAction_SE : UIAdditionalActon_base
{
    [SerializeField] AudioClip[] myClips;
    [SerializeField] AudioSource source;

    public void PlayClip(int i)
    {
        if (i < myClips.Length)
        {
            source.PlayOneShot(myClips[i]);
        }
    }
 }
