using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TreasureInfo
{
    public string name;
    public Sprite icon;
}

public class TreasureManager : MonoBehaviour
{
    PlayerDataController playerDataController;

    private void Start()
    {
        MyEventSystem._Instance.GenerateTreasure += GenerateTreasure;
        MyEventSystem._Instance.PickUpTreasure += RandomTreasure;
        playerDataController = LevelSystem._Instance.GetComponent<PlayerDataController>();
    }

    public void GenerateTreasure(Vector2 pos)
    {
        float rand = Random.Range(0, 1f);
        if (rand <= (0.3f + playerDataController.Lucky / 100))
        {
            var treasure = MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.PickUpPath, "Treasure");
            treasure.transform.position = pos;
            treasure.SetActive(true);
        }
    }

    void RandomTreasure()
    {
        ResourcesLoadManager._Instance.ResourcesLoadObject<PropertiesControllerSO>(ResourcesLoadManager._Instance.ConfigPath + "PropertiesControllerSO", out var SO);
        int index = Random.Range(0, SO.WeaponSOs.Count);
        MyEventSystem._Instance.LoadTreasureToUI?.Invoke(new TreasureInfo
        {
            name = SO.WeaponSOs[index].name,
            icon = SO.WeaponSOs[index].weaponIcon
        });
    }
}
