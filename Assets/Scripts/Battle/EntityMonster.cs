//怪物逻辑实体类
public class EntityMonster : EntityBase
{
    public MonsterData md;
    public override void SetBattleProps(BattleProps props)
    {
        int level = md.mLevel;

        BattleProps p = new BattleProps
        {
            hp = props.hp * level,
            ad = props.ad * level,
            ap = props.ap * level,
            addef = props.addef * level,
            apdef = props.apdef * level,
            dodge = props.dodge * level,
            pierce = props.pierce * level,
            critical = props.critical * level
        };
        
        Props = p;
        HP = p.hp;
    }
}
