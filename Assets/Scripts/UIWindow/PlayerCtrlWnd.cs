using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerCtrlWnd : WindowRoot
{   
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Transform expPrgTrans;

    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;
    public Vector2 currentDir;

    protected override void InitWnd()
    {
        base.InitWnd();

        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        //defaultPos = imgDirBg.transform.position;
        //Tils.SetActive(imgDirPoint, false);

        //RegisterTouchEvts();

        RefreshUI();
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Single.PlayerData;

        Tils.SetText(txtLevel, pd.lv);
        Tils.SetText(txtName, pd.name);

        #region Expprg
        int expPrgVal = (int)(pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv) * 100);
        Tils.SetText(txtExpPrg, expPrgVal + "%");
        int index = expPrgVal / 10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();

        float globalRate = 1.0F * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalRate;
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);

        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }
        #endregion    
    }

    /*
    public void RegisterTouchEvts()
    {
        OnClickDown(imgTouch.gameObject, (PointerEventData evt) => 
        {
            startPos = evt.position;
            Tils.SetActive(imgDirPoint);
            imgDirBg.transform.position = evt.position;
        });
        OnClickUp(imgTouch.gameObject, (PointerEventData evt) => 
        {
            imgDirBg.transform.position = defaultPos;
            Tils.SetActive(imgDirPoint, false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            //BattleSys.Instance.SetMoveDir(Vector2.zero);
        });
        OnDrag(imgTouch.gameObject, (PointerEventData evt) => 
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imgDirPoint.transform.position = evt.position;
            }
            //BattleSys.Instance.SetMoveDir(dir.normalized);
        });
    }
    */

    public void ClickNormalAtk()
    {
        BattleSys.Instance.ReqReleaseSkill(0);
    }
    public void ClickSkill1Atk()
    {
        BattleSys.Instance.ReqReleaseSkill(1);
    }
    public void ClickSkill2Atk()
    {
        BattleSys.Instance.ReqReleaseSkill(2);
    }
    public void ClickSkill3Atk()
    {
        BattleSys.Instance.ReqReleaseSkill(3);
    }

    
}