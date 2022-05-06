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
    public List<GameObject> pickups;

    protected override void Start()
    {
        base.Start();
        NextState();
    }

    private void NextState()
    {
        {
            switch (currentState)
            {
                case State.Flee:
                    {
                        StartCoroutine(FleeState());
                        break;
                    }
                case State.Search:
                    {
                        StartCoroutine(SearchState());
                        break;
                    }
                case State.Escape:
                    {
                        StartCoroutine(EscapeState());
                        break;
                    }
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
            // TODO: change this to check if the hunter is within safeDist of Seekers
            bool _hunterInSight = Vector2.Distance(
                transform.position, 
                aiMovement.hunter.position) <= safeDist;

            // if we're fleeing and we haven't run the full Flee Distance away
            while (aiMovement.distanceCovered != aiMovement.fleeDist)
            {
                switch (_hunterInSight)
                {
                    case false:
                        {
                            // TODO: keep running 1.5 more units
                            currentState = State.Search;    // switch back to Search state
                            break;
                        }
                    case true:
                        {
                            if (aiMovement.distanceCovered != aiMovement.fleeDist)
                            {
                                // TODO: keep running in a random direction that isn't towards the hunter
                            }

                            break;
                        }
                }
            }
            if (aiMovement.distanceCovered == aiMovement.fleeDist)
            {
                yield return new WaitForSeconds(3f);// stop for a few seconds
                currentState = State.Search;        // switch to Search state
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
                aiMovement.hunter.position) <= safeDist;
            
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
