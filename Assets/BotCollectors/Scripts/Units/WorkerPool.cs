using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkerPool : GenericPool<Worker>
{
    [SerializeField] private Transform _spawnPoint;

    private List<Worker> _activeWorkers = new List<Worker>();

    public IReadOnlyList<Worker> ActiveWorkers => _activeWorkers;
    public event Action<Worker> WorkerActivated;
    public event Action<Worker> WorkerDeactivated;

    private void Awake()
    {
        CreatePool();
    }

    protected override void ActionOnGet(Worker worker)
    {
        worker.transform.position = _spawnPoint.position;

        if (!_activeWorkers.Contains(worker))
        {
            _activeWorkers.Add(worker);
        }

        WorkerActivated?.Invoke(worker);
        base.ActionOnGet(worker);
    }

    protected override void OnRelease(Worker worker)
    {
        if (_activeWorkers.Contains(worker))
        {
            _activeWorkers.Remove(worker);
        }

        WorkerDeactivated?.Invoke(worker);

        base.OnRelease(worker);
    }
}
