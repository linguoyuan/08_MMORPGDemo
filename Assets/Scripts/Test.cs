using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadPrefab("Cube");
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    /// <summary>
    /// 加载预制体
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if (prefab != null)
        {
            Debug.Log("Instantiate:" + prefab.name);
            go = Instantiate(prefab);
        }
        return go;
    }
}
