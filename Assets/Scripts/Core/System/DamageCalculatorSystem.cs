using UnityEngine;

public struct DamageResoult
{
    public float damage;
    public bool isCritical;
}

public class DamageCalculatorSystem : BaseSingleton<DamageCalculatorSystem>
{
    PlayerDataController playerDataController;
    private void Start()
    {
        playerDataController = LevelSystem._Instance.GetComponent<PlayerDataController>();
    }

    public DamageResoult DamageCalculator(float baseDamage)
    {
        DamageResoult damageResoult = new DamageResoult();
        //…À∫¶º∆À„¬ﬂº≠
        baseDamage += playerDataController.AttackPower;
        float rand = Random.Range(0f, 1f);
        if (rand <= playerDataController.CritRate)
        {
            damageResoult.damage = baseDamage * (1f + playerDataController.CritDamage);
            damageResoult.isCritical = true;
        }
        else
        {
            damageResoult.damage = baseDamage;
            damageResoult.isCritical = false;
        }
        return damageResoult;
    }
}
