using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureController : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager._Instance.GameRestartEvent += RecycleTreasure;
    }

    private void OnDisable()
    {
        GameManager._Instance.GameRestartEvent -= RecycleTreasure;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            MyEventSystem._Instance.PickUpTreasure?.Invoke();
            RecycleTreasure();
        }
    }

    void RecycleTreasure()
    {
        MyObjectPool._Instance.RecycleObject(gameObject);
    }
}
