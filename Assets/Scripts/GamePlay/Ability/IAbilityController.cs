using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityController
{
    /// <summary>
    /// ƒ‹¡¶ Õ∑≈
    /// </summary>
    public void Release();

    public void BeCreate<T>(T t);
    public void BeDestroy();

    public AbilityInfo GetAbilityInfo();
    public AbilitySO GetAbilitySO();

    public GameObject Getobj();

    public void Pause(bool isPause);
}
