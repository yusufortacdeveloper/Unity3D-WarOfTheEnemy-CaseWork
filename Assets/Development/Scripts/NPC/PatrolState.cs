using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseState
{
    private EnemyCharacter character;
    private NavMeshAgent agent;
    private Vector3 targetPosition;
    private Animator animator;
    private float idleTime = 10f; 
    private float idleTimer;
    private Transform player; 
    public PatrolState(EnemyCharacter character)
    {
        this.character = character;
        this.agent = character.navMeshAgent;
        this.animator = character.anim;
        this.player = character.player; 
    }
    public override void Enter()
    {
        SetRandomDestination();
    }
    public override void Update()
    {
        if (Vector3.Distance(player.position, character.transform.position) < 4f)
        {
            character.SetState(new FindState(character));
        }

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (idleTimer <= 0)
            {
                animator.SetFloat("Blend", 0); 
                idleTimer = idleTime; 
            }
            else
            {
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0)
                {
                    SetRandomDestination();
                }
            }
        }
        else
        {
            animator.SetFloat("Blend", 1); 
        }
    }
    public override void Exit()
    {
        agent.ResetPath(); 
    }
    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f; 
        randomDirection += character.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 0.5f, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
            agent.SetDestination(targetPosition);
        }
    }
}