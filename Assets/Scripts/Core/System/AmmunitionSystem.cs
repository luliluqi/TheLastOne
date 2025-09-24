using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionSystem : BaseSingleton<AmmunitionSystem>
{

    public GameObject GetBullet(BulletType type)
    {
        switch (type) 
        {
            case BulletType.Bullet_762 :
                return MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.BulletPath, "B762");

            case BulletType.Pg_7vm : 
                return MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.BulletPath, "Pg7vm");

            case BulletType.Bullet_12 : 
                return MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.BulletPath, "B12");

            case BulletType.Bullet_9mm:
                return MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.BulletPath, "B9mm");

            default : return null;
        }
    }

    public GameObject GetOtherTypeBullet(string name)
    {
        return MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.BulletPath, name);
    }

    public void RecycleBullet(GameObject obj)
    {
        MyObjectPool._Instance.RecycleObject(obj);
    }
}
