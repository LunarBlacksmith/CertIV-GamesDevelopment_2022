using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    #region Public Variables
    public Transform hunter;        // bad guy of the maze
    public List<Transform> pickups; // "waypoints" to get to
    public int pickupIndex = 0;     // which pickup currently searching for

    public float speed = 4f;            // normal speed of agent
    public float fleeSpeed = 8f;        // max speed to flee at
    public float fleeDist = 10f;        // max distance to run away
    public float distanceCovered = 0f;  // current amount of distance covered while fleeing

    // minimum radius around the goal an agent needs to be before classified as 'arrived'
    public float minGoalDist = 0.25f;   
    #endregion


    public void AIMove(Transform goal)
    {
        Vector3 aiPosition = transform.position;
        Vector3 goalPosition = goal.position;

        // if we're not within minumum distance of the goal
        if (Vector2.Distance(aiPosition, goalPosition) > minGoalDist)
        {
            Vector2 dirToGoal = goalPosition - transform.position;
            dirToGoal.Normalize();
            transform.position += (Vector3)dirToGoal * speed * Time.deltaTime;
        }

        if (goal.tag == "Pickup")
        {
            // TODO: make agent wait at a "timed door" if at a timed door

            // TODO: move to the pickup using the NavMesh components
        }
        else if (goal.tag == "Seeker")
        {
            // TODO: make agent (hunter) sprint to goal using NavMesh components
        }
        else if (goal.tag == "MazeEnd")
        {
            // TODO: check if end gate is open
                // if not open, get to gate
                // wait until gate is open
                // move to goal

                // else, move to goal
        }
    }

    /// <summary>
    /// Adds pickup to agent if near enough, and assigns next pickup target.
    /// </summary>
    public void PickupUpdate()
    {
        Vector3 aiPosition = transform.position;
        Vector3 pickupPosition = pickups[pickupIndex].transform.position;

        // if we get within minimum distance of the pickup
        if (Vector2.Distance(aiPosition, pickupPosition) > minGoalDist)
        {
            // TODO: add pickup as child of Seeker and disable
                // also take pickup off list of pickups in StateMachine

            pickupIndex++;  // next pickup
        }
    }
}
