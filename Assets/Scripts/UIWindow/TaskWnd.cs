using UnityEngine.UI;
using UnityEngine;
using PEProtocol;
using System.Collections.Generic;
using System;

public class TaskWnd : WindowRoot
{
    public Button btnClose;
    public Transform scrollTrans;

    private PlayerData pd = null;
    private List<TaskRewardData> trdLst = new List<TaskRewardData>();//任务奖励列表

    protected override void InitWnd()
    {
        base.InitWnd();
        Debug.Log("Open TaskWnd");
        btnClose.onClick.AddListener(ClickCloseBtn);

        pd = GameRoot.Single.PlayerData;
        RefreshUI();
    }

    public void RefreshUI()
    {
        trdLst.Clear();

        List<TaskRewardData> todoLst = new List<TaskRewardData>();
        List<TaskRewardData> doneLst = new List<TaskRewardData>();

        //1|0|0
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            TaskRewardData trd = new TaskRewardData
            {
                ID = int.Parse(taskInfo[0]),
                prgs = int.Parse(taskInfo[1]),
                taked = taskInfo[2].Equals("1")
            };

            if (trd.taked)
            {
                doneLst.Add(trd);
            }
            else
            {
                todoLst.Add(trd);
            }
        }

        trdLst.AddRange(todoLst);
        trdLst.AddRange(doneLst);

        //防止每次刷新一次就多一次重复的任务
        for (int i = 0; i < scrollTrans.childCount; i++)
        {
            Destroy(scrollTrans.GetChild(i).gameObject);
        }

        for (int i = 0; i < trdLst.Count; i++)
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.TaskItemPrefab);
            go.transform.SetParent(scrollTrans);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.name = "taskItem_" + i;

            TaskRewardData trd = trdLst[i];
            TaskRewardCfg trf = xmlCfgSvc.GetTaskRewardCfg(trd.ID);

            Tils.SetText(Tils.GetTrans(go.transform, "txtName"), trf.taskName);
            Tils.SetText(Tils.GetTrans(go.transform, "txtPrg"), trd.prgs + "/" + trf.count);
            Tils.SetText(Tils.GetTrans(go.transform, "txtExp"), "经验" + trf.exp);
            Tils.SetText(Tils.GetTrans(go.transform, "txtCoin"), "金币" + trf.coin);
            Image imgPrg = Tils.GetTrans(go.transform, "preBar/preVal").GetComponent<Image>();
            float prgVal = trd.prgs * 1.0f / trf.count;
            imgPrg.fillAmount = prgVal;

            Button btnTake = Tils.GetTrans(go.transform, "btnTake").GetComponent<Button>();

            if (btnTake == null)
            {
                Debug.Log("父物体：" + go.name);
                Debug.Log("btnTake == null");
            }
            else
            {
                //btnTake.onClick.AddListener(ClickTakeBtn);//这种写法不能传参
                btnTake.onClick.AddListener(() =>
                {
                    ClickTakeBtn(go.name);
                });
            }
            

            Transform transComp = Tils.GetTrans(go.transform, "imgTake");
            if (trd.taked)
            {
                btnTake.interactable = false;
                Tils.SetActive(transComp);
            }
            else
            {
                Tils.SetActive(transComp, false);
                if (trd.prgs == trf.count)
                {
                    btnTake.interactable = true;
                }
                else
                {
                    btnTake.interactable = false;
                }
            }

        } 
    }

    private void ClickTakeBtn(string name)
    {
        string[] nameArr = name.Split('_');
        int index = int.Parse(nameArr[1]);
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqTakeTaskReward,
            reqTakeTaskReward = new ReqTakeTaskReward
            {
                rid = trdLst[index].ID
            }
        };

        netSvc.SendMsg(msg);

        TaskRewardCfg trc = xmlCfgSvc.GetTaskRewardCfg(trdLst[index].ID);
        int coin = trc.coin;
        int exp = trc.exp;
        GameRoot.AddTips(Constants.Color("获得奖励：", TxtColor.Blue) + Constants.Color(" 金币 +" + coin + " 经验 +" + exp, TxtColor.Green));
    }

    private void ClickCloseBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
        SetWndState(false);
    }
}
