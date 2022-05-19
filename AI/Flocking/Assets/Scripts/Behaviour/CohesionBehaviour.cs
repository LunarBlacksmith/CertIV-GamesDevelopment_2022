using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent_p, List<Transform> context_p, Flock flock_p)
    {
        // if there are no neighbours
        if (context_p.Count == 0)
        // return 0
        { return Vector2.zero; }

        Vector2 cohesionMove = Vector2.zero;

        int count = 0;
        foreach (Transform item in context_p)
        {
            //if (Vector2.SqrMagnitude(item.position-agent_p.transform.position) <= )
            //{
                cohesionMove += (Vector2)item.position; // add all positions together
                count++;
            //}
        }
        if (count != 0)
        { cohesionMove /= count; }  // the average position

        // direction from a to b = b - a
        cohesionMove -= (Vector2)agent_p.transform.position;

        return cohesionMove;
    }
}
