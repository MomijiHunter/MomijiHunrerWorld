using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationUtil : MonoBehaviour
{
    float awakeTime;
    float? deadLength=null;
    [SerializeField] bool rotateToMoveVector;

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

        if (rotateToMoveVector)
        {
            transform.rotation =  Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, GetVectorAngle(rb.velocity)),0.5f);
        }

        if (deadLength != null)
        {
            if (Time.fixedTime > awakeTime + deadLength)
            {
                DestSelf();
            }
        }
    }

    /// <summary>
    /// 自己破壊
    /// </summary>
    public void DestSelf()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 破壊されるまでの時間をセット
    /// </summary>
    /// <param name="f"></param>
    public void SetDedLength(float f)
    {
        deadLength = f;
    }

    float GetVectorAngle(Vector2 vec)
    {
        float result = Vector2.Angle(Vector2.up, vec);
        if ((vec.x >= 0 && vec.y >= 0)||(vec.x>=0&&vec.y<0)) result =360-result;
        return result-90;
    }

    /// <summary>
    /// 速度設定
    /// </summary>
    /// <param name="speed"></param>
    public void SetVelocity(Vector2 speed)
    {
        rb.velocity = speed;
        transform.rotation = Quaternion.Euler(0, 0, GetVectorAngle(rb.velocity));

        //var vec= transform.localScale;
        //vec.x *= Mathf.Sign(-speed.x);
        //transform.localScale = vec;
    }

    /// <summary>
    /// 重力設定
    /// </summary>
    /// <param name="f"></param>
    public void SetGravity(float f)
    {
        rb.gravityScale = f;
    }
    

    /// <summary>
    /// セットアクティブの設定
    /// </summary>
    public void ActiveSet()
    {
        gameObject.SetActive(true);
    }
    public void ActiveSetFlase()
    {
        gameObject.SetActive(false);
    }

    public GameObject InstanateObj(GameObject obj,Vector3 pos)
    {
       return Instantiate(obj, pos, Quaternion.identity);
    }

    //SEを鳴らす
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
