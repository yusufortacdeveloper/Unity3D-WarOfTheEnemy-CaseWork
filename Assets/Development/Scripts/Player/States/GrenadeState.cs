using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Cinemachine;

public class GrenadeState :MonoBehaviour , IEventListener
{
    [SerializeField] private Transform playerBody,grenadeTransform,grenadeAreaTransform,warPlaneSpawnPoz;
    [SerializeField] private FixedJoystick grenadeJoystick;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem nukeVfx;

    private PlayerMovement playerMovementSc;
    private AttackState attackStateSc;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer noise;

    public bool isGrenade, isThrowing = false;

    public float _horizontal;
    public float _vertical;

    [SerializeField] private GameObject warPlanePrefab;

    [SerializeField] private GrenadePoz grenadePoz;

    [SerializeField] private AudioSource nukeSound, airSound; 


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
        attackStateSc = EventManager.Instance?.GetListener<AttackState>();
        playerMovementSc = EventManager.Instance?.GetListener<PlayerMovement>();
        grenadeAreaTransform.gameObject.SetActive(false);
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        }
    }
    private void FixedUpdate()
    {
        if(!attackStateSc.isAttack && !playerMovementSc.isMove)
        {
            grenadeJoystick.gameObject.SetActive(true);
            GetMovementInputs();
            RotateBody();
        }
        else
        {
            grenadeJoystick.gameObject.SetActive(false);
        }

    }
    public void RotateBody()
    {
        if ((_horizontal != 0 || _vertical != 0))
        {
            SmoothFollowOffsetChange(12f); 
            isThrowing = true;

            Vector3 velocity = GetVelocity();
            if (velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(velocity);

                playerBody.DORotateQuaternion(targetRotation, 0.3f)
                    .SetEase(Ease.OutSine);
            }
            isGrenade = true;
            grenadeAreaTransform.gameObject.SetActive(true);
            grenadeTransform.rotation = Quaternion.LookRotation(GetVelocity());
        }
        else
        {
            isGrenade = false;

            if (isThrowing)
            {
                airSound.Play();
                StartCoroutine("StartGrenadePlane");
                isThrowing = false;
            }
        }
    }
    IEnumerator StartGrenadePlane()
    {
        Instantiate(warPlanePrefab, warPlaneSpawnPoz.position, Quaternion.Euler(0f, -90f, 0f));
        attackStateSc.isAttack = true;
        playerMovementSc.isMove = true;
        isGrenade = true;
        anim.SetBool("grenade", true);
        nukeVfx.Play();
        nukeSound.Play();
        attackStateSc.StartShake();
        List<EnemyCharacter> enemys = grenadePoz.GetAffectedEnemies();
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].DecreaseHealth(500);
        }
        yield return new WaitForSeconds(3f);
        nukeVfx.Stop();
        nukeSound.Stop();
        attackStateSc.isAttack = false;
        playerMovementSc.isMove = false;
        isGrenade = false;
        anim.SetBool("grenade", false);
        nukeVfx.Stop();
        attackStateSc.StopShake();
        grenadeAreaTransform.gameObject.SetActive(false);
        SmoothFollowOffsetChange(6.05f); 
    }
    private void SmoothFollowOffsetChange(float targetValue)
    {
        CinemachineTransposer body = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (body != null)
        {
            DOTween.To(() => body.m_FollowOffset.y,
                       y =>
                       {
                           Vector3 offset = body.m_FollowOffset;
                           offset.y = y;
                           body.m_FollowOffset = offset;
                       },
                       targetValue,
                       0.5f) // Geçiþ süresi
                  .SetEase(Ease.OutSine);
        }
    }

    public Vector3 GetVelocity()
    {
        return new Vector3(_horizontal, 0, _vertical) * rotateSpeed * Time.deltaTime;
    }
    private void GetMovementInputs()
    {
        _horizontal = grenadeJoystick.Horizontal;
        _vertical = grenadeJoystick.Vertical;
    }
}
