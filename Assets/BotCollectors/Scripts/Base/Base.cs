using UnityEngine;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private WorkerPool _workers;
    [SerializeField] private Storage _storage;

    private Scunner _scun;
    private List<ResourceItem> _pendingResources = new List<ResourceItem>();
    private float _workersAmount = 3;

    private void Awake()
    {
        _scun = GetComponent<Scunner>();
        _workers = GetComponent<WorkerPool>();
    }

    private void OnEnable()
    {
        _workers.WorkerActivated += SubscribeWorker;
        _workers.WorkerDeactivated += UnsubscribeWorker;

        if (_worker != null)
        {
            _worker.ResourceDelivered += HandleResourceDelivered;
        }
    }

    private void OnDisable()
    {
        if (_worker != null)
        {
            _worker.ResourceDelivered -= HandleResourceDelivered;
        }
    }

    private void Start()
    {
        GetWorkers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExecuteScun();
        }

        TryAssignPendingTasks();
    }

    private void ExecuteScun()
    {
        List<ResourceItem> found = _scun.Research(transform.position);

        foreach (var resources in found)
        {
            resources.IsTargeted = true;
            _pendingResources.Add(resources);
        }
    }

    private void TryAssignPendingTasks()
    {
        if (_pendingResources.Count == 0) return;

        var workers = _workers.ActiveWorkers;

        foreach (var worker in workers)
        {
            if (worker.IsBusy == false && _pendingResources.Count > 0)
            {
                ResourceItem target = _pendingResources[0];
                _pendingResources.RemoveAt(0);

                if (target != null)
                {
                    worker.Move(target.transform);
                }
            }
        }
    }

    private void GetWorkers()
    {
        for (int i = 0; i < _workersAmount; i++)
        {
            _workers.Get();
        }
    }

    private void HandleResourceDelivered(ResourceType type)
    {
        _storage.AcceptResource(type);
    }

    private void SubscribeWorker(Worker worker)
    {
        worker.ResourceDelivered += HandleResourceDelivered;
    }

    private void UnsubscribeWorker(Worker worker)
    {
        worker.ResourceDelivered -= HandleResourceDelivered;
    }
}
