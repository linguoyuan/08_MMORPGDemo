using System;
using PEProtocol;

public class FubenSys : SystemRoot
{
    public static FubenSys Instance = null;

    public FubenWnd fubenWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init FubenSys...");
    }

    public void EnterFuben()
    {
        OpenFubenWnd();
    }

    #region Fuben Wnd
    public void OpenFubenWnd()
    {
        fubenWnd.SetWndState();
    }

    public void RspFBFight(GameMsg msg)
    {
        GameRoot.Single.SetPlayerDataByFBStart(msg.rspFBFight);
        MainCitySys.Instance.mainCityWnd.SetWndState(false);
        SetFubenWndState(false);
        BattleSys.Instance.StartBattle(msg.rspFBFight.fbid);
    }

    private void SetFubenWndState(bool isActive = true)
    {
        fubenWnd.SetWndState(isActive);
    }
    #endregion

}
