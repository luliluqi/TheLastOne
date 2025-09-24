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
    /// ������Ҫ�Ķ������֣�pathΪԤ�������Դ·��
    /// </summary>
    public GameObject GetObjectFromPool(string path, string objectName)
    {
        GameObject obj;
        //���������Ҫ��ȡ�Ķ���ʵ��
        if (objectPool.ContainsKey(objectName) && objectPool[objectName].Count > 0)
        {
            obj = objectPool[objectName][objectPool[objectName].Count - 1];
            objectPool[objectName].Remove(obj);
        }
        //�����������Ҫ��ȡ�Ķ���ʵ��
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
        //���ö���ʵ��״̬
        obj.SetActive(false);
        obj.transform.SetParent(parent);
        obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        //��ö���������
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
            Debug.LogWarning("��ͼ�������ڵ����͵Ķ���Ӷ����ɾ��,�ѽ���δ֪�����Ƴ���Ϸ");
            PublicMono._Instance.DestroyGameObject(obj);
            return false;
        }

        //������ڶ���ʵ�����͵ĳ�
        if (objectPool.ContainsKey(objectName))
        {
            objectPool[objectName].Add(obj);
        }
        //��������ڶ���ʵ�����͵ĳ�
        else
        {
            List<GameObject> list = new() { obj };
            objectPool.Add(objectName, list);
        }

        return true;
    }

    /// <summary>
    /// ��������δʹ�õĶ���ʵ���Ͷ���ؼ�¼
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
