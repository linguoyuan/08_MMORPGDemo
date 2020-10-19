using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    public LoadingWnd loadingWnd;

    private ResSvc resSvc;
    private AudioSvc audioSvc;
    private XmlConfigSvc xmlConfigSvc;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;
    private MapCfg mapCfg;

    private EntityPlayer entitySelfPlayer;
    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();

    public void Init(int mapid)
    {
        resSvc = ResSvc.Single;
        audioSvc = AudioSvc.Single;
        audioSvc = AudioSvc.Single;
        xmlConfigSvc = XmlConfigSvc.Single;
        loadingWnd = GameRoot.Single.loginSys.loadingWnd;

        //初始化各管理器
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();

        mapCfg = xmlConfigSvc.GetMapCfgData(mapid);

        Debug.Log("scene id : " + mapCfg.sceneName);
        loadingWnd.SetWndState();
        resSvc.AsyncLoadScene(mapCfg.sceneName, loadingWnd.SetProgress, () =>
        {
            LoadSceneDone(mapCfg);
        });
    }

    private void LoadSceneDone(MapCfg mapData)
    {
        loadingWnd.SetWndState(false);
        //初始化地图数据
        GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
        mapMgr = map.GetComponent<MapMgr>();
        if (mapMgr == null)
        {
            Debug.Log("mapMgr == null");
        }
        mapMgr.Init(this);

        map.transform.localPosition = Vector3.zero;
        map.transform.localScale = Vector3.one;

        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        LoadPlayer(mapData);
        entitySelfPlayer.Idle();

        audioSvc.PlayBgMusic(Constants.BGHuangYe);

        BattleSys.Instance.SetPlayerCtrlWndState(true);

        //激活怪物
        ActiveCurrentBatchMonsters();
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnBattlePlayerPrefab);
        if (player == null)
        {
            Debug.Log("player == null");
        }
        
        entitySelfPlayer = new EntityPlayer
        {
            battleMgr = this,
            stateMgr = stateMgr,
            skillMgr = skillMgr
        };

        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        if (playerCtrl == null)
        {
            Debug.Log("playerCtrl == null");
        }
        playerCtrl.Init();
        entitySelfPlayer.controller = playerCtrl;
        Debug.Log("set controller succeed");
        entitySelfPlayer.currentAniState = AniState.Idle;
        entitySelfPlayer.SetAction(Constants.ActionDefault);

        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one*0.5f;
        Debug.Log("player.transform.position = " + player.transform.position);
    }

    //记载怪物
    public void LoadMonsterByWaveID(int wave)
    {
        for (int i = 0; i < mapCfg.monsterLst.Count; i++)
        {
            MonsterData md = mapCfg.monsterLst[i];
            
            if (md.mWave == wave)
            {
                GameObject m = resSvc.LoadPrefab(md.mCfg.resPath, true);
                m.transform.localPosition = md.mBornPos;
                m.transform.localEulerAngles = md.mBornRote;
                m.transform.localScale = Vector3.one;

                m.name = "m" + md.mWave + "_" + md.mIndex;

                EntityMonster em = new EntityMonster
                {
                    battleMgr = this,
                    stateMgr = stateMgr,
                    skillMgr = skillMgr
                };
                //设置初始属性
                em.md = md;
                if (md.mCfg.bps == null)
                {
                    Debug.Log("md.mCfg.bps == null");
                }
                em.SetBattleProps(md.mCfg.bps);

                MonsterController mc = m.GetComponent<MonsterController>();
                mc.Init();
                em.controller = mc;

                m.SetActive(false);
                monsterDic.Add(m.name, em);
            }
        }
    }

    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        //设置玩家移动
        //PECommon.Log(dir.ToString());

        if (entitySelfPlayer.canControl == false)
        {
            return;
        }

        if (dir == Vector2.zero)
        {
            entitySelfPlayer.Idle();
        }
        else
        {
            entitySelfPlayer.Move();
            entitySelfPlayer.SetDir(dir);
        }
    }

    public void ReqReleaseSkill(int index)
    {
        switch (index)
        {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
        }
    }


    private void ReleaseNormalAtk()
    {
        PECommon.Log("Click Normal Atk");
    }

    private void ReleaseSkill1()
    {
        PECommon.Log("Click Skill1");
        entitySelfPlayer.Attack(101);
    }
    private void ReleaseSkill2()
    {
        PECommon.Log("Click Skill2");
    }
    private void ReleaseSkill3()
    {
        PECommon.Log("Click Skill3");
    }

    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    }

    public List<EntityMonster> GetEntityMonsters()
    {
        List<EntityMonster> monsterLst = new List<EntityMonster>();
        foreach (var item in monsterDic)
        {
            monsterLst.Add(item.Value);
        }
        return monsterLst;
    }

    public void ActiveCurrentBatchMonsters()
    {
        TimerSvc.Instance.AddTimeTask((int tid) => {
            foreach (var item in monsterDic)
            {
                item.Value.controller.gameObject.SetActive(true);
                item.Value.Born();
                TimerSvc.Instance.AddTimeTask((int id) => {
                    //出生1秒钟后进入Idle
                    item.Value.Idle();
                }, 1000);
            }
        }, 500);
    }
}
