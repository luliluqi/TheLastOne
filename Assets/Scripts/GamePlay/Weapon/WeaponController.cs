using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField] protected WeaponSO weaponSO;
    public WeaponSO WeaponSO { get { return weaponSO; }}
    protected float cRate;
    protected float cCapacity;
    protected float cReloadTime;
    protected bool isReload;
    public bool canFire = true;

    protected Transform firePoint;
    protected AudioSource audioSource;
    protected AudioClip soundClip;

    /// <summary>
    /// ����
    /// </summary>
    public abstract bool Fire(Quaternion fireRot);
    /// <summary>
    /// װ��
    /// </summary>
    public abstract void Reload();

    /// <summary>
    /// ���������߼�
    /// </summary>
    protected abstract void FireCoolDown();

    protected virtual void Start()
    {
        firePoint = transform.Find("FirePoint");
        audioSource = GetComponent<AudioSource>();

        cRate = weaponSO.rate;
        cReloadTime = weaponSO.reloadTime;
        cCapacity = weaponSO.capacity;
    }

    protected virtual void FixedUpdate()
    {
        FireCoolDown();
    }
}
