using UnityEngine;
public class ResourcesLoadManager : BaseSingleton<ResourcesLoadManager>
{
    public string BulletPath = "Prefabs/Bullets/";
    public string GunPath = "Prefabs/Guns/";
    public string SoundPath = "Prefabs/Sounds/";
    public string HitEffectPath = "Prefabs/HitEffect/";
    public string PickUpPath = "Prefabs/PickUp/";
    public string AbilityPath = "Prefabs/Abilitys/";
    public string NpcPath = "Prefabs/Npcs/";
    public string ConfigPath = "Prefabs/Config/";

    /// <summary>
    /// 泛型资源加载方法
    /// </summary>
    public void ResourcesLoadObject<T>(string path, out T fileType) where T : Object
    {
        try
        {
            fileType = Resources.Load<T>(path);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            fileType = null;
        }
    }
}
