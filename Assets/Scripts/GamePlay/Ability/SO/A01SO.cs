using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "A01SO", menuName = "MySO/New AbilitySO/A01SO")]
public class A01SO : AbilitySO
{
    public float triggerTime;
    public int generateNumber;
    public float areaLength;

    public override void ResetSO()
    {
        triggerTime = 2f;
        generateNumber = 2;
        areaLength = 5;
    }
}
