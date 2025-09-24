using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 环绕
/// </summary>
public class A03 : MonoBehaviour, IAbilityController
{
    [SerializeField] A03SO A03SO;
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
            currentAngle += A03SO.orbitSpeed * Time.deltaTime;

            // 计算新位置
            float angleInRadians = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleInRadians) * A03SO.orbitRadius, Mathf.Sin(angleInRadians) * A03SO.orbitRadius, 0);

            // 设置位置
            transform.position = target.position + offset;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag("Npc"))
        {
            DamageResoult resoult = DamageCalculatorSystem._Instance.DamageCalculator(A03SO.damage);
            if (collision.gameObject.activeInHierarchy)
                MyEventSystem._Instance.GenerateDamageText?.Invoke(resoult, new Vector2(collision.transform.position.x, collision.transform.position.y));
            collision.GetComponent<NpcController>().GetHurt(A03SO.damage);
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
            id = this.A03SO.id,
            icon = this.A03SO.icon,
            description = this.A03SO.description,
        };
    }

    public AbilitySO GetAbilitySO()
    {
        return A03SO;
    }
}
