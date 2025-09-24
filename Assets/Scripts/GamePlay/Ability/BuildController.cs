using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AbilityInfo
{
    public string id;
    public Sprite icon;
    public string description;
}

public class BuildController : BaseSingleton<BuildController>
{
    protected List<IAbilityController> abilitys;
    public List<IAbilityController> Abilitys { get { return abilitys; } }
    List<AbilityInfo> abilitysRandom = new();

    PropertiesControllerSO propertiesControllerSO;

    private void Start()
    {
        abilitys = new();
        ResourcesLoadManager._Instance.ResourcesLoadObject<PropertiesControllerSO>(ResourcesLoadManager._Instance.ConfigPath + "PropertiesControllerSO", out propertiesControllerSO);
        propertiesControllerSO.InitSO();
        GameManager._Instance.GameOverEvent += ClearAbilityList;
        GameManager._Instance.GameRestartEvent += propertiesControllerSO.ResetSO;
    }

    private void Update()
    {
        ExecuteAll();
    }

    public List<AbilityInfo> CreateSelect()
    {
        abilitysRandom?.Clear();
        var abilitySOs = propertiesControllerSO.GetReward();
        for (int i = 0; i < abilitySOs.Count; i++)
        {
            abilitysRandom.Add(abilitySOs[i]);
        }
        return abilitysRandom;
    }

    public void GetSelectIndex(int index)
    {
        PropertySO SO;
        switch (abilitysRandom[index].id[0])
        {
            case 'A':
                GameObject newAbility = MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.AbilityPath, abilitysRandom[index].id);
                GetNewAbility(abilitysRandom[index].id, newAbility.GetComponent<IAbilityController>());
                break;
            case 'G':
                ResourcesLoadManager._Instance.ResourcesLoadObject<PropertySO>(ResourcesLoadManager._Instance.ConfigPath + "Properties/G/" + abilitysRandom[index].id, out SO);
                SO.PropertyUpdateEvent?.Invoke();
                break;
            case 'P':
                ResourcesLoadManager._Instance.ResourcesLoadObject<PropertySO>(ResourcesLoadManager._Instance.ConfigPath + "Properties/P/" + abilitysRandom[index].id, out SO);
                SO.PropertyUpdateEvent?.Invoke();
                break;
            case 'S':
                ResourcesLoadManager._Instance.ResourcesLoadObject<PropertySO>(ResourcesLoadManager._Instance.ConfigPath + "Properties/SP/" + abilitysRandom[index].id, out SO);
                SO.PropertyUpdateEvent?.Invoke();
                break;
            default: break;
        }
    }

    public string GetBuildDescription(int index)
    {
        if (index > abilitys.Count - 1 || index < 0) return "No Ability info";

        return abilitys[index].GetAbilityInfo().description;
    }

    void GetNewAbility(string id, IAbilityController ability)
    {
        for (int i = 0; i < abilitys.Count; i++)
        {
            if (abilitys[i].GetAbilityInfo().id == id)
            {
                return;
            }
        }

        ability.BeCreate(transform);
        abilitys.Add(ability);

        return;
    }

    /// <summary>
    /// 执行所有的能力
    /// </summary>
    public void ExecuteAll()
    {
        if (abilitys.Count <= 0) return;
        for (int i = 0; i < abilitys.Count; i++)
        {
            abilitys[i].Release();
        }
    }

    /// <summary>
    /// 移除某个能力
    /// </summary>
    /// <param name="id"></param>
    public void RemoveAbility(string id)
    {
        for (int i = 0; i < abilitys.Count; i++)
        {
            if (abilitys[i].GetAbilityInfo().id == id)
            {
                if (abilitys[i].Getobj() != null)
                {
                    MyObjectPool._Instance.RecycleObject(abilitys[i].Getobj());
                }
                abilitys.RemoveAt(i);
            }
        }
    }

    void ClearAbilityList()
    {
        for (int i = 0; i < abilitys.Count; i++)
        {
            MyObjectPool._Instance.RecycleObject(abilitys[i].Getobj());
        }
        abilitys.Clear();
    }
}