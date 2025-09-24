using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySO : ScriptableObject
{
    public string id;
    public Sprite icon;
    [TextArea] public string description;
    public List<PropertySO> abilityPropertyUp;

    public virtual void ResetSO() { }
}
