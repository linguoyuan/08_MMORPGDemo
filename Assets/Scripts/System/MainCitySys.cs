using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PEProtocol;

public class MainCitySys : SystemRoot
{
    public MainCityWnd mainCityWnd;
    public LoadingWnd loadingWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;

    private MapCfg mapData;
    private PlayerController playerCtrl;
    private CharacterController characterController;
    public static MainCitySys Instance;

    private Transform[] npcPosTrans;
    GameObject map;
    MainCityMap mcm;

    public override void InitSys()
    {
        Instance = this;
        base.InitSys();
        Debug.Log("Init MainCitySys");        
    }

    /// <summary>
    /// 进入主城场景
    /// </summary>
    public void EnterMainCity()
    {
        //此时还没加载好主城场景，不能使用主城场景的游戏物体
        mapData = xmlConfigSvc.GetMapCfgData(Constants.MainCityMapID);
        mainCityWnd.SetWndState();
        //resSvc.AsyncLoadScene(Constants.SceneMainCity, loadingWnd.SetProgress, LoadMainCityDone);
        resSvc.AsyncLoadScene(mapData.sceneName, loadingWnd.SetProgress, LoadMainCityDone);
    }

    private void LoadMainCityDone()
    {
        loadingWnd.SetWndState(false);
        // 加载游戏主角
        StartCoroutine(LoadPlayer(mapData));
        //LoadPlayer(mapData);

        //打开主城场景UI
        mainCityWnd.SetWndState();

        //播放主城背景音乐
        audioSvc.PlayBgMusic(Constants.BGMainCity);

        //设置人物展示相机
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
        }
    }

    private IEnumerator LoadPlayer(MapCfg mapData)
    //private void LoadPlayer(MapCfg mapData)//这里需要让场景加载完成后再实例化主角，不然实例化不出来
    {
        yield return new WaitForSeconds(0.2f);
        //GameObject player = resSvc.LoadPrefab(PathDefine.AssissnCityPlayerPrefab, true);      
        //实例化的时候需要给位置，不然实例化出来，人物会掉下去
        GameObject player = resSvc.LoadPrefabByPos(PathDefine.AssissnCityPlayerPrefab, mapData.playerBornPos, true);      
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5F, 1.5F, 1.5F);

        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        Animator animator = player.GetComponent<Animator>();
        animator.applyRootMotion = true;
        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        nav = player.GetComponent<NavMeshAgent>();
        characterController = player.GetComponent<CharacterController>();

        //获取主城NPC位置
        map = GameObject.FindGameObjectWithTag("MapRoot");
        if (map == null)
        {
            Debug.Log("map == null");
        }
        mcm = map.GetComponent<MainCityMap>();
        npcPosTrans = mcm.NpcPosTrans;
    }

    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();
        if (dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendWalk);
        }
        playerCtrl.Dir = dir;
    }


    #region Info Wnd
    private Transform charCamTrans;
    public void OpenInfoWnd()
    {
        
        StopNavTask();
        
        if (charCamTrans == null)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }

        //设置人物展示相机相对位置
        charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        charCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        
        infoWnd.SetWndState();
    }

    public void CloseInfoWnd()
    {
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }

    private float startRoate = 0;
    public void SetStartRoate()
    {
        startRoate = playerCtrl.transform.localEulerAngles.y;
    }

    public void SetPlayerRoate(float roate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRoate + roate, 0);
    }
    #endregion

    #region Guide Wnd
    private NavMeshAgent nav;
    private bool isNavGuide = false;
    private AutoGuideCfg curtTaskData;
    public void RunTask(AutoGuideCfg agc)
    {
        if (agc != null)
        {
            curtTaskData = agc;
        }

        //解析任务数据
        nav.enabled = true;
        Debug.Log("curtTaskData.npcID = " + curtTaskData.npcID);
        if (curtTaskData.npcID != -1)
        {
            float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[agc.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                nav.isStopped = true;
                characterController.enabled = true;
                playerCtrl.SetBlend(Constants.BlendIdle);
                nav.enabled = false;

                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                nav.enabled = true;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.SetDestination(npcPosTrans[agc.npcID].position);
                playerCtrl.SetBlend(Constants.BlendWalk);
            }
        }
        else
        {
            OpenGuideWnd();
        }
    }

    private void Update()
    {
        if (isNavGuide)
        {
            characterController.enabled = false;
            IsArriveNavPos();
            playerCtrl.SetCam();
        }
    }

    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curtTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
            nav.enabled = false;

            OpenGuideWnd();
        }
    }

    private void StopNavTask()
    {
        characterController.enabled = true;
        Debug.Log("isNavGuide = " + isNavGuide);
        if (isNavGuide)
        {
            isNavGuide = false;           
            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    private void OpenGuideWnd()
    {
        guideWnd.SetWndState();
    }

    public AutoGuideCfg GetCurtTaskData()
    {
        return curtTaskData;
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;

        GameRoot.AddTips(Constants.Color("任务奖励 金币+" + curtTaskData.coin + "  经验+" + curtTaskData.exp, TxtColor.Blue));

        switch (curtTaskData.actID)
        {
            case 0:
                //与智者对话
                break;
            case 1:
                //TODO 进入副本
                break;
            case 2:
                //TODO 进入强化界面
                break;
            case 3:
                //TODO 进入体力购买
                break;
            case 4:
                //TODO 进入金币铸造
                break;
            case 5:
                //TODO 进入世界聊天
                break;
        }
        GameRoot.Single.SetPlayerDataByGuide(data);
        mainCityWnd.RefreshUI();
    }
    #endregion
}
