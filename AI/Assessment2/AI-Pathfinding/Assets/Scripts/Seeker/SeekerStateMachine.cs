using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerStateMachine : StateBase
{
    public enum State
    {
        Flee,
        Search,
        Escape
    }

    public State currentState;
    public Transform tHunter;
    public List<GameObject> pickups;

    protected override void Start()
    {
        base.Start();
        tHunter = hunter.transform;
        currentState = State.Search;
        NextState();
    }

    private void NextState()
    {
        {
            switch (currentState)
            {
                case State.Flee:
                    { StartCoroutine(FleeState()); break; }
                case State.Search:
                    { StartCoroutine(SearchState()); break; }
                case State.Escape:
                    { StartCoroutine(EscapeState()); break; }
                default:
                    { 
                        Debug.LogException(new ArgumentOutOfRangeException());
                        break;
                    }
            }
        }
    }

    private IEnumerator FleeState()
    {
        Debug.Log("Flee State: Enter");
        while (currentState == State.Flee)
        {
            // making a new Vector to store only the x and z positions (2D directions) of the seeker and the hunter
            Vector2 _agentXZ = new Vector2(transform.position.x, transform.position.z);
            Vector2 _hunterXZ = new Vector2(tHunter.position.x, tHunter.position.z);

            // the hunter IS in sight if the distance between either the
            // X position of the Hunter is less than or equal to the safe distance
            // OR the Z position of the Hunter is less than or equal to the safe distance,
            // AND the Y position of the Hunter is within a 0.5 unit radius of the Seeker
            bool _hunterInSight = 
                Vector2.Distance(_agentXZ, _hunterXZ) <= safeDist &&
                (tHunter.position.y <= transform.position.y + 0.5f && tHunter.position.y >= transform.position.y - 0.5f);

            // if the hunter is within min distance (safe) and a path to them can be made successfully
            if (_hunterInSight && aiMovement.CanMoveTo(tHunter))
            // run in a random direction that isn't towards the hunter
            { aiMovement.FleeMove(tHunter); }
            else
            {
                // keep running in a random direction that isn't towards the hunter
                // for a couple seconds
                aiMovement.FleeMove(tHunter);
                yield return new WaitForSeconds(2f);
                currentState = State.Search;    // switch back to Search state
            }

            yield return null;
        }
        Debug.Log("Flee State: Exit");
        NextState();
    }

    private IEnumerator SearchState()
    {
        Debug.Log("Search State: Enter");

        while (currentState == State.Search)
        {
            // if there are no pickups left
            if (pickups.Count <= 0)
            { currentState = State.Escape; }

            // TODO: make the move goal the position of the pickup
            aiMovement.AIMove(pickups[aiMovement.pickupIndex].transform);

            // check if the agent is at the pickup and add it if they are
            aiMovement.PickupUpdate();

            // TODO: change this to check if the hunter is within safeDist of Seekers
            bool _hunterInSight = Vector2.Distance(
                transform.position, 
                aiMovement.agentTransform.position) <= safeDist;
            
            if (_hunterInSight)             // if we can see the hunter
            { currentState = State.Flee; }  // we are no longer searching, we are fleeing

            yield return null;
        }
        Debug.Log("Search State: Exit");
        NextState();
    }

    private IEnumerator EscapeState()
    {
        // TODO: get to the end of the maze

        // if the hunter is within sight, change state to Flee

        yield return null;
    }
}
