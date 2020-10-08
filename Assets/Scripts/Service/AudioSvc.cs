using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSvc : MonoSingleton<AudioSvc>
{
    public AudioSource BgAudio;
    public AudioSource UiAudio;
    public void Init()
    {
        if (BgAudio == null)
        {
            Debug.Log("1:BgAudio == null");
        }
        Debug.Log("Init AudioSvc");
    }

    private AudioClip clip;
    public void PlayBgMusic(string name, bool isLoop = true)
    {
        clip = ResSvc.Single.LoadAudio("ResAudio/" + name, isLoop, true);
        if (BgAudio == null)
        {
            Debug.Log("BgAudio == null");
        }
        if (BgAudio.clip == null || BgAudio.clip.name != clip.name)//已经在播放的音频为空或者音频不相同才会执行
        {
            Debug.Log("bg :" + name);
            BgAudio.clip = clip;
            BgAudio.loop = isLoop;
            BgAudio.Play();
        }
    }

    private AudioClip clipUi;
    public void PlayUIMusic(string name)
    {
        clipUi = ResSvc.Single.LoadAudio("ResAudio/" + name, false, true);

        if (UiAudio == null)
        {
            Debug.Log("UiAudio == null");
        }
        else
        {
            Debug.Log("ui :" + name);
            UiAudio.clip = clipUi;
            UiAudio.Play();
        }
    }
}
