using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stay In Radius")]
public class StayInRadiusBehaviour : FlockBehaviour
{
    [SerializeField] private Vector2 _center;
    [SerializeField] private float _radius = 15f;

    public override Vector2 CalculateMove(FlockAgent agent_p, List<Transform> context_p, Flock flock_p)
    {
        // direction to the center
        Vector2 centerOffset = _center - (Vector2)agent_p.transform.position;

        // if value in t is between 0 and 1, we're in the radius
        // if magnitude is higher than the radius, it could return a number bigger than 1
        float t = centerOffset.magnitude / _radius;

        // if we are between the center and 90% of the radius
        if (t < 0.9f)
        { return Vector2.zero; }

        return centerOffset * t * t;
    }
}
