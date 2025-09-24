using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : NpcController
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Dead()
    {
        if (isDead) return;
        isDead = true;
        //生成经验
        MyEventSystem._Instance.GenerateExp?.Invoke(transform.position);

        MyEventSystem._Instance.NpcDead?.Invoke();

        //概率生成宝箱
        MyEventSystem._Instance.GenerateTreasure?.Invoke(transform.position);

        GameManager._Instance.PauseEvent -= Pause;
        MyObjectPool._Instance.RecycleObject(gameObject);

        NpcManager._Instance.RemoveNpc(this);
    }

    public override void GetHurt(int damage)
    {
        cHp -= damage;
        if (cHp <= 0)
        {
            Dead();
        }
    }

    public override void GetMoveDir(Vector2 target)
    {
        if (isPause) return;

        moveDir = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * npcSO.moveSpeed;
        if(moveDir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void Spwan(Vector2 pos)
    {
        GameManager._Instance.PauseEvent += Pause;
        transform.SetPositionAndRotation(pos, Quaternion.identity);
        cHp = npcSO.hp;
        isDead = false;
        gameObject.SetActive(true);
    }

    protected override void Pause(bool isPause)
    {
        this.isPause = isPause;
        if (isPause)
        {
            animator.speed = 0f;
        }else
        {
            animator.speed = 1f;
        }
    }
}
