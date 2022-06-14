using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehaviour : ScriptableObject
{
    public abstract Vector2 CalculateMove(
        FlockAgent agent_p, 
        List<Transform> context_p, 
        Flock flock_p);
}
