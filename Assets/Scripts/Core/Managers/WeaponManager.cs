using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public string defaultWeapon;

    public WeaponController currentWeaponController { get; private set; }
    CinemachineImpulseSource CIS;
    [SerializeField]
    Vector2 impulseForce;


    private void Start()
    {
        //初始化默认武器
        UpdateWeapon(defaultWeapon);
        CIS = GetComponent<CinemachineImpulseSource>();
        MyEventSystem._Instance.ChangeWeapon += UpdateWeapon;
        GameManager._Instance.GameRestartEvent += Restart;
    }

    /// <summary>
    /// 更换新武器
    /// </summary>
    /// <param name="weapon"></param>
    public void UpdateWeapon(string weaponName)
    {
        var weapon = MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.GunPath, weaponName).GetComponent<WeaponController>();
        //回收旧武器
        if (currentWeaponController != null)
        {
            MyObjectPool._Instance.RecycleObject(currentWeaponController.gameObject);
        }

        //初始化新武器
        currentWeaponController = weapon;
        currentWeaponController.transform.SetParent(transform);
        currentWeaponController.transform.localPosition = Vector3.zero;
        currentWeaponController.transform.localRotation = Quaternion.identity;
        currentWeaponController.gameObject.SetActive(true);
    }

    void Restart()
    {
        UpdateWeapon(defaultWeapon);
    }

    public void CameraImpulse(Vector2 impulseDir)
    {
        CIS.m_DefaultVelocity = new Vector3 (impulseDir.x * impulseForce.x, impulseDir.y * impulseForce.y, 0);
        CIS.GenerateImpulse();
    }
}
