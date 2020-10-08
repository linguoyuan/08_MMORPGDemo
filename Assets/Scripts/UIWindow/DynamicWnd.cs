using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot {
    public Animation tipsAni;
    public Text txtTips;

    private bool isTipsShow = false;
    private Queue<string> tipsQue = new Queue<string>();

    protected override void InitWnd() {
        base.InitWnd();

        Tils.SetActive(txtTips, false);
    }

    public void AddTips(string tips) {
        lock (tipsQue) {
            tipsQue.Enqueue(tips);
        }
    }

    private void Update() {
        if (tipsQue.Count > 0 && isTipsShow == false) {
            lock (tipsQue) {
                string tips = tipsQue.Dequeue();
                isTipsShow = true;
                SetTips(tips);
            }
        }
    }

    AnimationClip clip;
    private void SetTips(string tips) {
        Tils.SetActive(txtTips);
        Tils.SetText(txtTips, tips);

        clip = tipsAni.GetClip("TipsShow");
        tipsAni.Play();
        //延时关闭激活状态

        if (clip == null)
        {
            Debug.Log("clip == null");
        }

        if (tipsAni == null)
        {
            Debug.Log("tipsAni == null");
        }
        StartCoroutine(AniPlayDone(clip.length, () => {
            Tils.SetActive(txtTips, false);
            isTipsShow = false;
        }));
    }

    private IEnumerator AniPlayDone(float sec, Action cb) {
        yield return new WaitForSeconds(sec);
        if (cb != null) {
            cb();
        }
    }
}