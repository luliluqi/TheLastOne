using UnityEngine;
public class PublicMono : BaseSingleton<PublicMono>
{
    public GameObject CreateGameObject(GameObject prefab)
    {
        return Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    public bool DestroyGameObject(GameObject obj)
    {
        try { 
            Destroy(obj);
            return true;
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }
}
