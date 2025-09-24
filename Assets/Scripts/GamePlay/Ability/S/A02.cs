using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 环绕
/// </summary>
public class A02 : MonoBehaviour, IAbilityController
{
    [SerializeField] A02SO A02SO;
    private Transform target;
    private float currentAngle = 0f;

    public void BeCreate<T>(T t)
    {
        target = (t as Transform).Find("AbilityHolder");
        transform.SetParent(target);
        GameManager._Instance.PauseEvent += Pause;
        gameObject.SetActive(true);
    }

    public GameObject Getobj()
    {
        return gameObject;
    }

    public void Release()
    {
        if(!isPause)
        if (target != null)
        {
            // 更新角度
            currentAngle += A02SO.orbitSpeed * Time.deltaTime;

            // 计算新位置
            float angleInRadians = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleInRadians) * A02SO.orbitRadius, Mathf.Sin(angleInRadians) * A02SO.orbitRadius, 0);

            // 设置位置
            transform.position = target.position + offset;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag("Npc"))
        {
            collision.GetComponent<NpcController>().GetHurt(A02SO.damage);
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
            id = this.A02SO.id,
            icon = this.A02SO.icon,
            description = this.A02SO.description,
        };
    }

    public AbilitySO GetAbilitySO()
    {
        return A02SO;
    }
}
