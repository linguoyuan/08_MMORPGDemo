using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharRotateShowEvtsHandler : MonoBehaviour
{
    public RawImage imgTouch;

    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;

    void Start()
    {
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
            MainCitySys.Instance.SetStartRoate();
        });

        OnClickUp(imgTouch.gameObject, (PointerEventData evt) =>
        {

        });

        OnDrag(imgTouch.gameObject, (PointerEventData evt) =>
        {
            float roate = -(evt.position.x - startPos.x) * 0.4f;
            MainCitySys.Instance.SetPlayerRoate(roate);
        });
    }
}
