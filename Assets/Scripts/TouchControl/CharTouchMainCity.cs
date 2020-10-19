using UnityEngine;
using System;

public class CharTouchMainCity : TouchEvtsHandlerBase
{
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
        MainCitySys.Instance.SetMoveDir(Vector2.zero);
    }

    private void ActionDrag(Vector2 dir)
    {
        MainCitySys.Instance.SetMoveDir(dir);
    }
}
