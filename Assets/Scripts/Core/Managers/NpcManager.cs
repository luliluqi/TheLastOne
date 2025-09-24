using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NpcManager : BaseSingleton<NpcManager>
{
    List<NpcController> npcList = new();
    public List<NpcController> NpcList { get { return npcList; } }

    NpcManagerSO npcManagerSO;

    Transform minGeneratePoint;
    Transform maxGeneratePoint;

    int killCount;
    public int KillCount { get { return killCount; } }

    int batch;
    int cBatch;

    private void Start()
    {
        ResourcesLoadManager._Instance.ResourcesLoadObject<NpcManagerSO>(ResourcesLoadManager._Instance.ConfigPath + "NpcManagerSO", out npcManagerSO);
        minGeneratePoint = LevelSystem._Instance.transform.Find("MinEnemyGeneratePoint");
        maxGeneratePoint = LevelSystem._Instance.transform.Find("MaxEnemyGeneratePoint");
        StartCoroutine(DoNpcGenerate());
        StartCoroutine(DoNpcMoveControl());
        GameManager._Instance.PauseEvent += Pause;
        GameManager._Instance.GameOverEvent += ClearAllNpc;
        GameManager._Instance.GameRestartEvent += Restart;
        MyEventSystem._Instance.NpcDead += OnKillCount;

        npcManagerSO.Init();
    }

    public void GenerateNpc(string npcName)
    {
        if (npcName == null) return;

        var npc = MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.NpcPath, npcName);
        //算法随机一个出生点给npc
        Vector2 generatePoint;

        //屏幕两侧生成
        if(Random.Range(0 ,1f) > 0.5f)
        {
            generatePoint.y = Random.Range(minGeneratePoint.position.y, maxGeneratePoint.position.y);
            generatePoint.x = Random.Range(0, 1f) > 0.5f ? minGeneratePoint.position.x : maxGeneratePoint.position.x;
        }
        //屏幕上下生成
        else
        {
            generatePoint.x = Random.Range(minGeneratePoint.position.x, maxGeneratePoint.position.x);
            generatePoint.y = Random.Range(0, 1f) > 0.5f ? minGeneratePoint.position.y : maxGeneratePoint.position.y;
        }

        npc.GetComponent<NpcController>().Spwan(generatePoint);
        npcList.Add(npc.GetComponent<NpcController>());
    }

    public void OnKillCount()
    {
        killCount++;
        if(killCount > 0 && killCount % npcManagerSO.bossGenerateKillCount == 0)
        {
            npcManagerSO.UpdateGenerate();
            GenerateNpc(npcManagerSO.GetBoss(killCount));
        }
    }

    public void RemoveNpc(NpcController npc)
    {
        if (npcList.Contains(npc))
        {
            npcList.Remove(npc);
        } 
    }

    public void ClearAllNpc()
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].Clear();
        }

        npcList.Clear();
        killCount = 0;
    }

    void NpcMoveControl()
    {
        if (batch == 0 && npcList.Count > npcManagerSO.maxProcessNumber)
        {
            batch = npcList.Count / npcManagerSO.maxProcessNumber;
            cBatch = 0;
        }

        //如果npc数量小于最大分批数量则一次性全部更新
        if (batch == 0)
        {
            for (int i = 0; i < npcList.Count; i++)
            {
                npcList[i].GetMoveDir(LevelSystem._Instance.transform.position);
            }
            return;
        }

        //否则按批次更新
        if (cBatch < batch)
        {
            for (int i = cBatch * npcManagerSO.maxProcessNumber;
                i < ((npcManagerSO.maxProcessNumber * ++cBatch) > npcList.Count ? npcList.Count : (npcManagerSO.maxProcessNumber * cBatch));
                i++)
            {
                npcList[i].GetMoveDir(LevelSystem._Instance.transform.position);
            }
        }
        else
        {
            batch = 0;
        }
    }

    IEnumerator DoNpcGenerate()
    {
        float cGenerateTime = npcManagerSO.generateTime;
        while (true)
        {
            if (!isPause)
            {
                cGenerateTime -= Time.deltaTime;
                if (cGenerateTime <= 0 && npcList.Count < npcManagerSO.maxGenerateNumber)
                {
                    if(npcList.Count < npcManagerSO.maxProcessNumber)
                    {
                        cGenerateTime = npcManagerSO.generateTime / 2f;
                    }
                    else
                    {
                        cGenerateTime = npcManagerSO.generateTime;
                    }
                    GenerateNpc(npcManagerSO.GetNpc());
                }
            }
            yield return null;
        }
    }

    IEnumerator DoNpcMoveControl()
    {
        float updateTime = npcManagerSO.npcUpdateTime;
        while (true)
        {
            if (!isPause)
            {
                updateTime -= Time.deltaTime;
                if (updateTime <= 0)
                {
                    updateTime = npcManagerSO.npcUpdateTime;
                    NpcMoveControl();
                }
            }
            yield return null;
        }
    }

    void Restart()
    {
        npcManagerSO.ResetSO();
        killCount = 0;
    }

    bool isPause;
    private void Pause(bool isPause)
    {
        this.isPause = isPause;
    }
}
