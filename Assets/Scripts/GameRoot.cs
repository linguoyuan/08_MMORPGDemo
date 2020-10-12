using UnityEngine;
using PEProtocol;
using System;

public class GameRoot : MonoBehaviour {

    private static GameRoot _Single = null;
    private GameRoot()
    {     
        _Single = this;
    }

    public static GameRoot Single
    {
        get
        {
            return _Single;
        }
    }

    public LoginSys loginSys;
    public DynamicWnd dynamicWnd;

    void Start ()
    {
        DontDestroyOnLoad(this);
        Debug.Log("Game start ...");
        ClearUIRoot();
        Init();
	}

    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        dynamicWnd.SetWndState();
    }

    public void SetPlayerDataByFBStart(RspFBFight data)
    {
        playerData.power = data.power;
    }

    private void Init()
    {
        //服务模块初始化
        TimerSvc timerSvc = GetComponent<TimerSvc>();
        timerSvc.InitSvc();
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.Init();

        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainCitySys mainCity = GetComponent<MainCitySys>();
        mainCity.InitSys();
        FubenSys fubenSys = GetComponent<FubenSys>();
        fubenSys.InitSys();
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();

        //配置文件系统初始化
        XmlConfigSvc xml = GetComponent<XmlConfigSvc>();
        xml.InitRDNameCfg(PathDefine.RDNameCfg);
        xml.InitMapCfg(PathDefine.MapCfg);
        xml.InitGuideCfg(PathDefine.GuideCfg);
        xml.InitStrongCfg(PathDefine.StrongCfg);
        xml.InitTaskRewardCfg(PathDefine.TaskCfg);

        //test
        //GameObject cube = res.LoadPrefab(PathDefine.cube, true);

        //进入登陆场景并加载场景资源
        login.EnterLogin();
    }

    public static void AddTips(string tips)
    {
        Single.dynamicWnd.AddTips(tips);
    }

    private PlayerData playerData = null;
    public PlayerData PlayerData
    {
        get
        {
            return playerData;
        }
    }

    public void SetPlayerData(RspLogin data)
    {
        playerData = data.playerData;
    }

    public void SetPlayerName(string name)
    {
        PlayerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data)
    {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.guideid = data.guideid;
    }

    public void SetPlayerDataByStrong(RspStrong data)
    {
        PlayerData.coin = data.coin;
        PlayerData.crystal = data.crystal;
        PlayerData.hp = data.hp;
        PlayerData.ad = data.ad;
        PlayerData.ap = data.ap;
        PlayerData.addef = data.addef;
        PlayerData.apdef = data.apdef;

        PlayerData.strongArr = data.strongArr;
    }

    public void SetPlayerDataByBuy(RspBuy data)
    {
        PlayerData.diamond = data.dimond;
        PlayerData.coin = data.coin;
        PlayerData.power = data.power;
    }

    public void SetPlayerDataByPower(PshPower data)
    {
        PlayerData.power = data.power;
    }

    public void SetPlayerDataByTask(RspTakeTaskReward data)
    {
        PlayerData.coin = data.coin;
        PlayerData.lv = data.lv;
        PlayerData.exp = data.exp;
        PlayerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByTaskPsh(PshTaskPrgs data)
    {
        PlayerData.taskArr = data.taskArr;
    }
}
