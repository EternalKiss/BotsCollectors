using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Mover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    public IEnumerator MoveToTarget(Vector3 target)
    {
        _agent.SetDestination(target);

        yield return null;

        while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
        {
            yield return null;
        }
    }
}
