using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 范围索敌
/// </summary>
public class A01 : MonoBehaviour, IAbilityController
{
    [SerializeField] A01SO A01SO;
    class Target : IComparable<Target>
    {
        public float distance;
        public Vector2 pos;

        public int CompareTo(Target other)
        {
           return distance > other.distance ? 1 : -1;
        }
    }

    List<Target> targetList = new();
    float cTime;

    public void BeCreate<T>(T t)
    {
        transform.SetParent((t as Transform).Find("AbilityHolder"));
        transform.SetPositionAndRotation(transform.parent.position, Quaternion.identity);
        GameManager._Instance.PauseEvent += Pause;
        cTime = A01SO.triggerTime;
        gameObject.SetActive(true);
    }

    public string GetId()
    {
        return A01SO.id;
    }

    public string GetDescription()
    {
        return A01SO.description;
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
            cTime = A01SO.triggerTime;

            targetList?.Clear();

            var tempList = NpcManager._Instance.NpcList.ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                if (tempList[i] == null) continue;

                Vector2 tempPos = tempList[i].transform.position;
                Vector2 vec = tempPos - (Vector2)transform.position;
                float dis = vec.x * vec.x + vec.y * vec.y;
                if (dis > A01SO.areaLength * A01SO.areaLength) continue;

                if (targetList.Count == 0)
                {
                    targetList.Add(new Target { distance = dis, pos = tempPos });
                }
                //根据最大生成数量决定targetList的大小
                else if (targetList.Count < A01SO.generateNumber)
                {
                    targetList.Add(new Target { distance = dis, pos = tempPos });

                    if (targetList.Count == A01SO.generateNumber)
                    {
                        targetList.Sort();
                    }
                }
                else
                {
                    // 比最远的还远，直接跳过
                    if (dis >= targetList[targetList.Count - 1].distance)
                        continue;

                    for (int j = 0; j < targetList.Count; j++)
                    {
                        if (dis < targetList[j].distance)
                        {
                            targetList[j].distance = dis;
                            targetList[j].pos = tempPos;
                            break;
                        }
                    }
                }
            }

            GameObject tempObj;
            for (int i = 0; i < targetList.Count; i++)
            {
                tempObj = AmmunitionSystem._Instance.GetOtherTypeBullet("Lightning");
                tempObj.GetComponent<Lightning>().TriggerPoint(targetList[i].pos);
            }
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
            id = this.A01SO.id,
            icon = this.A01SO.icon,
            description = this.A01SO.description,
        };
    }

    public AbilitySO GetAbilitySO()
    {
        return A01SO;
    }
}
