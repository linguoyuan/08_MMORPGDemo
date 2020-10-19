using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : SystemRoot
{
    public static BattleSys Instance;
    public PlayerCtrlWnd playerCtrlWnd;
    public BattleMgr battleMgr;

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
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapId);
    }

    public void SetPlayerCtrlWndState(bool isActive = true)
    {
        playerCtrlWnd.SetWndState(isActive);
    }

    public void SetMoveDir(Vector2 dir)
    {
        //Debug.Log("Battle play moving ...");
        battleMgr.SetSelfPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill(int index)
    {
        if (battleMgr == null)
        {
            Debug.Log("battleMgr == null");
        }
        Debug.Log("skill index = " + index);
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput()
    {
        return playerCtrlWnd.currentDir;
    }
}
