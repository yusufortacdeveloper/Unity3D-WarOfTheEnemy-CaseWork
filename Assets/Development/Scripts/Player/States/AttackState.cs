using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using System.Collections;

public class AttackState : MonoBehaviour, IEventListener
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private FixedJoystick attackJoystick;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Animator anim;

    private PlayerMovement playerMovementSc;
    private GrenadeState grenadeStateSc;

    public bool isAttack = false,isReloading = false;

    public float _horizontal;
    public float _vertical;

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int poolSize = 20, currentBulletCount = 30, maxBulletCount = 30;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private ObjectPool<Bullet> bulletPool;
    private float nextFireTime = 0f;

    [SerializeField] private AudioSource gunSound,reloadSound;
    private void OnEnable()
    {
        EventManager.Instance?.RegisterListener(this);
    }
    private void OnDisable()
    {
        EventManager.Instance?.UnregisterListener(this);
    }
    private void Start()
    {
        grenadeStateSc = EventManager.Instance?.GetListener<GrenadeState>();
        playerMovementSc = EventManager.Instance?.GetListener<PlayerMovement>();
        bulletPool = ObjectPoolManager.Instance.GetOrCreatePool("BulletPool", bulletPrefab, transform, poolSize);
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }
    private void FixedUpdate()
    {
        if (!grenadeStateSc.isGrenade && !playerMovementSc.isMove)
        {
            attackJoystick.gameObject.SetActive(true);
            GetMovementInputs();
            RotateBody();

            if (_horizontal != 0 || _vertical != 0)
            {
                if (Time.time >= nextFireTime)
                {
                    Fire();
                    nextFireTime = Time.time + fireRate;
                }
            }
        }
        else
        {
            attackJoystick.gameObject.SetActive(false);
        }
    }
    
    public void StopShake()
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
    public void StartShake()
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = 2f;
            noise.m_FrequencyGain = 2f;
        }
    }
    private void Fire()
    {
        if(currentBulletCount > 0)
        {
            muzzleFlash.Play();
            Bullet bullet = bulletPool.Get();
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.Init(() => bulletPool.ReturnToPool(bullet));
            currentBulletCount--;
            StartShake();
            gunSound.Play();
        }
        else
        {
            StopShake();
            muzzleFlash.Stop();
            gunSound.Stop();
        }
    }
    public void Reload()
    {
        if(currentBulletCount < maxBulletCount)
        {
            if (isReloading == false)
            {
                StartCoroutine("ReloadAmmo");
                isReloading = true;
            }
        }
    }
    IEnumerator ReloadAmmo()
    {
        reloadSound.Play();
        attackJoystick.gameObject.SetActive(false);
        grenadeStateSc.isGrenade = true;
        playerMovementSc.isMove = true;
        anim.SetBool("reload", true);
        yield return new WaitForSeconds(3f);
        reloadSound.Stop();
        grenadeStateSc.isGrenade = false;
        playerMovementSc.isMove = false;
        anim.SetBool("reload", false);
        currentBulletCount = maxBulletCount;
        attackJoystick.gameObject.SetActive(true);
        isReloading = false;
    }
    public void RotateBody()
    {
        if (_horizontal != 0 || _vertical != 0)
        {
            isAttack = true;
            playerBody.rotation = Quaternion.LookRotation(GetVelocity());
        }
        else
        {
            muzzleFlash.Stop();
            StopShake();
            isAttack = false;
        }
    }
    public Vector3 GetVelocity()
    {
        return new Vector3(_horizontal, 0, _vertical) * rotateSpeed * Time.deltaTime;
    }
    private void GetMovementInputs()
    {
        _horizontal = attackJoystick.Horizontal;
        _vertical = attackJoystick.Vertical;
    }
}
