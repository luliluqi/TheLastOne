using System.Collections.Generic;
using UnityEngine;

public class MyObjectPool : BaseSingletonNoMono<MyObjectPool>
{
    Dictionary<string, List<GameObject>> objectPool;
    Dictionary<string, List<GameObject>> objectInUsePool;

    public MyObjectPool()
    {
        objectPool = new Dictionary<string, List<GameObject>>();
        objectInUsePool = new Dictionary<string, List<GameObject>>();
    }

    /// <summary>
    /// 传入想要的对象名字，path为预制体的资源路径
    /// </summary>
    public GameObject GetObjectFromPool(string path, string objectName)
    {
        GameObject obj;
        //如果存在想要获取的对象实例
        if (objectPool.ContainsKey(objectName) && objectPool[objectName].Count > 0)
        {
            obj = objectPool[objectName][objectPool[objectName].Count - 1];
            objectPool[objectName].Remove(obj);
        }
        //如果不存在想要获取的对象实例
        else
        {
            ResourcesLoadManager._Instance.ResourcesLoadObject<GameObject>(path + objectName, out obj);
            if (obj != null)
            {
                obj = PublicMono._Instance.CreateGameObject(obj);
            }
            else
            {
                return null;
            }
        }

        if (objectInUsePool.ContainsKey(objectName))
        {
            objectInUsePool[objectName].Add(obj);
        }
        else
        {
            List<GameObject> list = new()
            {
                obj
            };
            objectInUsePool.Add(objectName, list);
        }

        obj.SetActive(false);
        return obj;
    }

    public bool RecycleObject(GameObject obj, Transform parent = null)
    {
        //重置对象实例状态
        obj.SetActive(false);
        obj.transform.SetParent(parent);
        obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        //获得对象所属键
        string objectName = null;

        foreach (var pair in objectInUsePool)
        {
            if (pair.Value.Contains(obj))
            {
                objectName = pair.Key;
                objectInUsePool[objectName].Remove(obj);
                break;
            }
        }

        if (objectName == null)
        {
            Debug.LogWarning("试图将不存在的类型的对象从对象池删除,已将此未知对象移除游戏");
            PublicMono._Instance.DestroyGameObject(obj);
            return false;
        }

        //如果存在对象实例类型的池
        if (objectPool.ContainsKey(objectName))
        {
            objectPool[objectName].Add(obj);
        }
        //如果不存在对象实例类型的池
        else
        {
            List<GameObject> list = new() { obj };
            objectPool.Add(objectName, list);
        }

        return true;
    }

    /// <summary>
    /// 销毁所有未使用的对象实例和对象池记录
    /// </summary>
    public void ClearPool()
    {
        foreach (var pair in objectPool)
        {
            for (int i = objectPool[pair.Key].Count - 1; i >= 0; i--)
            {
                PublicMono._Instance.DestroyGameObject(objectPool[pair.Key][i]);
            }
        }

        objectPool.Clear();
    }
}
