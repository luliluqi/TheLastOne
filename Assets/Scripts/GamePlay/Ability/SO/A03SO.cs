using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "A03SO", menuName = "MySO/New AbilitySO/A03SO")]
public class A03SO : AbilitySO
{
    public float orbitRadius;
    public float orbitSpeed;
    public int damage;

    public override void ResetSO()
    {
        orbitRadius = 3f;
        orbitSpeed = 100f;
        damage = 5;
    }
}
