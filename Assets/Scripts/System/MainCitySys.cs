using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCitySys : SystemRoot
{
    public MainCityWnd mainCityWnd;
    public LoadingWnd loadingWnd;
    private MapCfg mapData;
    private PlayerController playerCtrl;
    public static MainCitySys Instance;

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

        //TODO 设置人物展示相机
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

        
    }

    public void SetMoveDir(Vector2 dir)
    {
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
}
