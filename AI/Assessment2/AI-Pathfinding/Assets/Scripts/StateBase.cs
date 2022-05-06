using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase : MonoBehaviour
{
    public AIMovement aiMovement;
    public float chaseDist, safeDist;

    protected virtual void Start()
    {
        aiMovement = GetComponent<AIMovement>();
    }
}
