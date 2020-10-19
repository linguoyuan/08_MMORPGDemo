using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Animator ani;
    public CharacterController ctrl;
    private Vector2 dir = Vector2.zero;
    protected bool isMove = false;
    public Vector2 Dir
    {
        get
        {
            return dir;
        }

        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
    }

    protected bool skillMove = false;
    protected float skillMoveSpeed = 0f;
    protected TimerSvc timerSvc;
    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();
    public virtual void Init()
    {
        timerSvc = TimerSvc.Instance;
    }

    public virtual void SetAction(int act)
    {
        Debug.Log("------SetAction:" + act);
        ani.SetInteger("Action", act);
    }

    public virtual void SetBlend(float blend)
    {
        ani.SetFloat("Blend", blend);
    }

    public virtual void SetFX(string fx, float skillTime)
    {
        
    }

    public void SetSkillMoveState(bool move, float skillSpeed = 0f)
    {
        skillMove = move;
        skillMoveSpeed = skillSpeed;
    }
}
