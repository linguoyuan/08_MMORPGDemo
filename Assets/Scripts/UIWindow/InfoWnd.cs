using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot {
    #region UI Define
    public RawImage imgChar;

    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;

    public Text txtJob;
    public Text txtFight;
    public Text txtHP;
    public Text txtHurt;
    public Text txtDef;

    public Button btnClose;

    public Button btnDetail;
    public Button btnCloseDetail;
    public Transform transDetail;

    public Text dtxhp;
    public Text dtxad;
    public Text dtxap;
    public Text dtxaddef;
    public Text dtxapdef;
    public Text dtxdodge;
    public Text dtxpierce;
    public Text dtxcritical;
    #endregion

    private Vector2 startPos;

    protected override void InitWnd()
    {
        base.InitWnd();
        RegTouchEvts();
        Tils.SetActive(transDetail, false);
        RefreshUI();
    }

    private void RegTouchEvts()
    {
        /*
        OnClickDown(imgChar.gameObject, (PointerEventData evt) => 
        {
            startPos = evt.position;
            MainCitySys.Instance.SetStartRoate();
        });
        OnDrag(imgChar.gameObject, (PointerEventData evt) => 
        {
            float roate = -(evt.position.x - startPos.x) * 0.4f;
            MainCitySys.Instance.SetPlayerRoate(roate);
        });
        */
    }

    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Single.PlayerData;
        Tils.SetText(txtInfo, pd.name + " LV." + pd.lv);
        Tils.SetText(txtExp, pd.exp + "/" + PECommon.GetExpUpValByLv(pd.lv));
        imgExpPrg.fillAmount = pd.exp * 1.0F / PECommon.GetExpUpValByLv(pd.lv);
        Tils.SetText(txtPower, pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power * 1.0F / PECommon.GetPowerLimit(pd.lv);

        Tils.SetText(txtJob, " 职业   暗夜刺客");
        Tils.SetText(txtFight, " 战力   " + PECommon.GetFightByProps(pd));
        Tils.SetText(txtHP, " 血量   " + pd.hp);
        Tils.SetText(txtHurt, " 伤害   " + (pd.ad + pd.ap));
        Tils.SetText(txtDef, " 防御   " + (pd.addef + pd.apdef));

        //detail TODO
        Tils.SetText(dtxhp, pd.hp);
        Tils.SetText(dtxad, pd.ad);
        Tils.SetText(dtxap, pd.ap);
        Tils.SetText(dtxaddef, pd.addef);
        Tils.SetText(dtxapdef, pd.apdef);
        Tils.SetText(dtxdodge, pd.dodge + "%");
        Tils.SetText(dtxpierce, pd.pierce + "%");
        Tils.SetText(dtxcritical, pd.critical + "%");

    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
        //Gam.CloseInfoWnd();
    }
    public void ClickDetailBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
        Tils.SetActive(transDetail);
    }

    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIMusic(Constants.UIClickBtn);
        Tils.SetActive(transDetail, false);
    }
}