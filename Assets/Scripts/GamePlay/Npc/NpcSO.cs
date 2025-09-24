using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcSO", menuName = "MySO/New NpcSO")]
public class NpcSO : ScriptableObject
{
    [SerializeField] int baseHP;
    [SerializeField] float baseMoveSpeed;
    [SerializeField] int baseDamage;
    [SerializeField] int hpGrowth;
    [SerializeField] float speedGrowth;
    [SerializeField] int damageGrowth;
    [SerializeField] float maxSpeed;
    
    [HideInInspector] public int hp;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int damage;

    public void UpdateNpcData()
    {
        hp += hpGrowth;
        moveSpeed = moveSpeed + speedGrowth > maxSpeed ? maxSpeed : moveSpeed + speedGrowth;
        damage += damageGrowth;
    }

    public void Init()
    {
        hp = baseHP;
        moveSpeed = baseMoveSpeed;
        damage = baseDamage;
    }
}
