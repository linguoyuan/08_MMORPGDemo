using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot {
    public Text txtTips;
    public Image imageFG;
    public Image imagePoint;
    public Text txtPrg;
    private float fgWidth;
    float posX;
    protected override void InitWnd()
    {
        base.InitWnd();
        fgWidth = imageFG.GetComponent<RectTransform>().sizeDelta.x;
        //txtTips.text = "这是个游戏提示";
        Tils.SetText(txtTips, "这是个游戏提示");
        //txtPrg.text = "0%";
        Tils.SetText(txtPrg, "0%");
        imageFG.fillAmount = 0;
        imagePoint.transform.localPosition = new Vector3(-550f, 0, 0);
    }

    public void SetProgress(int prg)
    {
        txtPrg.text = prg.ToString() + "%"; ;
        imageFG.fillAmount = (float)prg/100;//prg需要强制转换一下，不然得不到小数

        posX = prg * fgWidth - 550;
        imagePoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
    }

    public void LoadDone()
    {
        base.SetWndState(false);
        GameRoot.Single.loginSys.loginWnd.SetWndState(true);
        audioSvc.PlayBgMusic(Constants.BGLogin);
    }
}
