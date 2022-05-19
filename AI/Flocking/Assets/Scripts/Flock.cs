using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public List<FlockAgent> agents; // = new List<FlockAgent>();

    public FlockBehaviour behaviour;

    [Range(10, 500)]
    public int  startingCount = 250;
    public float agentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighbourRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float _squareMaxSpeed;
    float _squareNeighbourRadius;
    float _squareAviodanceRadius;

    public float SquareAvoidanceRadius { get { return _squareAviodanceRadius; } }

    private void Start()
    {
        // square up the variables
        // spawn a bunch of agents

        _squareMaxSpeed         = maxSpeed * maxSpeed;
        _squareNeighbourRadius  = neighbourRadius * neighbourRadius;
        _squareAviodanceRadius  = _squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(  // create new gameobject
                agentPrefab,    // this is the prefab
                Random.insideUnitCircle * startingCount * agentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0, 360f)),
                transform
                );
            newAgent.name = "Agent-" + i;
            newAgent.Initialise(this);
            agents.Add(newAgent);
        }
    }

    private void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            // FOR TESTING
            // changing colour, the more neighbours the more red the agent is
            agent.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            // using behaviour to calculate the direction the agent should move in
            Vector2 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor; // speed

            // if speed is greater than max speed
            if (move.sqrMagnitude > _squareMaxSpeed)
            { move = move.normalized * maxSpeed; }  // bring speed back to max speed

            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            { context.Add(c.transform); }
        }
        return context;
    }
}
