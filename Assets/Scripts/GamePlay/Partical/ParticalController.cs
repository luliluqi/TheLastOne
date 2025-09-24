using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalController : MonoBehaviour
{
    ParticleSystem PS;
    bool isPlaying;

    private void Awake()
    {
        PS = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        gameObject.SetActive(true);
        PS.Play();
        isPlaying = true;
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (PS.isStopped)
        {
            isPlaying = false;
            MyObjectPool._Instance.RecycleObject(gameObject);
        }
    }
}
