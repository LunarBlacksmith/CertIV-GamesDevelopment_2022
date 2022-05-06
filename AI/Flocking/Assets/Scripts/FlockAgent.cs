using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    // Flock agentsFlock

    private Collider2D  _agentCollider;
    public  Collider2D  AgentCollider { get => _agentCollider; }    // property with lambda expression


    // void Start() => _agentCollider = GetComponent<Collider2D>();
    void Start()
    {
        _agentCollider = GetComponent<Collider2D>();

        // error checking (if null, debug.log or something)
    }

    public void Move(Vector2 velocity_p)
    {
        transform.up = velocity_p.normalized;   // rotate the AI

        // move the AI,
        // parse to Vec3 since transform.position takes Vec3
        transform.position += (Vector3)velocity_p * Time.deltaTime; 
    }
}
