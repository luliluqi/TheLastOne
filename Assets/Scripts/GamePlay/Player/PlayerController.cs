using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerDataController playerDC;
    Vector2 moveDirection = Vector2.zero;
    Rigidbody2D rb;
    Camera cam;
    Transform gunRoot;
    Transform sprite;
    Transform weapon;
    Animator animator;
    WeaponManager currentWeaponMgr;
    public WeaponManager CurrentWeaponMgr { get { return currentWeaponMgr; } }

    bool isFire = false;
    bool isMove = false;
    public bool isHit = false;

    bool isPause = false;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        gunRoot = transform.Find("GunRoot");
        sprite = transform.Find("Sprite");
        weapon = gunRoot.Find("Weapon");
        currentWeaponMgr = weapon.GetComponent<WeaponManager>();
        animator = GetComponent<Animator>();
        playerDC = GetComponent<PlayerDataController>();

        GameManager._Instance.PauseEvent += Pause;

        //开启枪械开火携程
        StartCoroutine(FireCoroutine());
    }

    void Update()
    {
        if (isPause) return;

        rb.velocity = new Vector2(moveDirection.x * playerDC.MoveSpeed, moveDirection.y * playerDC.MoveSpeed);
        Aim();
        AnimationStateControl();
    }

    public void MoveInput(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();
        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }
    }

    public void FireInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isFire = true;
        }

        if (ctx.canceled)
        {
            isFire = false;
        }
    }

    protected UnityAction openBuildPanel;
    public void AddListenOpenBuildPanel(UnityAction func)
    {
        openBuildPanel += func;
    }
    public void RemoveListenOpenBuildPanel(UnityAction func)
    {
        openBuildPanel -= func;
    }

    public void OpenBuildPanelInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            openBuildPanel?.Invoke();
        }
    }

    Vector2 impulseDir;
    void Fire()
    {
        if (currentWeaponMgr.currentWeaponController.canFire)
        {
            if (!currentWeaponMgr.currentWeaponController.Fire(gunRoot.rotation)) return;

            impulseDir = Input.mousePosition - cam.WorldToScreenPoint(gunRoot.position);
            impulseDir.x = impulseDir.x > 0 ? 1 : -1;
            impulseDir.y = impulseDir.y > 0 ? 1 : -1;

            currentWeaponMgr.CameraImpulse(impulseDir);
        }
    }

    void Aim()
    {
        Vector3 dir = cam.WorldToScreenPoint(gunRoot.position);
        dir = Input.mousePosition - dir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunRoot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (gunRoot.rotation.eulerAngles.z >= 270f || gunRoot.rotation.eulerAngles.z <= 90f)
        {
            sprite.rotation = new Quaternion(0, 0, 0, 0);
            weapon.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            sprite.rotation = new Quaternion(0, 180f, 0, 0);
            weapon.localRotation = Quaternion.Euler(180f, 0f, 0f);
        }
    }

    public Quaternion GetAimDiration()
    {
        return gunRoot.rotation;
    }

    void AnimationStateControl()
    {
        animator.SetBool("IsMove", isMove);
        animator.SetBool("IsHit", isHit);
    }

    //记录暂停时的运动速度
    Vector2 currentSpeed;
    void Pause(bool isPause)
    {
        this.isPause = isPause;

        if (isPause)
        {
            currentSpeed = rb.velocity;
            rb.velocity = Vector2.zero;
            animator.speed = 0f;
        }
        else
        {
            rb.velocity = currentSpeed;
            animator.speed = 1f;
        }
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            yield return null;
            if (isFire && !isPause)
            {
                Fire();
            }
        }
    }
}
