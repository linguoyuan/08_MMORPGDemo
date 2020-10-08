using UnityEngine;
using PEProtocol;

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


    //public LoadingWnd loadingWnd;
    public LoginSys loginSys;
    public DynamicWnd dynamicWnd;
    public MainCitySys mainCitySys;

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
	
    private void Init()
    {
        //服务模块初始化
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

        //配置文件系统初始化
        XmlConfigSvc xml = GetComponent<XmlConfigSvc>();
        xml.InitRDNameCfg(PathDefine.RDNameCfg);
        xml.InitMapCfg(PathDefine.MapCfg);

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
}
