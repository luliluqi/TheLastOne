using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSO", menuName = "MySO/New BulletSO")]
public class BulletSO : ScriptableObject
{
    public bool onHitRecycle = true;
    public float speed;
    public int damage;
    public float recycleTime = 1f;
}
