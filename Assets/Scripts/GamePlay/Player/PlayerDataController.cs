using UnityEngine;
using UnityEngine.Events;

public class PlayerDataController : MonoBehaviour
{
    /// <summary>
    /// ����ֵ
    /// </summary>
    [Header("����ֵ")][SerializeField] int hp;
    public int Hp { get { return hp; } }
    public void HpUp(float value)
    {
        if(hp + (int)(hp * value) <= maxHp)
            hp += (int)(hp * value);
        else hp = maxHp;
    }

    /// <summary>
    /// ��������ֵ
    /// </summary>
    [Header("��������")][SerializeField] int extraHp;
    public int ExtraHp { get { return extraHp; } }
    public void ExtraHpUp(int value)
    {
        extraHp += value;
    }

    /// <summary>
    /// ����
    /// </summary>
    [Header("����")][SerializeField] float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }
    public void MoveSpeedUp(float value)
    {
        moveSpeed *= 1f + value;
    }

    /// <summary>
    /// ������
    /// </summary>
    [Header("������")][SerializeField] float critRate;
    public float CritRate { get { return critRate; } }
    public void CritRateUp(float value)
    {
        critRate += value;
    }

    /// <summary>
    /// �����˺�
    /// </summary>
    [Header("�����˺�")][SerializeField] float critDamage;
    public float CritDamage { get { return critDamage; } }
    public void CritDamageUp(float value)
    {
        critDamage += value;
    }

    /// <summary>
    /// ����
    /// </summary>
    [Header("����")][SerializeField] float lucky;
    public float Lucky { get { return lucky; } }
    public void LuckyUp(float value)
    {
        if (lucky <= 50)
            lucky += value;
        else
            lucky = 50;
    }

    /// <summary>
    /// �Զ���Ѫ��
    /// </summary>
    [Header("�Զ���Ѫ��")][SerializeField] int autoTreatmentValue;
    public int AutoTreatmentValue { get { return autoTreatmentValue; } }
    public void AutoTreatmentValueUp(int value)
    {
        autoTreatmentValue += value;
    }

    /// <summary>
    /// ��Ѫ����
    /// </summary>
    [Header("��Ѫ����")][SerializeField] float treatmentRate;
    public float TreatmentRate { get { return treatmentRate; } }
    public void TreatmentRateUp(float value)
    {
        treatmentRate *= 1f + value;
    }

    /// <summary>
    /// ��ɱ��Ѫ
    /// </summary>
    [Header("��ɱ��Ѫ")][SerializeField] int killTreatment;
    public int KillTreatment { get { return killTreatment; } }
    public void KillTreatmentUp(int value)
    {
        killTreatment += value;
    }

    /// <summary>
    /// ������
    /// </summary>
    [Header("������")][SerializeField] int attackPower;
    public int AttackPower { get { return attackPower; } }
    public void AttackPowerUp(int value)
    {
        attackPower += value;
    }

    /// <summary>
    /// �ֿ���
    /// </summary>
    [Header("�ֿ���")][SerializeField] int resistance;
    public int Resistance { get { return resistance; } }
    public void ResistanceUp(int value)
    {
        if (resistance + value <= 50)
            resistance += value;
        else
            resistance = 50;
    }

    /// <summary>
    /// ������
    /// </summary>
    [Header("������")][SerializeField] float dodgeRate;
    public float DodgeRate { get { return dodgeRate; } }
    public void DodgeRateUp(float value)
    {
        if (dodgeRate + value <= 0.3f)
            dodgeRate += value;
        else
            dodgeRate = 0.3f;
    }

    /// <summary>
    /// ʰȡ��Χ
    /// </summary>
    [Header("ʰȡ��Χ")][SerializeField] float pickUpRange;
    public float PickUpRange { get { return pickUpRange; } }
    public void PickUpRangeUp(float value)
    {
        pickUpRange *= 1f + value;
    }

    /// <summary>
    /// �ȼ�������õ���������
    /// </summary>

    [Header("�ȼ���������������")][SerializeField] int maxHp;

    [Header("����������")][SerializeField] float growthCoefficient;

    int cHp;
    public int CHp
    {
        get { return cHp; }
    }

    [Header("�ܻ��޵�ʱ��")][SerializeField] float invincibleTime = 0.1f;
    float cInvincibleTime;

    public UnityAction<float> PlayerHpChange;
    public UnityAction PlayerDead;
    public UnityAction DataUpdateEvent;
    PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        LevelSystem._Instance.AddListenLevelUp(HpGrowth);
        cHp = hp + extraHp;
        cInvincibleTime = invincibleTime;

        DataUpdateEvent?.Invoke();

        DataUpdateEvent += HpUpdate;
        MyEventSystem._Instance.NpcDead += GetKillTreatment;
        GameManager._Instance.PauseEvent += Pause;
        GameManager._Instance.GameRestartEvent += ResetDate;
    }

    /// <summary>
    /// ���Ѫ������
    /// </summary>
    void HpGrowth()
    {
        if (hp < maxHp)
        {
            hp = (LevelSystem._Instance.Level * growthCoefficient + hp) < maxHp ? (LevelSystem._Instance.Level * 2 + hp) : maxHp;
        }
        PlayerHpChange?.Invoke((float)cHp / (float)(hp + extraHp));
        DataUpdateEvent?.Invoke();
    }

    void HpUpdate()
    {
        PlayerHpChange?.Invoke((float)cHp / (float)(hp + extraHp));
    }

    public void GetHurt(int damage)
    {
        controller.isHit = true;
        damage = (int)(1f - (resistance / 100f)) * damage;
        damage = damage == 0 ? 1 : damage;
        cHp -= damage;
        PlayerHpChange?.Invoke((float)cHp / (float)(hp + extraHp));
        if (cHp < 0)
        {
            cHp = 0;
            PlayerDead?.Invoke();
        }
    }

    public void GetKillTreatment()
    {
        cHp += KillTreatment;
        if (cHp > hp + extraHp)
        {
            cHp = hp + extraHp;
        }

        PlayerHpChange?.Invoke((float)cHp / (float)(hp + extraHp));
    }

    float treatmentTime;
    void AutoTreatment()
    {
        if (autoTreatmentValue > 0 || !isPause)
        {
            treatmentTime -= 0.02f;
            if (treatmentTime <= 0)
            {
                treatmentTime = treatmentRate;
                if (cHp + autoTreatmentValue <= hp + extraHp)
                {
                    cHp += autoTreatmentValue;
                }
                else
                {
                    cHp = hp + extraHp;
                }
            }
        }
    }

    public WeaponSO CurrentWeaponSO()
    {
        return controller.CurrentWeaponMgr.currentWeaponController.WeaponSO;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;

        if (controller.isHit == true) return;

        if (collision.CompareTag("Npc"))
        {
            float rand = Random.Range(0, 1f);
            if (rand > dodgeRate)
                GetHurt(collision.transform.GetComponent<NpcController>().Attack());
        }
    }

    private void FixedUpdate()
    {
        if(isPause) return;

        if (controller.isHit == true)
        {
            cInvincibleTime -= 0.02f;
            if (cInvincibleTime <= 0)
            {
                controller.isHit = false;
                cInvincibleTime = invincibleTime;
            }
        }

        AutoTreatment();
    }

    bool isPause;

    void Pause(bool isPause)
    {
        this.isPause = isPause;
    }

    public void ResetDate()
    {
        hp = 20;
        cHp = hp;
        extraHp = 0;
        moveSpeed = 5f;
        critRate = 0;
        critDamage = 0;
        lucky = 0;
        autoTreatmentValue = 0;
        treatmentRate = 5f;
        killTreatment = 0;
        attackPower = 0;
        resistance = 0;
        dodgeRate = 0f;
        pickUpRange = 1f;
        DataUpdateEvent?.Invoke();
    }
}
