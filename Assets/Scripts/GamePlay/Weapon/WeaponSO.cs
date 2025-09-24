using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "MySO/New WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    public float rate;
    public float capacity;
    public float reloadTime;
    public int singleCount;
    public int angle;
    public BulletType type;

    public virtual void ResetSO(WeaponDefault weaponDefault) {
        rate = weaponDefault.rate;
        capacity = weaponDefault.capacity;
        reloadTime = weaponDefault.reloadTime;
        singleCount = weaponDefault.singleCount;
        angle = weaponDefault.angle;
    }
}

public enum BulletType
{
    Pg_7vm, Bullet_9mm, Bullet_762, Bullet_12
}

public struct WeaponDefault
{
    public float rate;
    public float capacity;
    public float reloadTime;
    public int singleCount;
    public int angle;
}
