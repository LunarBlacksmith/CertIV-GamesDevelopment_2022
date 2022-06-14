using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Different Flock Filter")]
public class DifferentFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent_p, List<Transform> original_p)
    {
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original_p)
        {
            FlockAgent itemAgent = item.GetComponent<FlockAgent>();
            if (itemAgent != null)
            {
                if (itemAgent.AgentFlock != agent_p.AgentFlock)
                { filtered.Add(item); }
            }
        }
        return filtered;
    }
}
