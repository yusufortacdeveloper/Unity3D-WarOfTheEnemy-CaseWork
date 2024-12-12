using UnityEngine;
using DG.Tweening;

public class PlayerLook : MonoBehaviour, IEventListener
{
    [SerializeField] private Transform body;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private PlayerMovement playerMovementSc;
    [SerializeField] private ParticleSystem floorSmokeEffect;
    [SerializeField] private float rotationDuration = 0.3f; 
    private void OnEnable()
    {
        EventManager.Instance?.RegisterListener(this);
    }
    private void OnDisable()
    {
        EventManager.Instance?.UnregisterListener(this);
    }
    void Start()
    {
        playerMovementSc = EventManager.Instance?.GetListener<PlayerMovement>();
    }

    private void Update()
    {
        SetRotation();
    }
    private void SetRotation()
    {
        if (playerMovementSc._horizontal != 0 || playerMovementSc._vertical != 0)
        {
            Vector3 velocity = playerMovementSc.GetVelocity();
            if (velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(velocity);

                body.DORotateQuaternion(targetRotation, rotationDuration)
                    .SetEase(Ease.OutSine);
            }

            playerAnim.SetBool("run", true);
            if (!floorSmokeEffect.isPlaying)
                floorSmokeEffect.Play();
        }
        else
        {
            playerAnim.SetBool("run", false);
            if (floorSmokeEffect.isPlaying)
                floorSmokeEffect.Stop();
        }
    }
}
