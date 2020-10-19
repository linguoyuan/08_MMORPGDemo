using UnityEngine;
using System;

public class CharTouchFuBen : TouchEvtsHandlerBase
{
    public Vector2 currentDir;
    protected override void Start()
    {
        base.Start();
        Action acDown = ActionClickDown;
        Action acClickUp = ActionClickUp;
        Action<Vector2> acDrag = ActionDrag;
        base.RegisterTouchEvts(acDown, acClickUp, acDrag);
    }

    private void ActionClickDown()
    {
        //MainCitySys.Instance.SetMoveDir(Vector2.zero);
    }

    private void ActionClickUp()
    {
        //MainCitySys.Instance.SetMoveDir(Vector2.zero);
        Debug.Log("click up");
        currentDir = Vector2.zero;
        BattleSys.Instance.playerCtrlWnd.currentDir = currentDir;
        BattleSys.Instance.SetMoveDir(currentDir);
    }

    private void ActionDrag(Vector2 dir)
    {
        currentDir = dir.normalized;
        BattleSys.Instance.playerCtrlWnd.currentDir = currentDir;
        BattleSys.Instance.SetMoveDir(currentDir);
    }
}
