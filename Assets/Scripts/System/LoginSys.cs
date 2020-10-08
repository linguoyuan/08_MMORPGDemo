using UnityEngine;
using PEProtocol;

public class LoginSys : SystemRoot {

    public LoginWnd loginWnd;
    public LoadingWnd loadingWnd;
    public CreateWnd createWnd;
    public override void InitSys()
    {
        base.InitSys();
        Debug.Log("Init LoginSys");
    }

    /// <summary>
    /// 进入登陆场景
    /// </summary>
    public void EnterLogin()
    {
        //显示出来加载页面
        loadingWnd.SetWndState();//等同于下面的两句
        //GameRoot.Single.loadingWnd.gameObject.SetActive(true);
        //GameRoot.Single.loadingWnd.InitWnd();

        //异步加载登陆场景//显示进度条//加载完成打开注册登陆页面
        resSvc.AsyncLoadScene(Constants.SceneLogin, loadingWnd.SetProgress, loadingWnd.LoadDone);            
    }

    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登录成功");
        GameRoot.Single.SetPlayerData(msg.rspLogin);
        Debug.Log("player name :" + msg.rspLogin.playerData.name);
        if (msg.rspLogin.playerData.name == "")
        {
            //打开角色创建界面
            createWnd.SetWndState();
        }
        else
        {
            Debug.Log("进入主城场景");
            //打开主城的界面
            GameRoot.Single.mainCitySys.mainCityWnd.SetWndState();
            //进入主城
            GameRoot.Single.mainCitySys.EnterMainCity();
        }

        //关闭登录界面
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Single.SetPlayerName(msg.rspRename.name);

        //跳转场景进入主城
        GameRoot.Single.mainCitySys.EnterMainCity();
        //打开主城的界面
        GameRoot.Single.mainCitySys.mainCityWnd.SetWndState();

        //关闭创建界面
        createWnd.SetWndState(false);
    }
}
