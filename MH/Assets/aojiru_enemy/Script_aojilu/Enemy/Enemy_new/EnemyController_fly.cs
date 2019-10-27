using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aojilu;

[RequireComponent(typeof(LayCastObject))]
public class EnemyController_fly : EnemyController
{
    LayCastObject myLayCast;
    float firstGravity;

    [SerializeField,Space(10)] float moveSpeedY;
    public float MoveSpeedY { get { return moveSpeedY; } }
    public bool IsFly { get { return rb.gravityScale == 0; } }

    protected override void Init()
    {
        base.Init();
        myLayCast = GetComponent<LayCastObject>();
        firstGravity = rb.gravityScale;
    }
    #region 動作系
    #region 動作
    void Move_Y(float speed)
    {
        rb.velocity = new Vector2(rb.velocity.x, speed);
        animator.SetFloat("flySpeed", Mathf.Abs(speed / moveSpeedY));
    }

    public void StopMove_Y()
    {
        Move_Y(0);
    }

    public bool MoveToTarget_Y(float higher,float speed)
    {
        float nowDist = GetDistanceGround() - higher;
        if (Mathf.Abs( nowDist) <= 0.5f)
        {
            StopMove_Y();
            tr.position = GetPostionFromGround(higher);
            return true;
        }
        else
        {
            if (nowDist>0)
            {
                Move_Y(-speed);
            }
            else
            {
                Move_Y(speed);
            }

            return false;
        }
    }

    public bool MoveToHigher(float higher, float speed)
    {
        if (GetDistanceGround() >= higher)
        {
            StopMove_Y();
            tr.position = GetPostionFromGround(higher);
            return true;
        }
        else
        {
            Move_Y(speed);
            return false;
        }
    }

    public bool MoveToLower(float lower, float speed)
    {
        if (GetDistanceGround() <= lower)
        {
            StopMove_Y();
            tr.position = GetPostionFromGround(lower);
            return true;
        }
        else
        {
            Move_Y(-speed);
            return false;
        }
    }
    /// <summary>
    /// 目的の逆方向を向きながら目的地まで下がる
    /// </summary>
    /// <returns></returns>
    public bool MoveToTarget_X_reverse(float distanceTarget, Vector2 targetPos, float speed)
    {
        float distance = Mathf.Abs(targetPos.x - tr.position.x);
        if (distance < distanceTarget)
        {
            Move(0);
            return true;
        }
        else
        {
            SetDirectionToTarget(targetPos,true);
            float dir = Mathf.Sign(-tr.localScale.x);
            Move(-speed * dir);

            return false;
        }
    }

    public void GravityOff()
    {
        rb.gravityScale = 0;
    }

    public void GravityOn()
    {
        rb.gravityScale = firstGravity;
    }
    #endregion
    #region Get系
    /// <summary>
    /// 地面からの距離を取得
    /// </summary>
    public float GetDistanceGround()
    {
        var targetT= myLayCast.GetLayTransform();
        return tr.position.y - ((Vector2)targetT).y;
    }

    /// <summary>
    /// 地面から指定の距離の位置を取得
    /// </summary>
    /// <param name="higher"></param>
    /// <returns></returns>
    Vector2 GetPostionFromGround(float higher)
    {
        var targetT = myLayCast.GetLayTransform();
        return new Vector2(tr.position.x, ((Vector2)targetT).y + higher);
    }

    /// <summary>
    /// プレーヤーのから指定距離距離をとるための位置を返す
    /// </summary>
    /// <returns></returns>
    public Vector2 GetPostionFromPl_NearMe(float dist)
    {
        Vector2 result = Vector2.zero;
        var plPos = GetPlPosition();
        if (plPos.x > tr.position.x)
        {
            result = plPos + new Vector2(-dist, 0);
        }
        else
        {
            result = plPos + new Vector2(dist, 0);
        }
        result.y = tr.position.y;
        
        return result;
    }

    public Vector2 GetPlPosition()
    {
        return plTr.position;
    }
    #endregion
    #endregion
}
