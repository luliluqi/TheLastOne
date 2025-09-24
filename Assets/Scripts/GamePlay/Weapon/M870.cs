using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M870 : WeaponController
{
    protected Animator animator;
    List<GameObject> bullets;

    protected override void Start()
    {
        base.Start();
        ResourcesLoadManager._Instance.ResourcesLoadObject<AudioClip>(ResourcesLoadManager._Instance.SoundPath + "M870", out soundClip);
        animator = transform.Find("Sprite").GetComponent<Animator>();

        bullets = new List<GameObject>();
    }

    public override bool Fire(Quaternion fireRot)
    {
        if (isReload) return false;

        canFire = false;

        for (int i = 0; i < weaponSO.singleCount; i++)
        {
            bullets.Add(AmmunitionSystem._Instance.GetBullet(weaponSO.type));
        }

        if (bullets.Count < weaponSO.singleCount)
        {
            return false;
        }
        audioSource.clip = soundClip;
        audioSource.Play();
        animator.Play("Base Layer.Fire", 0);
        for (int i = 0; i < weaponSO.singleCount; i++)
        {
            bullets[i].transform.SetPositionAndRotation(firePoint.position,
                fireRot * Quaternion.Euler(new Vector3(0, 0, weaponSO.angle * (i % 2 == 0 ? 1 : -1) * i)));
            bullets[i].SetActive(true);
        }
        cCapacity--;
        bullets.Clear();

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
