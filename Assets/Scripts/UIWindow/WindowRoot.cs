using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    [HideInInspector]
    public ResSvc resSvc = null;
    [HideInInspector]
    public AudioSvc audioSvc = null;
    [HideInInspector]
    public XmlConfigSvc xmlCfgSvc = null;
    [HideInInspector]
    public NetSvc netSvc = null;

    public void SetWndState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }

        if (isActive)
        {
            InitWnd();
        }
        else
        {

        }
    }

    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Single;
        audioSvc = AudioSvc.Single;
        xmlCfgSvc = XmlConfigSvc.Single;
        netSvc = NetSvc.Single;
    }

    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        xmlCfgSvc = null;
        netSvc = null;
    }

    #region tools
    /*
    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }

    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }

    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    */
    #endregion
}
