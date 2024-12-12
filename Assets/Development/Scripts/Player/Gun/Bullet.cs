using UnityEngine;
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;

    private System.Action onReturnToPool;

    [SerializeField] private AudioSource hitSound; 
    public void Init(System.Action onReturnToPool)
    {
        this.onReturnToPool = onReturnToPool;
        Invoke(nameof(ReturnToPool), lifetime);
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void ReturnToPool()
    {
        CancelInvoke();
        onReturnToPool?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(other.GetComponent<EnemyCharacter>())
            {
                EnemyCharacter enemyCharacter = other.GetComponent<EnemyCharacter>();
                enemyCharacter.DecreaseHealth(10);
                if(enemyCharacter.isDamagable)
                {
                    hitSound.Play();
                }
            }
            
        }
    }
}

