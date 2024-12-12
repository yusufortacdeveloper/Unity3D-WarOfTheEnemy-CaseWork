using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindState : BaseState
{
    private EnemyCharacter character;
    private Animator anim;
    private NavMeshAgent navMesh;
    private Transform player; 
    public FindState(EnemyCharacter character)
    {
        this.character = character;
        this.anim = character.anim;
        this.navMesh = character.navMeshAgent;
        this.player = character.player;
    }
    public override void Enter()
    {
        character.BeCrazyFunc();
    }
    public override void Update()
    {

    }
    public override void Exit()
    {

    }
}
