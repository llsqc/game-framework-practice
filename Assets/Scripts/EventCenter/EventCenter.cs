using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 用于装载的父类
/// </summary>
public abstract class EventInfoBase
{
}

/// <summary>
/// 用于包裹对应函数委托的类
/// </summary>
/// <typeparam name="T">泛型委托</typeparam>
public class EventInfo<T> : EventInfoBase
{
    public UnityAction<T> Actions;

    public EventInfo(UnityAction<T> action)
    {
        Actions += action;
    }
}

/// <summary>
/// 用于记录无参无返回值的委托
/// </summary>
public class EventInfo : EventInfoBase
{
    public UnityAction Actions;

    public EventInfo(UnityAction action)
    {
        Actions += action;
    }
}

public class EventCenter : BaseManager<EventCenter>
{
    private Dictionary<string, EventInfoBase> _eventDict = new();

    private EventCenter()
    {
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="param">传递的参数</param>
    public void EventTrigger<T>(string eventName, T param)
    {
        if (_eventDict.TryGetValue(eventName, out var value))
        {
            ((EventInfo<T>)value).Actions?.Invoke(param);
        }
    }

    public void EventTrigger(string eventName)
    {
        if (_eventDict.TryGetValue(eventName, out var value))
            ((EventInfo)value).Actions?.Invoke();
    }

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="action">事件回调</param>
    public void AddEventListener<T>(string eventName, UnityAction<T> action)
    {
        if (_eventDict.TryGetValue(eventName, out var value))
            ((EventInfo<T>)value).Actions += action;
        else
            _eventDict.Add(eventName, new EventInfo<T>(action));
    }

    public void AddEventListener(string eventName, UnityAction action)
    {
        if (_eventDict.TryGetValue(eventName, out var value))
            ((EventInfo)value).Actions += action;
        else
            _eventDict.Add(eventName, new EventInfo(action));
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="action">事件回调</param>
    public void RemoveEventListener<T>(string eventName, UnityAction<T> action)
    {
        if (_eventDict.TryGetValue(eventName, out var value))
            ((EventInfo<T>)value).Actions -= action;
    }
    public void RemoveEventListener(string eventName, UnityAction action)
    {
        if (_eventDict.TryGetValue(eventName, out var value))
            ((EventInfo)value).Actions -= action;
    }

    /// <summary>
    /// 清空所有事件监听
    /// </summary>
    public void Clear()
    {
        _eventDict.Clear();
    }

    /// <summary>
    /// 清空指定事件监听
    /// </summary>
    /// <param name="eventName">事件名</param>
    public void Clear(string eventName)
    {
        _eventDict.Remove(eventName);
    }
}