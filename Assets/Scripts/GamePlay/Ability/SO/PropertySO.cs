using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PropertySO", menuName = "MySO/New PropertySO")]
public class PropertySO : ScriptableObject
{
    public string id;
    public string description;
    public float value;

    public UnityAction PropertyUpdateEvent;

    public void Init()
    {
        PlayerDataController dataController = LevelSystem._Instance.GetComponent<PlayerDataController>();
        //���������ص�����
        if (id[0] == 'P')
        {
            switch (id)
            {
                //��ǰ��������ֵ����15��
                case "P01":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.ExtraHpUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //��ǰ�������ֵ����5%
                case "P02":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.HpUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //��ǰ�������ֵ����10%
                case "P03":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.HpUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //�������ֵ����20%
                case "P04":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.HpUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //��������10%
                case "P05":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.MoveSpeedUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //����������5%
                case "P06":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.CritRateUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //����������10%
                case "P07":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.CritRateUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //�����˺�����10%
                case "P08":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.CritDamageUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //��������5��
                case "P09":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.LuckyUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //�Զ���Ѫ����5��
                case "P10":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.AutoTreatmentValueUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //��Ѫ��������10%
                case "P11":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.TreatmentRateUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //��ɱ��Ѫ����5��
                case "P12":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.KillTreatmentUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //����������2��
                case "P13":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.AttackPowerUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //�ֿ�������1��
                case "P14":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.ResistanceUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //����������5%
                case "P15":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.DodgeRateUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //ʰȡ��Χ����10%
                case "P16":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.PickUpRangeUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                default: break;
            }
        }

        //����������ص�����
        if (id[0] == 'S')
        {
            switch (id)
            {
                //A01��������������1
                case "SP01":
                    PropertyUpdateEvent += () =>
                    {
                        ResourcesLoadManager._Instance.ResourcesLoadObject<AbilitySO>
                            (ResourcesLoadManager._Instance.ConfigPath + "Ability/A01SO", out AbilitySO SO);
                        A01SO tempSO = SO as A01SO;
                        if (tempSO.generateNumber < 20) tempSO.generateNumber += (int)value;
                        else tempSO.generateNumber = 20;
                    };
                    break;
                //A01�Ĵ���ʱ�����10%
                case "SP02":
                    PropertyUpdateEvent += () =>
                    {
                        ResourcesLoadManager._Instance.ResourcesLoadObject<AbilitySO>
                            (ResourcesLoadManager._Instance.ConfigPath + "Ability/A01SO", out AbilitySO SO);
                        A01SO tempSO = SO as A01SO;
                        if (tempSO.triggerTime > 0.2f) tempSO.triggerTime += tempSO.triggerTime * value;
                        else tempSO.triggerTime = 0.2f;
                    };
                    break;
                //A01�ļ�ⷶΧ����10%
                case "SP03":
                    PropertyUpdateEvent += () =>
                    {
                        ResourcesLoadManager._Instance.ResourcesLoadObject<AbilitySO>
                            (ResourcesLoadManager._Instance.ConfigPath + "Ability/A01SO", out AbilitySO SO);
                        A01SO tempSO = SO as A01SO;
                        if (tempSO.areaLength < 15) tempSO.areaLength += tempSO.areaLength * value;
                        else tempSO.areaLength = 15;
                    };
                    break;
                //A02
                case "SP04":
                    PropertyUpdateEvent += () =>
                    {
                        ResourcesLoadManager._Instance.ResourcesLoadObject<AbilitySO>
                            (ResourcesLoadManager._Instance.ConfigPath + "Ability/A02SO", out AbilitySO SO);
                        A02SO tempSO = SO as A02SO;
                    };
                    break;
                //A03����ת�ٶ�����10%
                case "SP05":
                    PropertyUpdateEvent += () =>
                    {
                        ResourcesLoadManager._Instance.ResourcesLoadObject<AbilitySO>
                            (ResourcesLoadManager._Instance.ConfigPath + "Ability/A03SO", out AbilitySO SO);
                        A03SO tempSO = SO as A03SO;
                        tempSO.orbitSpeed *= (1 + value);
                        if(tempSO.orbitSpeed > 500) tempSO.orbitSpeed = 500;
                    };
                    break;
                //A04�Ĵ����������10%
                case "SP06":
                    PropertyUpdateEvent += () =>
                    {
                        ResourcesLoadManager._Instance.ResourcesLoadObject<AbilitySO>
                            (ResourcesLoadManager._Instance.ConfigPath + "Ability/A04SO", out AbilitySO SO);
                        A04SO tempSO = SO as A04SO;
                        if (tempSO.triggerTime > 0.2) tempSO.triggerTime += tempSO.triggerTime * value;
                        else tempSO.triggerTime = 0.2f;
                    };
                    break;
                default: break;
            }
            return;
        }

        //ǹе��ص�����
        if (id[0] == 'G')
        {
            ResourcesLoadManager._Instance.ResourcesLoadObject<PropertiesControllerSO>
                            (ResourcesLoadManager._Instance.ConfigPath + "PropertiesControllerSO", out PropertiesControllerSO SO);
            switch (id)
            {
                //������������10%
                case "GP01":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.WeaponSOs.Count; i++)
                        {
                            if (SO.WeaponSOs[i].rate > 0.1f) SO.WeaponSOs[i].rate += SO.WeaponSOs[i].rate * value;
                            else SO.WeaponSOs[i].rate = 0.1f;
                        }
                    };
                    break;
                //������������5��
                case "GP02":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.WeaponSOs.Count; i++)
                        {
                            SO.WeaponSOs[i].capacity += value;
                        }
                    };
                    break;
                //����ʱ�����10%
                case "GP03":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.WeaponSOs.Count; i++)
                        {
                            if (SO.WeaponSOs[i].reloadTime > 0.5f) SO.WeaponSOs[i].reloadTime += SO.WeaponSOs[i].reloadTime * value;
                            else SO.WeaponSOs[i].reloadTime = 0.5f;
                        }
                    };
                    break;
                //�����ӵ�
                case "GP04":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.WeaponSOs.Count; i++)
                        {
                            SO.WeaponSOs[i].reloadTime = 0f;
                        }
                    };
                    break;
                //�ӵ���������1(ֻ��������������Ч)
                case "GP05":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.WeaponSOs.Count; i++)
                        {
                            if (SO.WeaponSOs[i].type == BulletType.Bullet_12)
                            {
                                if (SO.WeaponSOs[i].singleCount < 24) SO.WeaponSOs[i].singleCount++;
                                else SO.WeaponSOs[i].singleCount = 24;
                            }
                        }
                    };
                    break;
                //�ӵ���͸
                case "GP06":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.BulletsSOs.Count; i++)
                        {
                            SO.BulletsSOs[i].onHitRecycle = false;
                        }
                    };
                    break;
                //ɢ��Ƕ�����1(ֻ��������������Ч)
                case "GP07":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.WeaponSOs.Count; i++)
                        {
                            if (SO.WeaponSOs[i].type == BulletType.Bullet_12)
                            {
                                if (SO.WeaponSOs[i].angle < 15) SO.WeaponSOs[i].angle++;
                                else SO.WeaponSOs[i].singleCount = 15;
                            }
                        }
                    };
                    break;
                default: break;
            }
            return;
        }
    }
}

