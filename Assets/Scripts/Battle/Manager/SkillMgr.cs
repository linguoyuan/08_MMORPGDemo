using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    //技能数据读取
    private XmlConfigSvc xmlConfigSvc;
    //定时服务器调用
    private TimerSvc timerSvc;

    public void Init()
    {
        xmlConfigSvc = XmlConfigSvc.Single;
        timerSvc = TimerSvc.Single;
        PECommon.Log("Init SkillMgr Done.");
    }

    public void EffectSkillAttack(EntityBase entity, int skillID)
    {
        Debug.Log("播放技能效果");
        SkillCfg skillCfgData = xmlConfigSvc.GetSkillCfg(skillID);
        entity.SetAction(skillCfgData.aniAction);
        entity.SetFX(skillCfgData.fx, skillCfgData.skillTime);

        CalcSkillMove(entity, skillCfgData);
        entity.canControl = false;
        entity.SetDir(Vector2.zero);

        timerSvc.AddTimeTask((int tid) =>
        {
            entity.Idle();
        }, skillCfgData.skillTime);
    }

    
    public void SkillAttack(EntityBase entity, int skillID)
    {
        //3、技能管理器，调用对应的攻击方法
        AttackDamage(entity, skillID);
        EffectSkillAttack(entity, skillID);
    }

    public void AttackDamage(EntityBase entity, int skillID)
    {
        SkillCfg skillData = xmlConfigSvc.GetSkillCfg(skillID);
        List<int> actonLst = skillData.skillActionLst;
        int sum = 0;
        for (int i = 0; i < actonLst.Count; i++)
        {
            SkillActionCfg skillAction = xmlConfigSvc.GetSkillActionCfg(actonLst[i]);
            sum += skillAction.delayTime;
            int index = i;
            if (sum > 0)
            {
                timerSvc.AddTimeTask((int tid) => 
                {
                    //4、调用表现实体Controller去实现具体的
                    SkillAction(entity, skillData, index);
                }, sum);
            }
            else
            {
                //瞬时技能
                SkillAction(entity, skillData, index);
            }
        }
    }

    public void SkillAction(EntityBase caster, SkillCfg skillCfg, int index)
    {
        SkillActionCfg skillActionCfg = xmlConfigSvc.GetSkillActionCfg(skillCfg.skillActionLst[index]);

        int damage = skillCfg.skillDamageLst[index];
        //获取场景里所有的怪物实体，遍历运算
        List<EntityMonster> monsterLst = caster.battleMgr.GetEntityMonsters();
        for (int i = 0; i < monsterLst.Count; i++)
        {
            EntityMonster target = monsterLst[i];
            //判断距离，判断角度
            if (InRange(caster.GetPos(), target.GetPos(), skillActionCfg.radius)
                && InAngle(caster.GetTrans(), target.GetPos(), skillActionCfg.angle))
            {
                CalcDamage(caster, target, skillCfg, damage);
            }
        }
    }

    private bool InRange(Vector3 from, Vector3 to, float range)
    {
        float dis = Vector3.Distance(from, to);
        if (dis <= range)
        {
            return true;
        }
        return false;
    }

    private bool InAngle(Transform trans, Vector3 to, float angle)
    {
        if (angle == 360)
        {
            return true;
        }
        else
        {
            Vector3 start = trans.forward;
            Vector3 dir = (to - trans.position).normalized;

            float ang = Vector3.Angle(start, dir);

            if (ang <= angle / 2)
            {
                return true;
            }
            return false;
        }
    }

    System.Random rd = new System.Random();
    private void CalcDamage(EntityBase caster, EntityBase target, SkillCfg skillCfg, int damage)
    {
        if (caster == null)
        {
            Debug.Log("施法者实体为空");
            return;
        }

        if (target == null)
        {
            Debug.Log("target实体为空");
            //return;
        }

        int dmgSum = damage;
        if (skillCfg.dmgType == DamageType.AD)
        {
            //计算闪避
            int dodgeNum = PETools.RDInt(1, 100, rd);
            if (dodgeNum <= target.Props.dodge)
            {
                //UI显示闪避 TODO
                PECommon.Log("闪避Rate:" + dodgeNum + "/" + target.Props.dodge);
                return;
            }
            //计算属性加成
            dmgSum += caster.Props.ad;

            //计算暴击
            int criticalNum = PETools.RDInt(1, 100, rd);
            if (criticalNum <= caster.Props.critical)
            {
                float criticalRate = 1 + (PETools.RDInt(1, 100, rd) / 100.0f);
                dmgSum = (int)criticalRate * dmgSum;
                PECommon.Log("暴击Rate:" + criticalNum + "/" + caster.Props.critical);
            }

            //计算穿甲
            int addef = (int)((1 - caster.Props.pierce / 100.0f) * target.Props.addef);
            dmgSum -= addef;
        }
        else if (skillCfg.dmgType == DamageType.AP)
        {
            //计算属性加成
            dmgSum += caster.Props.ap;
            //计算魔法抗性
            dmgSum -= target.Props.apdef;
        }
        else { }

        //最终伤害
        if (dmgSum < 0)
        {
            dmgSum = 0;
            return;
        }

        if (target.HP < dmgSum)
        {
            target.HP = 0;
            //目标死亡
            target.Die();
        }
        else
        {
            target.HP -= dmgSum;
            target.Hit();
        }
        Debug.Log("最终伤害:" + dmgSum);
    }

    private void CalcSkillMove(EntityBase entity, SkillCfg skillData)
    {
        List<int> skillMoveLst = skillData.skillMoveLst;
        int sum = 0;
        Debug.Log("移动段位：" + skillMoveLst.Count);
        for (int i = 0; i < skillMoveLst.Count; i++)
        {
            SkillMoveCfg skillMoveCfg = xmlConfigSvc.GetSkillMoveCfg(skillData.skillMoveLst[i]);
            float speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);
            sum += skillMoveCfg.delayTime;
            if (sum > 0)
            {
                timerSvc.AddTimeTask((int tid) => 
                {
                    entity.SetSkillMoveState(true, speed);
                }, sum);
            }
            else
            {
                entity.SetSkillMoveState(true, speed);
            }

            sum += skillMoveCfg.moveTime;
            timerSvc.AddTimeTask((int tid) => 
            {
                entity.SetSkillMoveState(false);
            }, sum);
        }
    }
}