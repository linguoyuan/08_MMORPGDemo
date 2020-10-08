using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRoot : MonoBehaviour
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected XmlConfigSvc xmlConfigSvc;
    protected NetSvc netSvc;
    public virtual void InitSys()
    {
        resSvc = ResSvc.Single;
        audioSvc = AudioSvc.Single;
        xmlConfigSvc = XmlConfigSvc.Single;
        netSvc = NetSvc.Single;
    }
}
