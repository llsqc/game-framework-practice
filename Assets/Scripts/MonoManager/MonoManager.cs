using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : SingletonAutoMono<MonoManager>
{
    private event UnityAction UpdateEvent;
    private event UnityAction FixedUpdateEvent;
    private event UnityAction LateUpdateEvent;

    /// <summary>
    /// 添加Update帧更新监听函数
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        UpdateEvent += action;
    }

    /// <summary>
    /// 移除Update帧更新监听函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        UpdateEvent -= action;
    }

    /// <summary>
    /// 添加FixUpdate帧更新监听函数
    /// </summary>
    /// <param name="action"></param>
    public void AddFixUpdateListener(UnityAction action)
    {
        FixedUpdateEvent += action;
    }

    /// <summary>
    /// 移除FixUpdate帧更新监听函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveFixUpdateListener(UnityAction action)
    {
        FixedUpdateEvent -= action;
    }

    /// <summary>
    /// 添加LateUpdate帧更新监听函数
    /// </summary>
    /// <param name="action"></param>
    public void AddLateUpdateListener(UnityAction action)
    {
        LateUpdateEvent += action;
    }

    /// <summary>
    /// 移除LateUpdate帧更新监听函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveLateUpdateListener(UnityAction action)
    {
        LateUpdateEvent -= action;
    }

    private void Update()
    {
        UpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        FixedUpdateEvent?.Invoke();
    }

    private void LateUpdate()
    {
        LateUpdateEvent?.Invoke();
    }
}