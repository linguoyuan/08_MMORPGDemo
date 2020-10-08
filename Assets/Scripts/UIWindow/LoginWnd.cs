using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot
{
    public InputField iptAcct;
    public InputField iptPass;
    public Button btnEnter;
    public Button btnNotice;

    protected override void InitWnd()
    {
        base.InitWnd();
        //获取b本地存储的账号密码
        if (PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
        {
            iptAcct.text = PlayerPrefs.GetString("Acct");
            iptPass.text = PlayerPrefs.GetString("Pass");
        }
        else
        {
            iptAcct.text = "";
            iptPass.text = "";
        }

        btnEnter.GetComponent<Button>().onClick.AddListener(ClickEnterBtn);
        btnNotice.GetComponent<Button>().onClick.AddListener(ClickNoticeBtn);
    }

    string _acct;
    string _pass;
    //更新本地存储的账号密码
    public void ClickEnterBtn()
    {
        Debug.Log("点击进入游戏");
        audioSvc.PlayUIMusic(Constants.UILoginBtn);
        _acct = iptAcct.text;
        _pass = iptPass.text;
        if (_acct != "" && _pass != "")
        {
            PlayerPrefs.SetString("Acct", _acct);
            PlayerPrefs.SetString("Pass", _acct);

            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqLogin,
                reqLogin = new ReqLogin
                {
                    acct = _acct,
                    pass = _pass
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("账号或密码为空");
        }
    }

    public void ClickNoticeBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
        GameRoot.AddTips("功能正在开发中....");
    }
}
