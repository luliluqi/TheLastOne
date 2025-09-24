using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指向鼠标方向
/// </summary>
public class A04 : MonoBehaviour, IAbilityController
{
    [SerializeField] A04SO A04SO;
    float cTime;

    public void BeCreate<T>(T t)
    {
        transform.SetParent((t as Transform).Find("AbilityHolder"));
        transform.SetPositionAndRotation(transform.parent.position, Quaternion.identity);
        cTime = A04SO.triggerTime;
        GameManager._Instance.PauseEvent += Pause;
        gameObject.SetActive(true);
    }

    public GameObject Getobj()
    {
        return gameObject;
    }

    public void Release()
    {
        if (isPause) return;
        cTime -= Time.deltaTime;
        if (cTime <= 0)
        {
            cTime = A04SO.triggerTime;
            var obj = AmmunitionSystem._Instance.GetOtherTypeBullet("MagicArrow");
            obj.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 181))));
            obj.gameObject.SetActive(true);
        }
    }

    bool isPause;
    public void Pause(bool isPause)
    {
        this.isPause = isPause;
    }

    public void BeDestroy()
    {
        GameManager._Instance.PauseEvent -= Pause;
    }

    public AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo
        {
            id = this.A04SO.id,
            icon = this.A04SO.icon,
            description = this.A04SO.description,
        };
    }

    public AbilitySO GetAbilitySO()
    {
       return A04SO;
    }
}
