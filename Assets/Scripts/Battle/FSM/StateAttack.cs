public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Attack;
        PECommon.Log("Enter StateAttack.");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        entity.canControl = true;
        entity.SetAction(Constants.ActionDefault);
        PECommon.Log("Exit StateAttack.");
    }

    public void Process(EntityBase entity, params object[] args)
    {       
        //技能表现 放在处理后面处理
        //entity.AttackEffect((int)args[0]);

        PECommon.Log("Process StateAttack." + "args[0] = " + args[0]);
        //entity.SetAction(1);--test

        //1、状态机处理伤害逻辑
        entity.SkillAttack((int)args[0]);
    }
}
