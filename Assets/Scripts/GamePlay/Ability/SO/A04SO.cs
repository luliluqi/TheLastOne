using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "A04SO", menuName = "MySO/New AbilitySO/A04SO")]
public class A04SO : AbilitySO
{
    public float triggerTime;

    public override void ResetSO()
    {
        triggerTime = 2f;
    }
}