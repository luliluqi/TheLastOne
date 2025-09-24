using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PropertiesControllerSO", menuName = "MySO/New PropertiesControllerSO")]
public class PropertiesControllerSO : ScriptableObject
{
    [Header("��ɫ��������")]
    public List<PropertySO> PType = new();

    [Header("������������")]
    public List<PropertySO> GPType = new();

    [Header("����������������")]
    public List<PropertySO> SPType = new();

    [Header("����")]
    public List<AbilitySO> AbilitySOs = new();

    [Header("����")]
    public List<WeaponSO> WeaponSOs = new();
    List<WeaponDefault> WeaponDefaults = new();

    [Header("ǹе�ӵ�")]
    public List<BulletSO> BulletsSOs = new();

    [Header("�����ӵ�")]
    public List<BulletSO> SBulletsSOs = new();

    public void InitSO()
    {
        for (int i = 0; i < PType.Count; i++)
        {
            PType[i].Init();
        }

        for (int i = 0; i < GPType.Count; i++)
        {
            GPType[i].Init();
        }

        for (int i = 0; i < SPType.Count; i++)
        {
            SPType[i].Init();
        }

        for (int i = 0; i < WeaponSOs.Count; i++)
        {
            WeaponDefaults.Add(new WeaponDefault
            {
                rate = WeaponSOs[i].rate,
                capacity = WeaponSOs[i].capacity,
                reloadTime = WeaponSOs[i].reloadTime,
                singleCount = WeaponSOs[i].singleCount,
                angle = WeaponSOs[i].angle,
            });
        }
    }

    public void ResetSO()
    {
        for (int i = 0; i < WeaponSOs.Count; i++)
        {
            WeaponSOs[i].ResetSO(WeaponDefaults[i]);
        }

        for (int i = 0; i < AbilitySOs.Count; i++)
        {
            AbilitySOs[i].ResetSO();
        }

        for (int i = 0; i < BulletsSOs.Count; i++)
        {
            BulletsSOs[i].onHitRecycle = true;
        }
    }

    public List<AbilityInfo> GetReward()
    {
        List<AbilityInfo> tempSO = new();
        List<AbilityInfo> randomList = new();

        for (int i = 0; i < GPType.Count; i++)
        {
            randomList.Add(new AbilityInfo
            {
                id = GPType[i].id,
                icon = null,
                description = GPType[i].description
            });
        }

        for (int i = 0; i < PType.Count; i++)
        {
            randomList.Add(new AbilityInfo
            {
                id = PType[i].id,
                icon = null,
                description = PType[i].description
            });
        }

        for (int i = 0; i < BuildController._Instance.Abilitys.Count; i++)
        {
            for (int j = 0; j < BuildController._Instance.Abilitys[i].GetAbilitySO().abilityPropertyUp.Count; j++)
            {
                randomList.Add(new AbilityInfo
                {
                    id = BuildController._Instance.Abilitys[i].GetAbilitySO().abilityPropertyUp[j].id,
                    icon = null,
                    description = BuildController._Instance.Abilitys[i].GetAbilitySO().abilityPropertyUp[j].description
                });
            }
        }

        bool isRepeat;
        for (int i = 0; i < AbilitySOs.Count; i++)
        {
            isRepeat = false;
            for (int j = 0; j < BuildController._Instance.Abilitys.Count; j++)
            {
                if (AbilitySOs[i].id == BuildController._Instance.Abilitys[j].GetAbilityInfo().id)
                {
                    isRepeat = true;
                    break;
                }
            }
            //�Ѿ���ȡ�����������ٲ����ȡ
            if (!isRepeat)
            {
                randomList.Add(new AbilityInfo
                {
                    id = AbilitySOs[i].id,
                    icon = AbilitySOs[i].icon,
                    description = AbilitySOs[i].description
                });
            }
        }

        int index;
        for (int i = 0; i < 3; i++)
        {
            index = Random.Range(0, randomList.Count);
            tempSO.Add(randomList[index]);
            randomList.RemoveAt(index);
        }

        return tempSO;
    }

    public WeaponSO GetWeaponSO()
    {
        WeaponSO tempSO;
        tempSO = WeaponSOs[Random.Range(0, WeaponSOs.Count)];
        return tempSO;
    }
}
