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
        //��ʼ��Ĭ������
        UpdateWeapon(defaultWeapon);
        CIS = GetComponent<CinemachineImpulseSource>();
        MyEventSystem._Instance.ChangeWeapon += UpdateWeapon;
        GameManager._Instance.GameRestartEvent += Restart;
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="weapon"></param>
    public void UpdateWeapon(string weaponName)
    {
        var weapon = MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.GunPath, weaponName).GetComponent<WeaponController>();
        //���վ�����
        if (currentWeaponController != null)
        {
            MyObjectPool._Instance.RecycleObject(currentWeaponController.gameObject);
        }

        //��ʼ��������
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
