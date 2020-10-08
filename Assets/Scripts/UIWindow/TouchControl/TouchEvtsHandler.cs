using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchEvtsHandler : MonoBehaviour
{
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;

    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;

    void Start()
    {
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        Tils.SetActive(imgDirPoint, false);
        RegisterTouchEvts();
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
            GameRoot.Single.mainCitySys.SetMoveDir(Vector2.zero);
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
            GameRoot.Single.mainCitySys.SetMoveDir(dir.normalized);
        });
    }
}
