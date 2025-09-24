using UnityEngine;

public abstract class NpcController : MonoBehaviour
{
    [SerializeField]
    protected NpcSO npcSO;
    protected int cHp;
    protected Vector2 moveDir = Vector2.zero;
    protected bool isDead;
    protected bool isPause;

    public abstract void Spwan(Vector2 pos);

    public virtual void Dead()
    {
        if (isDead) return;
        isDead = true;
        //生成经验
        MyEventSystem._Instance.GenerateExp?.Invoke(transform.position);

        MyEventSystem._Instance.NpcDead?.Invoke();

        GameManager._Instance.PauseEvent -= Pause;
        MyObjectPool._Instance.RecycleObject(gameObject);

        NpcManager._Instance.RemoveNpc(this);
    }

    public virtual void Clear()
    {
        GameManager._Instance.PauseEvent -= Pause;
        MyObjectPool._Instance.RecycleObject(gameObject);
    }

    public abstract void GetMoveDir(Vector2 target);

    public virtual void Move()
    {
        if(isPause) return;
        transform.Translate(moveDir * Time.deltaTime, Space.Self);
    }

    public abstract void GetHurt(int damage);

    public virtual int Attack()
    {
        return npcSO.damage;
    }

    protected abstract void Pause(bool isPause);

    protected virtual void Update()
    {
        Move();
    }
}
