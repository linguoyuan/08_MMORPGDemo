using UnityEngine;
using UnityEngine.UI;

public static class Tils
{
    public static T GetOrAddComponect<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    public static void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    public static void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    public static void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    public static void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }
    public static void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }

    public static void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }

    public static void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }
    public static void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    public static void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }

    public static Transform GetTrans(Transform trans, string name)
    {
        if  (trans != null)
        {
            return trans.Find(name);
        }
        else
        {
            return trans.Find(name);
        }
    }
}
