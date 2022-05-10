using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : MonoBehaviour
{
    #region Public Variables
    public NavMeshAgent meshAgent;      // the AI agent that will be utilising the NavMeshSurfaces, etcs
    public List<Transform> pickups;     // "waypoints" to get to
    public int pickupIndex = 0;         // which pickup currently searching for

    //public float speed = 4f;            // normal speed of agent
    public float fleeSpeed = 8f;        // max speed to flee at
    public float fleeDist = 10f;        // max distance to run away
    public float distanceCovered = 0f;  // current amount of distance covered while fleeing

    // minimum radius around the goal an agent needs to be before classified as 'arrived'
    public float minGoalDist = 0.2f;
    #endregion

    private float _mapXBounds = 20f, _mapZBounds = 40f, _mapYBounds = 5f;

    public void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
    }

    public void AIMove(Transform goal)
    {
        Vector3 aiPosition = transform.position;

        // setting the Destination of the agent will allow it to use the NavMesh to move
        // towards the position given
        meshAgent.SetDestination(goal.position);

        // if we can't reach the goal
        if (meshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        { 
            Debug.Log("Can't reach the goal."); // send a debug message
            meshAgent.destination = aiPosition; // set the destination to the agent's own position
        }
    }

    /// <summary>
    /// Adds pickup to agent if near enough, and assigns next pickup target.
    /// </summary>
    public void PickupUpdate()
    {
        if (pickups.Count == 0)             // if there are no pickups
        { return; }                         // exit the method
        else if (pickups.Count == 1)        // if there is 1 pickup left
        { 
            pickupIndex = 0;                // assign the index to that pickup

            Vector3 aiPosition = transform.position;
            Vector3 currPickupPos = pickups[pickupIndex].transform.position;

            // if we get within minimum distance of the pickup
            if (Vector2.Distance(aiPosition, currPickupPos) <= minGoalDist)
            {
                // add pickup as child of Seeker and disable
                pickups[pickupIndex].transform.parent = gameObject.transform;

                // remove the last pickup from the list
                pickups.RemoveAt(pickupIndex);
            }
        }                
        else                                // else..
        {
            Debug.Log("We're in PickupUpdate() 'else'.");

            Vector3 aiPosition = transform.position;
            Vector3 currPickupPos = pickups[pickupIndex].transform.position;

            pickupIndex = 0;

            // for every pickup in the list
            for (int i = 1; i < pickups.Count; i++)
            {
                Debug.Log($"meshAgent path corners length: {meshAgent.path.corners.Length}");

                Vector3[] corners1 = meshAgent.path.corners;
                meshAgent.SetDestination(pickups[i].transform.position);
                Vector3[] corners2 = meshAgent.path.corners;

                if (corners1.Length == 1 && Vector3.Distance(aiPosition, currPickupPos) > minGoalDist)
                { pickupIndex = i; }
                // if the path to the current pickup is longer than the alternative path
                else if (corners1.Length > corners2.Length)
                // use the alternative pickup's path
                { pickupIndex = i; }
                // if the 2 pickups' paths are equidistant from the seeker
                else if (corners1.Length == corners2.Length)
                {
                    pickupIndex = 0;
                    continue; 
                }   
            }

            // if we get within minimum distance of the current, closest pickup
            if (Vector2.Distance(aiPosition, currPickupPos) <= minGoalDist)
            {
                // add pickup as child of Seeker
                pickups[pickupIndex].transform.parent = gameObject.transform;

                // also take the pickup off list of pickups
                pickups.RemoveAt(pickupIndex);
            }

        }
    }

    /// <summary>
    /// Checks if the caller can set their destination successfully to the provided goal. Throws an Argument Null Exception if the caller is not a NavMeshAgent.
    /// </summary>
    /// <param name="goal"></param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public bool CanMoveTo(Transform goal)
    {
        if (gameObject.GetComponent<NavMeshAgent>() == null)
        {
            Debug.Log("The caller of the CanMoveTo() method is not a NavMeshAgent and therefore can not traverse a NavMeshSurface");
            throw new System.ArgumentNullException();
        }

        // avoiding interfering with the caller's actual set destination by creating a temp copy
        NavMeshAgent tempAgent = gameObject.GetComponent<NavMeshAgent>();

        // return the resulting boolean when we check if the caller agent can 
        // successfully traverse the NavMesh to reach the goal
        return tempAgent.SetDestination(goal.position);
    }

    /// <summary>
    /// Using the agent's Transform and the Transform passed, generates a random position away from the passed Transform's position and sets the agent's destination to that position.
    /// </summary>
    /// <param name="fleeingFromObject_p"></param>
    public void FleeMove(Transform fleeingFromObject_p)
    {
        bool _dirNotAwayFromEnemy = true;
        float _agentX = transform.position.x, _agentZ = transform.position.z;
        float _hunterX = fleeingFromObject_p.position.x, _hunterZ = fleeingFromObject_p.position.z;
        Vector3 newPosition; // declare variable for random position outside of do/while scope

        do
        {
            // generate new random position within the map's bounds
            newPosition = GetRandomGoalInBounds();

            // if the random position is between the hunter and seeker then it is 'towards' the
            // enemy, in which case we need to generate a new position that is away from the enemy
            if (_hunterX >= _agentX)
            {
                if (newPosition.x >= _agentX)
                { _dirNotAwayFromEnemy = true; }
                else
                {_dirNotAwayFromEnemy = false; }
            }
            else if (_hunterX <= _agentX)
            {
                if (newPosition.x <= _agentX)
                { _dirNotAwayFromEnemy = true; }
                else
                { _dirNotAwayFromEnemy = false; }
            }
            else if (_hunterZ >= _agentZ)
            {
                if (newPosition.z >= _agentZ)
                { _dirNotAwayFromEnemy = true; }
                else
                { _dirNotAwayFromEnemy = false; }
            }
            else if (_hunterZ <= _agentZ)
            {
                if (newPosition.z <= _agentZ)
                { _dirNotAwayFromEnemy = true; }
                else
                { _dirNotAwayFromEnemy = false; }
            }

        } while (_dirNotAwayFromEnemy);

        // once the new position is away from the enemy, set the caller NavMeshAgent's destination to it
        meshAgent.SetDestination(newPosition);
    }

    /// <summary>
    /// Gets the private values of the Map's x, y, z coordinates and generates a random position between their positive and negative values, with y being generated between -0.5 and the positive variable.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomGoalInBounds()
    {
        Vector3 randomGoal = new Vector3(
                Random.Range(-_mapXBounds,  _mapXBounds),
                Random.Range(-0.5f,         _mapYBounds),
                Random.Range(-_mapZBounds,  _mapZBounds)
                );
        return randomGoal;
    }
}
