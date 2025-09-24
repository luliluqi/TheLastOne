using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pg7vm : BaseBulletController
{
    protected Rigidbody2D rb;
    protected TrailRenderer bulletTrail;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletTrail = transform.GetChild(0).GetComponent<TrailRenderer>();
        currentTime = bulletSO.recycleTime;
    }

    protected override void OnEnable()
    {
        currentTime = bulletSO.recycleTime;
        isRecycle = false;

        GameManager._Instance.PauseEvent += Pause;
        GameManager._Instance.GameOverEvent += AutoRecycle;
        MotionTrajectory();
    }

    protected override void OnDisable()
    {
        GameManager._Instance.PauseEvent -= Pause;
        GameManager._Instance.GameOverEvent -= AutoRecycle;
        bulletTrail.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Npc"))
        {
            if (collision.gameObject.activeInHierarchy)
            {
                DamageResoult resoult = DamageCalculatorSystem._Instance.DamageCalculator(bulletSO.damage);

                //生成爆炸子弹
                if (transform.gameObject.activeInHierarchy)
                {
                    AmmunitionSystem._Instance.GetOtherTypeBullet("Explosion").GetComponent<Explosion>().TriggerPoint(transform.position);
                }
                MyEventSystem._Instance.GenerateDamageText?.Invoke(resoult, new Vector2(collision.transform.position.x, collision.transform.position.y));
            }
            collision.GetComponent<NpcController>().GetHurt(bulletSO.damage);
            if (bulletSO.onHitRecycle)
            {
                HitRecycle();
            }
        }
    }

    protected override void HitRecycle()
    {
        if (isRecycle) return;
        isRecycle = true;
        AmmunitionSystem._Instance.RecycleBullet(gameObject);
    }

    protected override void AutoRecycle()
    {
        isRecycle = true;
        AmmunitionSystem._Instance.RecycleBullet(gameObject);
    }

    /// <summary>
    /// 子弹运动轨迹
    /// </summary>
    protected override void MotionTrajectory()
    {
        rb.velocity = transform.right * bulletSO.speed;
    }

    Vector2 v;
    protected override void Pause(bool isPause)
    {
        this.isPaused = isPause;
        if (isPaused)
        {
            v = rb.velocity;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = v;
        }
    }

}
