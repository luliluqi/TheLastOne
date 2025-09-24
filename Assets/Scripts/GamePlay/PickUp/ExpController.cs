using UnityEngine;

public class ExpController : MonoBehaviour
{
    [SerializeField] int expValue;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float autoRecycleTime = 5f;
    float recycleTime;
    float reverseMoveTime;
    bool isPickUp;
    bool isPause;

    private void OnEnable()
    {
        GameManager._Instance.PauseEvent += Pause;
        isPickUp = false;
        isPause = false;
        reverseMoveTime = 0.5f;
        recycleTime = autoRecycleTime;
    }

    private void OnDisable()
    {
        GameManager._Instance.PauseEvent -= Pause;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            LevelSystem._Instance.GetExp(expValue);
            ExpRecycle();
        }
    }

    public void MoveToPlayer(Transform target, float checklength)
    {
        if (isPause) return;

        Vector2 distance =target.position - transform.position;
        if (!isPickUp)
        {
            if ((distance.x * distance.x + distance.y * distance.y) < checklength * checklength)
            {
                isPickUp = true;
            }
        }
        else
        {
            if(reverseMoveTime > 0)
                transform.Translate(-distance.normalized * moveSpeed / 2 * Time.deltaTime, Space.Self);
            else 
                transform.Translate(distance.normalized * moveSpeed * Time.deltaTime, Space.Self);
        }
    }

    void ExpRecycle()
    {
        MyEventSystem._Instance.PickUpExp?.Invoke(this);
        MyObjectPool._Instance.RecycleObject(gameObject);
    }

    void Pause(bool isPause)
    {
        this.isPause = isPause;
    }

    private void FixedUpdate()
    {
        if(isPause) return;

        if (isPickUp)
        {
            reverseMoveTime -= 0.02f;
        }

        recycleTime -= 0.02f;
        if(recycleTime <= 0)
        {
            ExpRecycle();
        }
    }
}
