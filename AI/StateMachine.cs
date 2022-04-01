using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        attack,
        defend,
        run,
        berrypick
    }

    public State currentState;

    public AIMovement aiMovement;

    public float chaseDist;

    private void Start()
    {
        aiMovement = GetComponent<AIMovement>();
        NextState();
    }

    private void NextState()
    {
        switch (currentState)
        {
            case State.attack:
                StartCoroutine(AttackState());
                break;
            case State.defend:
                StartCoroutine(DefendState());
                break;
            case State.run:
                StartCoroutine(RunState());
                break;
            case State.berrypick:
                StartCoroutine(BerryState());
                break;
        }
    }


    private IEnumerator AttackState()
    {
        Debug.Log("Attack: Enter");

        aiMovement.WaypointCreate();

        while (currentState == State.attack)
        {
            aiMovement.AIMove(aiMovement.player);
            if (Vector2.Distance(transform.position, aiMovement.player.position) > chaseDist)
            {
                
                currentState = State.berrypick;
            }
            yield return null;
        }
        aiMovement.ChaseEnd();
        Debug.Log("Attack: Exit");
        NextState();
    }

    private IEnumerator DefendState()
    {
        Debug.Log("Defend: Enter");
        while (currentState == State.defend)
        {
            Debug.Log("Defending");
            yield return null;
        }
        Debug.Log("Defend: Exit");
        NextState();
    }

    private IEnumerator RunState()
    {
        Debug.Log("Run: Enter");
        while (currentState == State.run)
        {
            Debug.Log("Running");
            yield return null;
        }
        Debug.Log("Run: Exit");
        NextState();
    }

    private IEnumerator BerryState()
    {
        Debug.Log("Berry: Enter");

        while (currentState == State.berrypick)
        {
            aiMovement.AIMove(aiMovement.waypoints[aiMovement.waypointIndex]);
            aiMovement.WaypointUpdate();

            if (Vector2.Distance(transform.position, aiMovement.player.position) < chaseDist)
            {
                currentState = State.attack;
            }
            yield return null;
        }
        Debug.Log("Berry: Exit");
        NextState();
    }
}