using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCityWnd : WindowRoot
{
    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    public Transform expPrgTrans;
    private float pointDis;

    //主城按钮显隐
    public Animation menuAni;
    public Button btnMenu;
    public Button btnHeadOpen;
    public Button btnStrongOpen;
    private bool menuState = true;

    //自动任务按钮
    public Button btnGuide;
    private AutoGuideCfg curtTaskData;

    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();

        btnMenu.GetComponent<Button>().onClick.AddListener(ClickMenuBtn);
        btnHeadOpen.GetComponent<Button>().onClick.AddListener(ClickHeadBtn);
        btnGuide.GetComponent<Button>().onClick.AddListener(ClickGuideBtn);
        btnStrongOpen.GetComponent<Button>().onClick.AddListener(ClickStrongBtn);
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Single.PlayerData;
        Tils.SetText(txtFight, PECommon.GetFightByProps(pd));
        Tils.SetText(txtPower, "体力:" + pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);
        Tils.SetText(txtLevel, pd.lv);
        Tils.SetText(txtName, pd.name);

        //经验条自适应
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalRate = 1.0F * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalRate;//当前屏幕相对于主分辨率后的宽度
        //1134-(72+9*2) = 108 
        float width = (screenWidth - 108) / 10;
        grid.cellSize = new Vector2(width, 7);

        //经验条百分比数值
        int expPrgVal = (int)(pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv) * 100);
        Tils.SetText(txtExpPrg, expPrgVal + "%");
        //经验条item显示
        int index = expPrgVal / 10;
        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;//前面的全部填充
            }
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;//取出余数部分作为当前格的百分比
            }
            else
            {
                img.fillAmount = 0;//其余的显示空经验条
            }
        }


        //设置自动任务图标
        curtTaskData = xmlCfgSvc.GetAutoGuideData(pd.guideid);
        if (curtTaskData != null)
        {
            SetGuideBtnIcon(curtTaskData.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }
    }

    /// <summary>
    /// 主城菜单控制按钮
    /// </summary>
    public void ClickMenuBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIExtenBtn);

        menuState = !menuState;
        AnimationClip clip = null;
        if (menuState)
        {
            clip = menuAni.GetClip("OpenMCMenu");
        }
        else
        {
            clip = menuAni.GetClip("CloseMCMenu");
        }
        menuAni.Play(clip.name);
    }

    public void ClickHeadBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }

    /// <summary>
    /// 动态更新新手引导头像
    /// </summary>
    /// <param name="npcID"></param>
    private void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        SetSprite(img, spPath);
    }

    public void ClickGuideBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);

        if (curtTaskData != null)
        {
            MainCitySys.Instance.RunTask(curtTaskData);
        }
        else
        {
            GameRoot.AddTips("更多引导任务，正在开发中...");
        }
    }

    private void ClickStrongBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
        MainCitySys.Instance.OpenStrongWnd();
    }
}
