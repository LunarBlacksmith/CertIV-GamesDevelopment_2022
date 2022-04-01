using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public Transform player;
    public List<Transform> waypoints;
    //public Transform[] waypoints;
    public int waypointIndex = 0;

    public float speed = 1.5f;
    public float minGoalDist = 0.1f;

    private bool chasing = false;

    /*
    public void WaypointCreate()
    {
        GameObject wpoint = Instantiate(waypointPrefab, transform.position, Quaternion.identity);
        waypoints.Add(wpoint.transform);
    }*/

    /// <summary>
    /// I can't wrap my head around this crap. It's 12:30 AM.. How to make people flee?
    /// </summary>
    /// <param name="goal"></param>
    public void AIMove(Transform goal)
    {
        Vector2 AIPosition = transform.position;


        //if we're not near the goal
        if (Vector2.Distance(AIPosition, goal.position) > minGoalDist)
        {
            Vector2 dirToGoal = goal.position - transform.position;
            dirToGoal.Normalize();
            transform.position += (Vector3)dirToGoal * speed * Time.deltaTime;
        }
        else if(goal != player)
        {
           transform.position = goal.position;
        }

        if (chasing == false && goal == player)
        {
            chasing = true;
        }
    }

    public void ChaseEnd()
    {
        int closePoint = 0;
        float dist = float.PositiveInfinity;
        float tempDist;
        for (int i = 0; i < waypoints.Count; i++)
        {
            tempDist = Vector2.Distance(transform.position, waypoints[i].position);
            //check if the distance to point i is smaller than point i-1
            if (tempDist < dist)
            {
                dist = tempDist;
                closePoint = i;
            }
        }
        //then move to the nearest point
        waypointIndex = closePoint;
    }

    public void WaypointUpdate()
    {
        Vector2 AIPosition = transform.position;


        //once we reach the goal
        if (transform.position == waypoints[waypointIndex].position) //(Vector2.Distance(AIPosition, waypoints[waypointIndex].transform.position) < minGoalDist)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = 0;
            }
        }
    }
}


/*          METHOD 1
if (transform.position.x < position0.transform.position.x)
{
    transform.position += Vector3.right * 1 * Time.deltaTime;
}
else
{
    transform.position -= Vector3.right * 1 * Time.deltaTime;
}

if (transform.position.y < position0.transform.position.y)
{
    transform.position += Vector3.up * 1 * Time.deltaTime;
}
else
{
    transform.position -= Vector3.up * 1 * Time.deltaTime;
}

            METHOD 2


AIPosition.x = AIPosition.x + (1 * Time.deltaTime);

transform.position = AIPosition;
*/

//transform.position = Vector2.MoveTowards(transform.position, position0.transform.position, 1 * Time.deltaTime);