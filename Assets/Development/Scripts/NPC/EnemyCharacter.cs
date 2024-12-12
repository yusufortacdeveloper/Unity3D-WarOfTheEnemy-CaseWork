using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyProperty
{
    public float speed;
    public float health;
    public float rangeDistance;
}
public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private BaseState currentState;
    public NavMeshAgent navMeshAgent;
    public Animator anim;
    public EnemyProperty enemyProperty;
    public Transform player;

    public int health = 100;
    public bool isDamagable = true; 
    private void Start()
    {
        player = EventManager.Instance?.GetListener<Player>().transform;
        SetState(new PatrolState(this));
    }
    void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
    public void SetState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }
    public void BeCrazyFunc()
    {
        StartCoroutine(BeCrazy(player));
    }

    public void DecreaseHealth(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            if(isDamagable)
            {
                SetState(new DeadState(this));
                StartCoroutine("DeadEnemy");
                isDamagable = false;
            }
        
        }
    }
    public IEnumerator BeCrazy(Transform player)
    {
        transform.LookAt(player);
        anim.SetFloat("Blend", 2);
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(3f);
        SetState(new EnemyAttackState(this));
    }

    public IEnumerator DeadEnemy()
    {
        navMeshAgent.isStopped = true;
        anim.SetFloat("Blend", 6);
        LevelManager.Instance.DecreaseEnemyCount();
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
