using UnityEngine;

public class MagicArrowBullet : BaseBulletController
{
    protected Rigidbody2D rb;
    protected Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Npc"))
        {
            DamageResoult resoult = DamageCalculatorSystem._Instance.DamageCalculator(bulletSO.damage);

            if (collision.gameObject.activeInHierarchy)
                MyEventSystem._Instance.GenerateDamageText?.Invoke(resoult, new Vector2(collision.transform.position.x, collision.transform.position.y));
            collision.GetComponent<NpcController>().GetHurt(bulletSO.damage);
        }
    }

    protected override void HitRecycle()
    {

    }


    protected override void AutoRecycle()
    {
        isRecycle = true;
        AmmunitionSystem._Instance.RecycleBullet(gameObject);
    }

    /// <summary>
    /// ×Óµ¯ÔË¶¯¹ì¼£
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
            animator.speed = 0f;
        }
        else
        {
            rb.velocity = v;
            animator.speed = 1f;
        }
    }
}
