using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;

    private Collider2D  _agentCollider;
    public  Collider2D  AgentCollider { get => _agentCollider; }    // property with lambda expression


    // same method, but latter can have other code with error checking or whatever
    // void Start() => _agentCollider = GetComponent<Collider2D>();
    void Start()
    {
        _agentCollider = GetComponent<Collider2D>();

        // error checking (if null, debug.log or something)
    }

    public void Initialise(Flock flock_p)
    {
        agentFlock = flock_p;
    }

    public void Move(Vector2 velocity_p)
    {
        transform.up = velocity_p.normalized;   // rotate the AI

        // move the AI,
        // parse to Vec3 since transform.position takes Vec3
        transform.position += (Vector3)velocity_p * Time.deltaTime; 
    }
}
