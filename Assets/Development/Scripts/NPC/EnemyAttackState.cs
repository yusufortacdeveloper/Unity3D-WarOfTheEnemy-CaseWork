using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : BaseState
{
    private EnemyCharacter character;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private float attackRange = 2.0f;
    private float chaseRange = 10.0f;
    private float attackCooldown = 1.5f; // Saldýrýlar arasýndaki süre
    private float lastAttackTime = 0f; // Son saldýrýnýn zamaný

    public EnemyAttackState(EnemyCharacter character)
    {
        this.character = character;
        this.agent = character.navMeshAgent;
        this.player = character.player;
        this.animator = character.anim;
    }

    public override void Enter()
    {
        animator.SetFloat("Blend", 4);
        agent.isStopped = false;
    }

    public override void Update()
    {
        float distanceToPlayer = Vector3.Distance(character.transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            agent.ResetPath();
            animator.SetFloat("Blend", 0);
        }
    }

    public override void Exit()
    {
        agent.ResetPath();
    }

    private void ChasePlayer()
    {
        if (!agent.pathPending && agent.destination != player.position)
        {
            agent.SetDestination(player.position);
        }
        animator.SetFloat("Blend", 4);
    }

    private void AttackPlayer()
    {
        agent.ResetPath();
        animator.SetFloat("Blend", 3);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            DealDamage();
            lastAttackTime = Time.time;
        }
    }
    private void DealDamage()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.isDamagable)
        {
            playerHealth.DecreaseHealth(10); 
        }
    }
}
