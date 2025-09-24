using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectSystem : BaseSingleton<HitEffectSystem>
{
    public void CreateHitEffect(Vector3 pos, string effectName)
    {
        GameObject effect = MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.HitEffectPath, effectName);
        ParticalController pc = effect.GetComponent<ParticalController>();
        effect.transform.position = pos;
        pc.Play();
    }
}
