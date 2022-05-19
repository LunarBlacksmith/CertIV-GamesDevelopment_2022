using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent_p, List<Transform> context_p, Flock flock_p)
    {
        if (context_p.Count == 0)
        { return agent_p.transform.up; }

        Vector2 alignmentMove = Vector2.zero;
        int count = 0;
        foreach (Transform item in context_p)
        {
            alignmentMove += (Vector2)item.transform.up;
            count++;
        }
        if (count != 0)
        { alignmentMove /= count; }

        return alignmentMove;
    }
}
