using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase : MonoBehaviour
{
    public AIMovement aiMovement;
    public GameObject hunter;
    public float chaseDist, safeDist;

    protected virtual void Start()
    {
        aiMovement = GetComponent<AIMovement>();
        hunter = FindObjectOfType<HunterStateMachine>().gameObject;
        safeDist = 5.0f;
        chaseDist = 4.0f;

        Debug.Log(hunter.name);
    }
}
