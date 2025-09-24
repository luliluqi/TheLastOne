using UnityEngine;

public class Explosion : BaseBulletController
{
    Animator animator;
    AudioSource source;
    AudioClip boomSound;

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        ResourcesLoadManager._Instance.ResourcesLoadObject<AudioClip>(ResourcesLoadManager._Instance.SoundPath + "Boom", out boomSound);
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

    protected override void MotionTrajectory()
    {
        int index = Random.Range(1, 4);
        animator.Play("Base Layer.Explosion" + index.ToString(), 0);
        source.clip = boomSound;
        source.Play();
    }

    public void TriggerPoint(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }

    protected override void FixedUpdate()
    {
        if (isRecycle || isPaused) return;
        currentTime -= 0.02f;
        if (currentTime <= 0)
        {
            AutoRecycle();
        }
    }

    Vector2 v;
    protected override void Pause(bool isPause)
    {
        this.isPaused = isPause;
        if (isPaused)
        {
            animator.speed = 0;
            source.Pause();
        }
        else
        {
            animator.speed = 1;
            source.UnPause();
        }
    }
}
