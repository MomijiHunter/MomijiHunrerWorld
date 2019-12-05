using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storyPanelAnimationEve : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject storyImage;
    [SerializeField] string animKey;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ActiveImage()
    {
        storyImage.SetActive(!storyImage.activeInHierarchy);
    }

    public void SetTrrigger()
    {
        anim.SetTrigger(animKey);
    }
}
