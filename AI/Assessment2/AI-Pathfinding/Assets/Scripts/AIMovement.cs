using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    public Transform hunter;        // bad guy of the maze
    public List<Transform> pickups; // "waypoints" to get to
    public int pickupIndex = 0;     // which pickup currently searching for

    public float speed = 4f;        // normal speed of agent
    public float fleeDist = 10f;    // max distance to run away
    public float fleeSpeed = 8f;    // max speed to flee at
    public float distanceCovered = 0f;  // current amount of distance covered while fleeing

    public float minGoalDist = 0.1f;

    private bool _chasing = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WaypointUpdate()
    {
        Vector3 aiPosition = transform.position;
        Vector3 pickupPosition = pickups[pickupIndex].transform.position;

        // if we reach the pickup
        if (aiPosition == pickupPosition)
        {
            // TODO: add pickup as child of Seeker and disable
                // also take pickup off list of pickups in StateMachine

            pickupIndex++;  // next pickup
        }
    }
}
