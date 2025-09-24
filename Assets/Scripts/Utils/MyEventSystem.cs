using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyEventSystem : BaseSingleton<MyEventSystem>
{
    public UnityAction<DamageResoult, Vector2> GenerateDamageText;

    public UnityAction NpcDead;

    public UnityAction<Vector2> GenerateExp;

    public UnityAction<ExpController> PickUpExp;

    public UnityAction<Vector2> GenerateTreasure;

    public UnityAction PickUpTreasure;

    public UnityAction<TreasureInfo> LoadTreasureToUI;

    public UnityAction<string> ChangeWeapon;
}
