using UnityEngine;

public abstract class BaseBulletController : MonoBehaviour
{
    [SerializeField] protected BulletSO bulletSO;
    protected float currentTime;
    protected bool isRecycle = false;
    protected bool isPaused = false;
    protected bool isHit = false;

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    /// <summary>
    /// �ӵ����л���
    /// </summary>
    protected abstract void HitRecycle();

    protected abstract void AutoRecycle();

    /// <summary>
    /// �ӵ��˶��켣
    /// </summary>
    protected abstract void MotionTrajectory();

    protected abstract void Pause(bool isPause);


    protected virtual void FixedUpdate()
    {
        if (isPaused) return;

        if (isRecycle && bulletSO.onHitRecycle) return;

        if (currentTime >= 0)
        {
            currentTime -= 0.02f;
        }
        else
        {
            AutoRecycle();
        }
    }
}
