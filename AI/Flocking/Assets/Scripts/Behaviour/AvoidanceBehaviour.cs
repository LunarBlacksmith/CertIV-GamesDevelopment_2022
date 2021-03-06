using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent_p, List<Transform> context_p, Flock flock_p)
    {
        // if there are no neighbours
        if (context_p.Count == 0)
        // return 0
        { return Vector2.zero; }

        Vector2 avoidanceMove = Vector2.zero;
        List<Transform> filteredContext = filter == null ? context_p : filter.Filter(agent_p, context_p);

        int count = 0;
        foreach (Transform item in filteredContext)
        {
            if (Vector2.SqrMagnitude(item.position-agent_p.transform.position) <= flock_p.SquareAvoidanceRadius)
            {
              // add all positions together
              avoidanceMove += (Vector2)(agent_p.transform.position - item.position); 
              count++;
            }
        }
        if (count != 0)
        { avoidanceMove /= count; }  // the average position

        return avoidanceMove;
    }
}
