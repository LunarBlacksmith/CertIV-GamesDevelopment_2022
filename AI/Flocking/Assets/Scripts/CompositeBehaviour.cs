using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    [System.Serializable]
    public struct BehaviourGroup
    {
        public FlockBehaviour behaviour;
        public float weights;
    }

    public BehaviourGroup[] behaviours;

    public override Vector2 CalculateMove(FlockAgent agent_p, List<Transform> context_p, Flock flock_p)
    {
        Vector2 move = Vector2.zero;

        foreach (BehaviourGroup b in behaviours)
        {
            Vector2 partialMove = b.behaviour.CalculateMove(agent_p, context_p, flock_p) * b.weights;

            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > b.weights * b.weights)
                { partialMove = partialMove.normalized * b.weights; }
            }
            move += partialMove;
        }

        return move;
    }
}
