using UnityEngine;

public class PlayerMovement : MonoBehaviour, IEventListener
{
    [SerializeField] private Rigidbody rigPlayer;
    [SerializeField] private FixedJoystick moveJoystick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed = 5f; // Maksimum hýz sýnýrý

    private GrenadeState grenadeStateSc;
    private AttackState attackStateSc;

    [SerializeField] private AudioSource movementSoundSfx;

    public bool isMove = false;
    private bool wasMoving = false; // Önceki hareket durumunu takip eder

    public float _horizontal;
    public float _vertical;

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
        attackStateSc = EventManager.Instance?.GetListener<AttackState>();
    }

    private void FixedUpdate()
    {
        if (!grenadeStateSc.isGrenade && !attackStateSc.isAttack)
        {
            moveJoystick.gameObject.SetActive(true);
            GetMovementInputs();
            SetMovement();
        }
        else
        {
            moveJoystick.gameObject.SetActive(false);
            StopMovement();
        }

        LimitVelocity();
        HandleMovementSound(); // Hareket seslerini kontrol et
    }

    public Vector3 GetVelocity()
    {
        return new Vector3(_horizontal, 0f, _vertical).normalized * moveSpeed;
    }

    private void SetMovement()
    {
        Vector3 velocity = GetVelocity();
        rigPlayer.velocity = new Vector3(velocity.x, rigPlayer.velocity.y, velocity.z);

        isMove = (_horizontal != 0 || _vertical != 0);
    }

    private void GetMovementInputs()
    {
        _horizontal = moveJoystick.Horizontal;
        _vertical = moveJoystick.Vertical;
    }

    private void LimitVelocity()
    {
        Vector3 flatVelocity = new Vector3(rigPlayer.velocity.x, 0f, rigPlayer.velocity.z);
        if (flatVelocity.magnitude > maxSpeed)
        {
            flatVelocity = flatVelocity.normalized * maxSpeed;
            rigPlayer.velocity = new Vector3(flatVelocity.x, rigPlayer.velocity.y, flatVelocity.z);
        }
    }

    private void StopMovement()
    {
        rigPlayer.velocity = new Vector3(0f, rigPlayer.velocity.y, 0f);
        isMove = false;
    }

    private void HandleMovementSound()
    {
        if (isMove && !wasMoving)
        {
            movementSoundSfx.pitch = Random.Range(1f, 1.4f);
            movementSoundSfx.Play();
        }
        else if (!isMove && wasMoving)
        {
            movementSoundSfx.Stop();
        }

        wasMoving = isMove; // Mevcut hareket durumunu sakla
    }
}
