using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Base _base;
    [SerializeField] private CarryPoint _hands;
    [SerializeField] private ResourceType _carryingType;
    [SerializeField] private Miner _miner;

    private NavMeshAgent _agent;

    public bool IsBusy { get; private set; } = false;

    public event Action<ResourceItem> ResourceDelivered;
    public event Action<Transform, Worker> FlagReached;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Transform resource)
    {
        if (!IsBusy) StartCoroutine(CollectRoutine(resource));
    }

    public void MoveToBuildBase(Transform target)
    {
        if (!IsBusy) StartCoroutine(BuildRoutine(target));
    }

    private IEnumerator BuildRoutine(Transform target)
    {
        IsBusy = true;
        _agent.stoppingDistance = 0.5f;

        yield return _mover.MoveToTarget(target.position);

        FlagReached?.Invoke(target, this);

        IsBusy = false;
    }

    public void SetBase(Base newBase)
    {
        _base = newBase;
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

        _agent.stoppingDistance = 0.5f;

        yield return _mover.MoveToTarget(_base.GetDropOffPoint());

        if (resource != null && resource.TryGetComponent<ResourceItem>(out ResourceItem item))
        {
            ResourceDelivered?.Invoke(item);
        }

        IsBusy = false;
    }
}
