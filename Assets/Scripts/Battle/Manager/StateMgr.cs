﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMgr : MonoBehaviour
{
    private Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();
    public void Init()
    {
        fsm.Add(AniState.Idle, new StateIdle());
        fsm.Add(AniState.Move, new StateMove());
        fsm.Add(AniState.Attack, new StateAttack());

        PECommon.Log("Init StateMgr Done.");
    }

    public void ChangeStatus(EntityBase entity, AniState targetState, params object[] args)
    {
        if (entity.currentAniState == targetState)
        {
            return;
        }

        if (fsm.ContainsKey(targetState))
        {
            if (entity.currentAniState != AniState.None)
            {
                fsm[entity.currentAniState].Exit(entity, args);
            }
            fsm[targetState].Enter(entity, args);
            fsm[targetState].Process(entity, args);
        }
    }
}