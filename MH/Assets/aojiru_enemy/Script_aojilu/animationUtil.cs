using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationUtil : MonoBehaviour
{
    float awakeTime;
    float? deadLength=null;

    Rigidbody2D rb;
    [SerializeField] AudioClip se;
    AudioSource audioSource;

    private void Awake()
    {
        awakeTime = Time.fixedTime;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (deadLength != null)
        {
            if (Time.fixedTime > awakeTime + deadLength)
            {
                DestSelf();
            }
        }
    }

    public void DestSelf()
    {
        Destroy(gameObject);
    }

    public void SetDedLength(float f)
    {
        deadLength = f;
    }

    public void SetVelocity(Vector2 speed)
    {
        rb.velocity = speed;
        var vec= transform.localScale;
        vec.x *= Mathf.Sign(-speed.x);
        transform.localScale = vec;
    }

    public void ActiveSet()
    {
        gameObject.SetActive(true);
    }
    public void ActiveSetFlase()
    {
        gameObject.SetActive(false);
    }

    public void PlaySE_oneShot()
    {
        audioSource.PlayOneShot(se);
    }

    public void PlaySE_loop()
    {
        audioSource.clip = se;
        audioSource.Play();
    }
}
