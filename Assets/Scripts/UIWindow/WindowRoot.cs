using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    protected void SetSprite(Image img, string path)
    {
        Sprite sp = resSvc.LoadSprite(path, true);
        img.sprite = sp;
    }
    #endregion


    #region Click Evts
    protected void OnClick(GameObject go, Action<object> cb, object args)
    {
        PEListener listener = Tils.GetOrAddComponect<PEListener>(go);
        listener.onClick = cb;
        listener.args = args;
    }

    protected void OnClickDown(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = Tils.GetOrAddComponect<PEListener>(go);
        listener.onClickDown = cb;
    }

    protected void OnClickUp(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = Tils.GetOrAddComponect<PEListener>(go);
        listener.onClickUp = cb;
    }

    protected void OnDrag(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = Tils.GetOrAddComponect<PEListener>(go);
        listener.onDrag = cb;
    }
    #endregion
}
