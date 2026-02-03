using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, IFlagTarget
{
    [SerializeField] private Worker _worker;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Storage _storage;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private Scanner _scan;
    [SerializeField] private ResourceRegistry _resourcesManager;
    [SerializeField] private CounterUI _uiCounter;

    private Flag _currentFlag;

    private List<Worker> _activeWorkers = new List<Worker>();
    private float _startWorkersAmount = 3;

    private bool _canGetWorkers = true;

    public event Action<Transform, Worker> BuildingBase;

    private void Start()
    {
        if (_canGetWorkers)
        {
            GetWorkersOnStart();
        }

        CounterUI uiInstance = Instantiate(_uiCounter);

        uiInstance.Initialize(_storage);

        uiInstance.SetPosition(this.transform.position, new Vector3(0, 11f, 0));
    }

    private void Update()
    {
        TryAssignPendingTasks();
    }

    private void OnEnable()
    {
        _storage.ReachedResourcesForWorker += NewWorker;
        _storage.ReachedResourcesForBase += SendWorkerBuildingBase;
    }

    private void OnDisable()
    {
        _storage.ReachedResourcesForWorker -= NewWorker;
        _storage.ReachedResourcesForBase -= SendWorkerBuildingBase;
    }

    public void SetStorage(Storage storage)
    {
        _storage = storage;
    }

    public void Scan()
    {
        _scan?.Research(transform.position);
    }

    public void SetFlag(Vector3 position)
    {
        if (_currentFlag == null)
        {
            _currentFlag = Instantiate(_flagPrefab);
        }

        _currentFlag.transform.position = position;
    }

    public Vector3 GetDropOffPoint()
    {
        return _storage != null ? _storage.transform.position : transform.position;
    }

    public void InitializeFromBuilder(Scanner scanner, bool canSpawnWorkers)
    {
        _canGetWorkers = canSpawnWorkers;
        _scan = scanner;

        _storage.ReachedResourcesForWorker += NewWorker;
        _storage.ReachedResourcesForBase += SendWorkerBuildingBase;

        _resourcesManager = _scan.GetComponent<ResourceRegistry>();
    }

    public void AddWorkerManually(Worker worker)
    {
        if (!_activeWorkers.Contains(worker))
        {
            _activeWorkers.Add(worker);
            worker.ResourceDelivered += HandleResourceDelivered;
            worker.FlagReached += WorkerArrived;
        }
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

    private void SendWorkerBuildingBase()
    {
        if (_currentFlag == null) return;
        
        if(_activeWorkers.Count <= 1) return;

        Worker freeWorker = _activeWorkers.Find(w => !w.IsBusy);

        if (freeWorker != null)
        {
            _storage.SpendResources(ResourcesForObject.BaseCost);

            freeWorker.MoveToBuildBase(_currentFlag.transform);
        }
    }

    private void WorkerArrived(Transform basePosition, Worker worker)
    {
        BuildingBase?.Invoke(basePosition, worker);
        _activeWorkers.Remove(worker);

        Destroy(_currentFlag.gameObject);
        _currentFlag = null;
    }

    private void GetWorkersOnStart()
    {
        for (int i = 0; i < _startWorkersAmount; i++)
        {
            NewWorker();
        }
    }

    private void NewWorker()
    {
        if (_currentFlag == null)
        {
            Worker newWorker = Instantiate(_worker, _spawnPoint.position, Quaternion.identity);
            _activeWorkers.Add(newWorker);

            newWorker.ResourceDelivered += HandleResourceDelivered;
            newWorker.FlagReached += WorkerArrived;
            newWorker.SetBase(this);

            _storage.SpendResources(ResourcesForObject.WorkerCost);
        }
    }

    private void HandleResourceDelivered(ResourceItem resource)
    {
        _storage.AcceptResource(resource.GetResourceType());
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
                _worker.FlagReached -= WorkerArrived;
            }
        }
    }
}
