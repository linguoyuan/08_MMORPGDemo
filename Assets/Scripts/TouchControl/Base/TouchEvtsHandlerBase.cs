﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchEvtsHandlerBase : MonoBehaviour
{
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;

    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;

    //public Action acDown;
    //public Action acClickup;
    //public Action acDrag;

    protected virtual void Start()
    {
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        Tils.SetActive(imgDirPoint, false);
        //RegisterTouchEvts(acDown, acClickup, acDrag);
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

    public void RegisterTouchEvts(Action acDown = null, Action acClickup = null, Action<Vector2> acDrag = null)
    {
        OnClickDown(imgTouch.gameObject, (PointerEventData evt) => 
        {
            startPos = evt.position;
            Tils.SetActive(imgDirPoint);
            imgDirBg.transform.position = evt.position;
            acDown?.Invoke();
        });

        OnClickUp(imgTouch.gameObject, (PointerEventData evt) => 
        {
            imgDirBg.transform.position = defaultPos;
            Tils.SetActive(imgDirPoint, false);
            imgDirPoint.transform.localPosition = Vector2.zero;
            //MainCitySys.Instance.SetMoveDir(Vector2.zero);
            acClickup?.Invoke();          
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
            //MainCitySys.Instance.SetMoveDir(dir.normalized);
            if (acDrag != null)
            {
                acDrag(dir.normalized);
            }
        });
    }
}
