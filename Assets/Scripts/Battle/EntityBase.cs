

using System;
using UnityEngine;

public abstract class EntityBase
{
    public AniState currentAniState = AniState.None;

    public BattleMgr battleMgr = null;
    public StateMgr stateMgr = null;//状态机
    public Controller controller = null;//移动相关
    public SkillMgr skillMgr = null;//技能表现

    public bool canControl = true;

    public void Move()
    {
        stateMgr.ChangeStatus(this, AniState.Move);
    }

    public void Idle()
    {
        stateMgr.ChangeStatus(this, AniState.Idle);
    }

    public void Attack(int skillID)
    {
        stateMgr.ChangeStatus(this, AniState.Attack, skillID);
    }

    public void Hit()
    {
        stateMgr.ChangeStatus(this, AniState.Hit, null);
    }
    public void Die()
    {
        stateMgr.ChangeStatus(this, AniState.Die, null);
    }

    public void Born()
    {
        stateMgr.ChangeStatus(this, AniState.Born, null);
    }

    public virtual void SetFX(string fx, int skillTime)
    {
        if (controller != null)
        {
            controller.SetFX(fx, skillTime);
        }
        else
        {
            Debug.Log("---controller = null");
        }
    }

    public virtual void SetBlend(float blend)
    {
        if (controller != null)
        {
            controller.SetBlend(blend);
        }
    }

    public virtual void SetDir(Vector2 dir)
    {
        if (controller != null)
        {
            controller.Dir = dir;
        }
    }

    public virtual void SetAction(int act)
    {
        if (controller != null)
        {
            controller.SetAction(act);
        }
    }

    public virtual void AttackEffect(int skillID)
    {
        if (skillMgr != null)
        {
            skillMgr.EffectSkillAttack(this, skillID);
        }
        else
        {
            Debug.Log("skillMgr = null");
        }
    }

    public virtual void SetSkillMoveState(bool move, float speed = 0f)
    {
        if (controller != null)
        {
            controller.SetSkillMoveState(move, speed);
        }
    }

    public virtual Vector2 GetDirInput()
    {
        return Vector2.zero;
    }


    private int hp;
    public int HP
    {
        get
        {
            return hp;
        }

        set
        {
            //通知UI层TODO
            PECommon.Log("hp change:" + hp + " to " + value);
            hp = value;
        }
    }
    private BattleProps props;
    public BattleProps Props
    {
        get
        {
            return props;
        }

        protected set
        {
            props = value;
        }
    }
    public virtual void SetBattleProps(BattleProps props)
    {
        HP = props.hp;
        Props = props;
    }


    public virtual void SkillAttack(int skillID)
    {
        //2、实体通过技能管理器引用，调用伤害逻辑
        skillMgr.SkillAttack(this, skillID);
    }

    public virtual Vector3 GetPos()
    {
        return controller.transform.position;
    }

    public virtual Transform GetTrans()
    {
        return controller.transform;
    }
}
