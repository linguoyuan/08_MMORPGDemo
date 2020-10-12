using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : SystemRoot
{
    public static BattleSys Instance;

    public override void InitSys()
    {
        Instance = this;
        base.InitSys();
        Debug.Log("Init BattleSys");
    }

    public void StartBattle(int mapId)
    {
        Debug.Log("StartBattle  = " + mapId);
        GameObject go = new GameObject
        {
            name = "BattleRoot"
        };
        go.transform.SetParent(GameRoot.Single.transform);
        BattleMgr battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapId);
    }
}
