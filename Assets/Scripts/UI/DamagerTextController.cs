using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagerTextController : MonoBehaviour
{
    [SerializeField] float recycleTime;
    [SerializeField] float jumpSpeed;
    [SerializeField] float scaleSpeed;
    [SerializeField] TextMeshProUGUI textUI;
    float cTime;
    Color defaultColor = new Color(1, 0.65f, 0.5f, 1f);
    Color criticalColor = new Color(1, 0.37f, 0.31f, 1f);

    public void BeCreate(DamageResoult resoult, Vector2 pos)
    {
        GameManager._Instance.PauseEvent += Pause;
        cTime = recycleTime;
        this.textUI.text = ((int)resoult.damage).ToString();
        if (resoult.isCritical)
        {
            this.textUI.color = criticalColor;
        }
        transform.position = pos;
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager._Instance.PauseEvent -= Pause;
    }

    private void FixedUpdate()
    {
        if (isPause) return;
        cTime -= 0.02f;
        if (cTime <= 0)
        {
            OnRecycle();
        }
        floatEffect();
    }

    void floatEffect()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + jumpSpeed);
        if (cTime > recycleTime / 3)
            transform.localScale = new Vector3(transform.localScale.x + scaleSpeed, transform.localScale.y + scaleSpeed, transform.localScale.z);
        else
            transform.localScale = new Vector3(transform.localScale.x - scaleSpeed, transform.localScale.y - scaleSpeed, transform.localScale.z);
    }

    void OnRecycle()
    {
        textUI.color = defaultColor;
        transform.localScale = new Vector3(0.5f, 0.5f, 1);
        MyObjectPool._Instance.RecycleObject(gameObject, transform.parent);
    }

    bool isPause;
    void Pause(bool isPause)
    {
        this.isPause = isPause;
    }
}
