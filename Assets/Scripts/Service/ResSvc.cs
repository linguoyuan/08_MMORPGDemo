using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoSingleton<ResSvc>
{

	public void InitSvc()
    {
        Debug.Log("Init ResSvc");
    }

    int displayProgress = 0;
    int toProgress = 0;

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    /// <param name="done"></param>
    /// <returns></returns>
    public void AsyncLoadScene(string name, Action<int> callback, Action done)
    {
        StartCoroutine(ReallyAsyncLoadScene(name, callback, done));
    }
    private IEnumerator ReallyAsyncLoadScene(string name, Action<int> callback, Action done)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        ao.allowSceneActivation = false;
        while (ao.progress < 0.9f)
        {
            //toProgress = (int)ao.progress * 100;           
            toProgress = 90;
            while (displayProgress <= toProgress)
            {               
                ++displayProgress;
                callback(displayProgress);
                //这里故意延时0.01秒是为了放慢加载速度，更好地观察代码效果，实际应用不能这么写
                yield return new WaitForSeconds(0.01f);
                //使用下面的任一句都可以
                //yield return new WaitForEndOfFrame();
                //yield return ao.progress;
            }
        }

        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            callback(displayProgress);

            yield return new WaitForSeconds(0.01f);
            //yield return new WaitForEndOfFrame();
            //yield return ao;
        }

        //加载完成后执行done
        ao.allowSceneActivation = true;
        Debug.Log("加载完成");
        done();
    }

    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    /// <summary>
    /// 加载音频
    /// </summary>
    /// <param name="path"></param>
    /// <param name="loop"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public AudioClip LoadAudio(string path, bool loop, bool cache = false)
    {
        Debug.Log(path);
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au))
        {
            au = Resources.Load<AudioClip>(path);
            //au = await Addressables.LoadAssetAsync<AudioClip>(path).Task;
            if (cache)
            {
                adDic.Add(path, au);
            }
        }
        if (au == null)
        {
            Debug.Log("au == null");
        }
        return au;
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

    public GameObject LoadPrefabByPos(string path, Vector3 position, bool cache = false)
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
            //go = Instantiate(prefab);
            go = Instantiate(prefab, position, Quaternion.identity);
        }
        return go;
    }


    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    /// <summary>
    /// 加载精灵图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public Sprite LoadSprite(string path, bool cache = false)
    {
        Sprite sp = null;
        if (!spDic.TryGetValue(path, out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }
}
