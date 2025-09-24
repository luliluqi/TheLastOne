using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : NpcController
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();     
    }

    public override void Dead()
    {
        base.Dead();
    }

    public override void GetMoveDir(Vector2 target)
    {
        if (isPause) return;

        moveDir = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * npcSO.moveSpeed;
        if (moveDir.x < 0)
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

    public override void GetHurt(int damage)
    {
        cHp -= damage;
        if(cHp <= 0)
        {
            Dead();
        }
    }

    protected override void Pause(bool isPause)
    {
        this.isPause = isPause;
        if (this.isPause)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }
}
