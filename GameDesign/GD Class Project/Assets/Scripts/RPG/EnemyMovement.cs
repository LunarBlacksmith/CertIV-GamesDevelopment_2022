using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Nav mesh

public class EnemyMovement : GradientHealth
{
    public enum AIStates
    {
        Patrol,
        Seek,
        Attack,
        Die
    }

    public AIStates state;
    public Transform player;
    public Transform waypointParent;
    public Transform[] waypoints;
    public int curWaypoint, difficulty;
    public NavMeshAgent agent;
    public float walkSpeed, runSpeed, attackSpeed, turnSpeed, attackRange, sightRange, baseDamage, critAmount;
    public bool isDead;
    public float distanceToPoint, changeWaypointWhenThisClose;
    public float stopFromPlayer;
    public Animator anim;

    public override void Start()
    {
        base.Start();
        Debug.Log("AHH 2");
        //Get waypoints array from waypoint parent
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
        //Get navMeshAgent from self
        agent = GetComponent<NavMeshAgent>();
        //Set speed of agent
        agent.speed = walkSpeed;
        //Get Animator from self
        anim = GetComponent<Animator>();
        //Set target waypoint
        curWaypoint = 1;
        //Set Patrol as default
        Patrol();
    }

    public override void Update()
    {
        base.Update();
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);

        Patrol();
        Seek();
        Attack();
        Die();
        FaceTarget();
    }

    void FaceTarget()
    {
        Vector3 turnTowardsSteeringTarget = agent.steeringTarget;
        Vector3 direction = (turnTowardsSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //smooth rotation (Slerp)
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
    void Patrol()
    {
        //If there are no more waypoints, or if player is within sightRange, or AI is dead
        if (waypoints.Length <= 0 || Vector3.Distance(player.position, transform.position) <= sightRange || isDead)
        {
            //get out of the Patrol method
            return;
        }

        agent.stoppingDistance = 0;
        agent.speed = walkSpeed;
        state = AIStates.Patrol;
        anim.SetBool("Walk", true);
        agent.destination = waypoints[curWaypoint].position;
        distanceToPoint = Vector3.Distance(transform.position, waypoints[curWaypoint].position);

        if (distanceToPoint <= changeWaypointWhenThisClose)
        {
            if (curWaypoint < waypoints.Length-1)
            {
                ++curWaypoint;
            }
            else
            {
                curWaypoint = 1;
            }
        }
    }
    void Seek()
    {
        //If player is out of sightRange, or within attackRange, or AI is dead
        if (Vector3.Distance(player.position, transform.position) > sightRange || Vector3.Distance(player.position, transform.position) < attackRange || isDead)
        {
            return;
        }
        state = AIStates.Seek;
        anim.SetBool("Run", true);
        agent.stoppingDistance = stopFromPlayer;
        agent.speed = runSpeed;
        agent.destination = player.position;
    }
    void Attack()
    {
        //If player is out of attackRange, or AI is dead, or player is dead
        if (Vector3.Distance(player.position, transform.position) > attackRange || isDead /* || PlayerHandler.isDead*/)
        {
            return;
        }
        state = AIStates.Attack;
        anim.SetBool("Attack", true);
        agent.stoppingDistance = stopFromPlayer;
        agent.speed = 0;
    }
    void Die()
    {
        if (attributes[0].curValue >= 0 || isDead)
        {
            return;
        }

        state = AIStates.Die;
        anim.SetTrigger("Dead");
        isDead = true;
        agent.speed = 0;
        agent.destination = transform.position;
        agent.enabled = false;
    }
}
