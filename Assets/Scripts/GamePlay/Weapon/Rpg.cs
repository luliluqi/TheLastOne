using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rpg : WeaponController
{
    protected override void Start()
    {
        base.Start();
        ResourcesLoadManager._Instance.ResourcesLoadObject<AudioClip>(ResourcesLoadManager._Instance.SoundPath + "Rpg", out soundClip);
    }

    public override bool Fire(Quaternion fireRot)
    {
        if(isReload) return false;

        canFire = false;

        var bullet = AmmunitionSystem._Instance.GetBullet(weaponSO.type);

        if (bullet == null)
        {
            return false;
        }
        audioSource.clip = soundClip;
        audioSource.Play();
        bullet.transform.SetPositionAndRotation(firePoint.position, fireRot);
        bullet.SetActive(true);
        cCapacity--;

        if (cCapacity <= 0)
        {
            isReload = true;
        }

        return true;
    }

    public override void Reload()
    {
        if (!isReload) return;

        cReloadTime -= 0.02f;
        if (cReloadTime <= 0)
        {
            isReload = false;
            cReloadTime = weaponSO.reloadTime;
            cCapacity = weaponSO.capacity;
        }
    }

    protected override void FireCoolDown()
    {
        if (canFire) return;

        cRate -= 0.02f;
        if (cRate <= 0f)
        {
            cRate = weaponSO.rate;
            canFire = true;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Reload();
    }
}
