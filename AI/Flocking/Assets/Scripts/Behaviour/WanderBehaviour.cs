using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Wander")]
public class WanderBehaviour : FilteredFlockBehaviour
{
    private Path _path;
    private int _currentWaypoint = 0;

    // private Vector2 _waypointDirection = Vector2.zero;

    public override Vector2 CalculateMove(FlockAgent agent_p, List<Transform> context_p, Flock flock_p)
    {
        if (_path == null)
        { FindPath(agent_p, context_p); }
        return FollowPath(agent_p);
    }

    private Vector2 FollowPath(FlockAgent agent_p)
    {
        if (_path == null) return Vector2.zero;
        Vector3 waypointDirection_p;

        if (WaypointInRadius(agent_p, _currentWaypoint, out waypointDirection_p))
        {
            _currentWaypoint++;
            if (_currentWaypoint >= _path.waypoints.Count)
            { _currentWaypoint = 0; }
            return Vector2.zero;
        }
        return waypointDirection_p.normalized;
    }

    public bool WaypointInRadius(FlockAgent agent_p, int currentWaypoint_p, out Vector3 waypointDirection_p)
    {
        waypointDirection_p = (Vector2)(_path.waypoints[currentWaypoint_p].position - agent_p.transform.position);

        if (waypointDirection_p.magnitude < _path.radius)
        { return true; }
        else
        { return false; }
    }

    private void FindPath(FlockAgent agent_p, List<Transform> context_p)
    {
        List<Transform> filteredContext = (filter == null) ? context_p : filter.Filter(agent_p, context_p);

        if (filteredContext.Count == 0)
        { return; }

        int randomPath = Random.Range(0, filteredContext.Count);
        _path = filteredContext[randomPath].GetComponentInParent<Path>();
    }
}