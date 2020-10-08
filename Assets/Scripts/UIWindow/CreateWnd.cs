
using UnityEngine;
using UnityEngine.UI;
using PEProtocol;

public class CreateWnd : WindowRoot
{
    public Button btnRandomName;
    public Button btnEnterMainCity;
    public InputField iptName;
    protected override void InitWnd()
    {
        base.InitWnd();
        btnRandomName.GetComponent<Button>().onClick.AddListener(ClickRandomNameBtn);
        btnEnterMainCity.GetComponent<Button>().onClick.AddListener(ClickEnterMainCity);
        Debug.Log("iptName.text = " + iptName.text);
    }

    private void ClickRandomNameBtn()
    {
        iptName.text = xmlCfgSvc.GetRDNameData();
    }

    private void ClickEnterMainCity()
    {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);

        if (iptName.text != "")
        {
            //发送名字数据到服务器，登录主城
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename
                {
                    name = iptName.text
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("当前名字不符合规范");
        }
    }
}
