using DG.Tweening;
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

    private NavMeshAgent _agent;

    private float _collectionTime = 2.0f;

    public bool IsBusy { get; private set; } = false;

    public event Action<ResourceType> ResourceDelivered;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Transform resource)
    {
        if (!IsBusy) StartCoroutine(MiningRoutine(resource));
    }

    private IEnumerator MiningRoutine(Transform resource)
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

        yield return new WaitForSeconds(_collectionTime);

        if (resource != null)
        {
            resource.SetParent(_hands.transform);
            resource.DOLocalMove(Vector3.zero, 0.3f);
        }
        else
        {
            IsBusy = false;
            yield break;
        }

        resource.SetParent(_hands.transform);
        resource.DOLocalMove(Vector3.zero, 0.3f);

        _agent.stoppingDistance = 3.0f;

        yield return _mover.MoveToTarget(_basePosition);

        if (resource.TryGetComponent<ResourceItem>(out ResourceItem item))
        {
            ResourceDelivered?.Invoke(item.Type);
        }

        Destroy(resource.gameObject);
        IsBusy = false;
    }
}
