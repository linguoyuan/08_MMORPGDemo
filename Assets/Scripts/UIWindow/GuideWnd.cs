using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot
{
    public Text txtName;//当前说话的人
    public Text txtTalk;//当前说话的内容
    public Image imgIcon;//当前说话人的头像

    private PlayerData pd;
    private AutoGuideCfg curtTaskData;
    private string[] dialogArr;
    private int index;

    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Single.PlayerData;
        curtTaskData = MainCitySys.Instance.GetCurtTaskData();
        dialogArr = curtTaskData.dilogArr.Split('#');
        index = 1;

        SetTalk();
    }

    private void SetTalk()
    {
        string[] talkArr = dialogArr[index].Split('|');
        if (talkArr[0] == "0")
        {
            //自己
            SetSprite(imgIcon, PathDefine.SelfIcon);
            Tils.SetText(txtName, pd.name);
        }
        else
        {
            //对话NPC
            switch (curtTaskData.npcID)
            {
                case 0:
                    SetSprite(imgIcon, PathDefine.WiseManIcon);
                    Tils.SetText(txtName, "智者");
                    break;
                case 1:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    Tils.SetText(txtName, "将军");
                    break;
                case 2:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    Tils.SetText(txtName, "工匠");
                    break;
                case 3:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    Tils.SetText(txtName, "商人");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    Tils.SetText(txtName, "小芸");
                    break;
            }
        }

        imgIcon.SetNativeSize();
        Tils.SetText(txtTalk, talkArr[1].Replace("$name", pd.name));
    }


    public void ClickNextBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);

        index += 1;
        if (index == dialogArr.Length)
        {
            //TODO 发送任务引导完成信息
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqGuide,
                reqGuide = new ReqGuide
                {
                    guideid = curtTaskData.ID
                }
            };

            netSvc.SendMsg(msg);
            SetWndState(false);
        }
        else
        {
            SetTalk();
        }
    }
}
