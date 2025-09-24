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
        //玩家属性相关的升级
        if (id[0] == 'P')
        {
            switch (id)
            {
                //当前额外生命值增加15点
                case "P01":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.ExtraHpUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //当前最大生命值提升5%
                case "P02":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.HpUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //当前最大生命值提升10%
                case "P03":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.HpUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //最大生命值提升20%
                case "P04":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.HpUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //移速提升10%
                case "P05":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.MoveSpeedUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //暴击率增加5%
                case "P06":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.CritRateUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //暴击率增加10%
                case "P07":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.CritRateUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //暴击伤害增加10%
                case "P08":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.CritDamageUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //幸运增加5点
                case "P09":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.LuckyUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //自动回血增加5点
                case "P10":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.AutoTreatmentValueUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //回血速率提升10%
                case "P11":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.TreatmentRateUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //击杀回血增加5点
                case "P12":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.KillTreatmentUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //攻击力增加2点
                case "P13":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.AttackPowerUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //抵抗力增加1点
                case "P14":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.ResistanceUp((int)value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //闪避率增加5%
                case "P15":
                    PropertyUpdateEvent += () =>
                    {
                        dataController.DodgeRateUp(value);
                        dataController.DataUpdateEvent?.Invoke();
                    };
                    break;
                //拾取范围提升10%
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

        //能力属性相关的升级
        if (id[0] == 'S')
        {
            switch (id)
            {
                //A01的生成数量增加1
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
                //A01的触发时间减少10%
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
                //A01的检测范围提升10%
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
                //A03的旋转速度提升10%
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
                //A04的触发间隔减少10%
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

        //枪械相关的升级
        if (id[0] == 'G')
        {
            ResourcesLoadManager._Instance.ResourcesLoadObject<PropertiesControllerSO>
                            (ResourcesLoadManager._Instance.ConfigPath + "PropertiesControllerSO", out PropertiesControllerSO SO);
            switch (id)
            {
                //武器射速提升10%
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
                //弹夹容量增加5发
                case "GP02":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.WeaponSOs.Count; i++)
                        {
                            SO.WeaponSOs[i].capacity += value;
                        }
                    };
                    break;
                //换弹时间减少10%
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
                //无限子弹
                case "GP04":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.WeaponSOs.Count; i++)
                        {
                            SO.WeaponSOs[i].reloadTime = 0f;
                        }
                    };
                    break;
                //子弹数量增加1(只对霰弹类武器生效)
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
                //子弹穿透
                case "GP06":
                    PropertyUpdateEvent += () =>
                    {
                        for (int i = 0; i < SO.BulletsSOs.Count; i++)
                        {
                            SO.BulletsSOs[i].onHitRecycle = false;
                        }
                    };
                    break;
                //散射角度增加1(只对霰弹类武器生效)
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

