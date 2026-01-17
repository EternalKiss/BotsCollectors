using UnityEngine;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    [SerializeField] private Worker _worker;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Storage _storage;

    private ResourceRegistry _resourcesManager;
    private Scunner _scun;

    private List<Worker> _activeWorkers = new List<Worker>();
    private float _workersAmount = 3;

    private void Awake()
    {
        _resourcesManager = GetComponent<ResourceRegistry>();
        _scun = GetComponent<Scunner>();
    }

    private void Start()
    {
        GetWorkers();
    }

    private void Update()
    {
        TryAssignPendingTasks();
    }

    private void TryAssignPendingTasks()
    {
        if (_resourcesManager.FreeResources.Count == 0) return;

        foreach (var worker in _activeWorkers)
        {
            if (worker.IsBusy == false && _resourcesManager.FreeResources.Count > 0)
            {
                ResourceItem target = _resourcesManager.FreeResources[0];

                _resourcesManager.SetBusy(target);

                worker.Move(target.transform);
            }
        }
    }

    public void Scun()
    {
        _scun?.Research(transform.position);
    }

    private void GetWorkers()
    {
        for (int i = 0; i < _workersAmount; i++)
        {
            Worker newWorker = Instantiate(_worker, _spawnPoint.position, Quaternion.identity);
            _activeWorkers.Add(newWorker);

            newWorker.ResourceDelivered += HandleResourceDelivered;
        }
    }

    private void HandleResourceDelivered(ResourceItem resource)
    {
        _storage.AcceptResource(resource.Type);
        _resourcesManager.RemoveDeliveredResource(resource);
        Destroy(resource.gameObject);
    }

    private void OnDestroy()
    {
        foreach (var worker in _activeWorkers)
        {
            if (worker != null)
            {
                worker.ResourceDelivered -= HandleResourceDelivered;
            }
        }
    }
}
