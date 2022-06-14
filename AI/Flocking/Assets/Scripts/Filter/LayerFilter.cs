using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Layer")]
public class LayerFilter : ContextFilter
{
    public LayerMask mask;

    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach (Transform item in original)
        {
            //1     =  000000000000000001
            //1<<6  =  000000000001000000

            //mask  =  000000000001000001
            //mask  =  000101010001010010

            //   000000000001000000 |
            //   000000000001000001

            //   000000000001000001

            // if any layer of the boid is the same as the behaviour's layer
            if (0 != (mask & (1 << item.gameObject.layer)))
            { filtered.Add(item); }
        }
        return filtered;
    }
}