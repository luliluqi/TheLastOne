using UnityEngine;

public class B762 : BaseBulletController
{
    Rigidbody2D rb;
    TrailRenderer bulletTrail;

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
            DamageResoult resoult = DamageCalculatorSystem._Instance.DamageCalculator(bulletSO.damage);

            if (collision.gameObject.activeInHierarchy)
            {
                //创建击中粒子特效
                if (transform.gameObject.activeInHierarchy)
                {
                    HitEffectSystem._Instance.CreateHitEffect(transform.position, "BleedEffect");
                }
                MyEventSystem._Instance.GenerateDamageText?.Invoke(resoult, new Vector2(transform.position.x, transform.position.y));
            }
            collision.GetComponent<NpcController>().GetHurt((int)resoult.damage);

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
