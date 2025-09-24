using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    List<ExpController> expList = new();
    Transform player;
    PlayerDataController playerDataController;

    [SerializeField] int maxProcessNumber = 10;
    int batch;
    int cBatch;

    void Start()
    {
        player = LevelSystem._Instance.transform;
        playerDataController = player.GetComponent<PlayerDataController>();
        MyEventSystem._Instance.GenerateExp += GenerateExp;
        MyEventSystem._Instance.PickUpExp += RemoveExp;
        GameManager._Instance.GameOverEvent += ClearExpPickUp;
    }

    void Update()
    {
        ExpMoveControl();
    }

    void ExpMoveControl()
    {
        if (batch == 0 && expList.Count > maxProcessNumber)
        {
            batch = expList.Count / maxProcessNumber;
            cBatch = 0;
        }

        //如果npc数量小于最大分批数量则一次性全部更新
        if (batch == 0)
        {
            for (int i = 0; i < expList.Count; i++)
            {
                expList[i].MoveToPlayer(player, playerDataController.PickUpRange);
            }
            return;
        }

        //否则按批次更新
        if (cBatch < batch)
        {
            for (int i = cBatch * maxProcessNumber;
                i < ((maxProcessNumber * ++cBatch) > expList.Count ? expList.Count : (maxProcessNumber * cBatch));
                i++)
            {
                expList[i].MoveToPlayer(player, playerDataController.PickUpRange);
            }
        }
        else
        {
            batch = 0;
        }
    }

    public void GenerateExp(Vector2 pos)
    {
        var exp = MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.PickUpPath, "Exp");
        exp.transform.SetPositionAndRotation(pos, Quaternion.identity);
        expList.Add(exp.GetComponent<ExpController>());
        exp.SetActive(true);
    }

    public void RemoveExp(ExpController exp)
    {
        if (expList.Contains(exp))
        {
            expList.Remove(exp);
        }
    }

    void ClearExpPickUp()
    {
        for (int i = 0; i < expList.Count; i++)
        {
            MyObjectPool._Instance.RecycleObject(expList[i].gameObject);
        }
        expList.Clear();
    }
}
