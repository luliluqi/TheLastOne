using UnityEngine;

/// <summary>
/// �̳�Mono���̰߳�ȫ�ĵ���ģʽ
/// </summary>
public class BaseSingleton<T> : MonoBehaviour where T : class
{
    public static T _Instance = null;

    private static readonly object objLock = new object();

    protected virtual void Awake()
    {
        lock (objLock)
        {
            if(_Instance != null)
            {
                return;
            }
            _Instance = this as T;
        }
    }
}


/// <summary>
/// ���̳�Mono���̰߳�ȫ�ĵ���ģʽ
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseSingletonNoMono<T> where T : class, new()
{
    private static T Instance = null;

    private static readonly object objLock = new object();


    public static T _Instance
    {
        get
        {
            lock (objLock)
            {
                if (Instance == null)
                {
                    Instance = new T();
                }
                return Instance;
            }
        }
    }
}

