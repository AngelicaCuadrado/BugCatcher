using UnityEngine;
using UnityEngine.AI;

public class TestMovement : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform target; // Drag your player here in the inspector

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true; // Let the agent handle rotation for this test
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}