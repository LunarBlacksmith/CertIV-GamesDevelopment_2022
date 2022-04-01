using System;
using UnityEngine;
using UnityEngine.AI;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();
    [SerializeField] bool _isRandomPos = false;
    [SerializeField] float _randomDistRadius = 10f;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_isRandomPos)
        {
            RandomMove();
        }
        ClickMove();
    }

    private void RandomMove()
    {
        if (m_Agent.remainingDistance < 0.1f 
            && m_Agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-_randomDistRadius, _randomDistRadius), 
                UnityEngine.Random.Range(-_randomDistRadius, _randomDistRadius));
            Vector3 newPosition = m_Agent.transform.position + randomOffset;

            m_Agent.SetDestination(newPosition);
        }
    }

    void ClickMove()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            _isRandomPos = false;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
            {
                m_Agent.destination = m_HitInfo.point;
            }
            if (m_Agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                _isRandomPos = true;
            }
        }
    }
}
