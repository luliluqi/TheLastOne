using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcManagerSO", menuName = "MySO/New NpcManagerSO")]
public class NpcManagerSO : ScriptableObject
{
    public List<string> NpcList = new();
    public List<string> BossList = new();

    public List<NpcSO> npcSOs = new();

    List<string> generateNpcList = new();

    public int maxProcessNumber;
    public float generateTime;
    public int maxGenerateNumber;
    public float npcUpdateTime;
    public int bossGenerateKillCount = 150;


    public void Init()
    {
        generateNpcList.Add(NpcList[0]);
        for (int i = 0; i < npcSOs.Count; i++)
        {
            npcSOs[i].Init();
        }
    }

    public void UpdateGenerate()
    {
        if(generateNpcList.Count < NpcList.Count)
            generateNpcList.Add(NpcList[generateNpcList.Count]);

        if(maxGenerateNumber <= 250) maxGenerateNumber += 50;

        generateTime = generateTime - 0.05f >= 0.05f ? generateTime - 0.05f : 0.05f;

        for (int i = 0; i < npcSOs.Count; i++)
        {
            npcSOs[i].UpdateNpcData();
        }
    }

    public string GetNpc()
    {
        if (generateNpcList.Count <= 0) return null;

        return generateNpcList[Random.Range(0, generateNpcList.Count)];
    }

    public string GetBoss(int KillCount)
    {
        return BossList[Random.Range(0, BossList.Count)];
    }

    public void ResetSO()
    {
        generateTime = 0.5f;
        maxGenerateNumber = 20;
        bossGenerateKillCount = 150;
        generateNpcList.Clear();
        Init();
    }
}
