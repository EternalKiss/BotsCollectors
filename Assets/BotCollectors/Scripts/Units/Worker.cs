using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Vector3 _basePosition;
    [SerializeField] private CarryPoint _hands;
    [SerializeField] private ResourceType _carryingType;
    [SerializeField] private Miner _miner;

    private NavMeshAgent _agent;

    public bool IsBusy { get; private set; } = false;

    public event Action<ResourceItem> ResourceDelivered;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Transform resource)
    {
        if (!IsBusy) StartCoroutine(CollectRoutine(resource));
    }

    private IEnumerator CollectRoutine(Transform resource)
    {
        if (resource == null) yield break;

        IsBusy = true;
        _agent.stoppingDistance = 1.5f;

        yield return _mover.MoveToTarget(resource.position);

        if (resource == null)
        {
            IsBusy = false;
            yield break;
        }

        yield return _miner.MiningRoutine(resource, _hands);

        _agent.stoppingDistance = 3.0f;

        yield return _mover.MoveToTarget(_basePosition);

        if (resource != null && resource.TryGetComponent<ResourceItem>(out ResourceItem item))
        {
            ResourceDelivered?.Invoke(item);
        }

        IsBusy = false;
    }
}
