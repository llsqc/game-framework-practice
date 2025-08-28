using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData
{
    private Stack<GameObject> _dataStack = new Stack<GameObject>();
    private List<GameObject> _usedList = new List<GameObject>();
    private GameObject _rootObj;
    public int Count => _dataStack.Count;
    public int UsedCount => _usedList.Count;

    /// <summary>
    /// 初始化构造函数
    /// </summary>
    /// <param name="root">缓存池父对象</param>
    /// <param name="name">抽屉父对象的名字</param>
    /// <param name="usedObj">使用对象入队</param>
    public PoolData(GameObject root, string name, GameObject usedObj)
    {
        if (PoolMgr.IsOpenLayout)
        {
            _rootObj = new GameObject(name);
            _rootObj.transform.SetParent(root.transform);
        }

        PushUsedList(usedObj);
    }

    /// <summary>
    /// 从缓存池中弹出数据对象
    /// </summary>
    /// <returns>对象数据</returns>
    public GameObject Pop()
    {
        GameObject obj;
        if (Count > 0)
        {
            obj = _dataStack.Pop();
            _usedList.Add(obj);
        }
        else
        {
            obj = _usedList[0];
            _usedList.RemoveAt(0);
            _usedList.Add(obj);
        }

        obj.SetActive(true);
        if (PoolMgr.IsOpenLayout)
            obj.transform.SetParent(null);
        return obj;
    }

    /// <summary>
    /// 将物体放入到抽屉对象中
    /// </summary>
    /// <param name="obj"></param>
    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        if (PoolMgr.IsOpenLayout)
            obj.transform.SetParent(_rootObj.transform);
        _dataStack.Push(obj);
        _usedList.Remove(obj);
    }

    /// <summary>
    /// 将对象压入到使用中的容器中记录
    /// </summary>
    /// <param name="obj"></param>
    public void PushUsedList(GameObject obj)
    {
        _usedList.Add(obj);
    }
}

public class PoolMgr : BaseManager<PoolMgr>
{
    private Dictionary<string, PoolData> _poolDic = new Dictionary<string, PoolData>();
    private GameObject _poolObj;
    public static bool IsOpenLayout = false;

    private PoolMgr()
    {
    }

    /// <summary>
    /// 从缓存池取出
    /// </summary>
    /// <param name="name">抽屉容器的名字</param>
    /// <param name="maxNum">缓存池上限</param>
    /// <returns>从缓存池中取出的对象</returns>
    public GameObject GetObject(string name, int maxNum = 50)
    {
        if (_poolObj == null && IsOpenLayout)
            _poolObj = new GameObject("Pool");
        GameObject obj;
        if (!_poolDic.ContainsKey(name) || (_poolDic[name].Count == 0 && _poolDic[name].UsedCount < maxNum))
        {
            obj = Object.Instantiate(Resources.Load<GameObject>(name));
            obj.name = name;
            if (!_poolDic.ContainsKey(name))
                _poolDic.Add(name, new PoolData(_poolObj, name, obj));
            else
                _poolDic[name].PushUsedList(obj);
        }
        else
            obj = _poolDic[name].Pop();

        return obj;
    }

    /// <summary>
    /// 往缓存池中放入对象
    /// </summary>
    /// <param name="obj">希望放入的对象</param>
    public void PushObject(GameObject obj)
    {
        _poolDic[obj.name].Push(obj);
    }

    /// <summary>
    /// 用于清空缓存池
    /// </summary>
    public void ClearPool()
    {
        _poolDic.Clear();
        _poolObj = null;
    }
}