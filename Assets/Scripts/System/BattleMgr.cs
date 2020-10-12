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

        MapCfg mapData = xmlConfigSvc.GetMapCfgData(mapid);

        Debug.Log("scene id : " + mapData.sceneName);
        loadingWnd.SetWndState();
        resSvc.AsyncLoadScene(mapData.sceneName, loadingWnd.SetProgress, () =>
        {
            LoadSceneDone(mapData);
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
        mapMgr.Init();

        map.transform.localPosition = Vector3.zero;
        map.transform.localScale = Vector3.one;

        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        //LoadPlayer(mapData);

        audioSvc.PlayBgMusic(Constants.BGHuangYe);
    }
}
