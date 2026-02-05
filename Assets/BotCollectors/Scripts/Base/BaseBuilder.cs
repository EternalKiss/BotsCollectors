using System.Collections.Generic;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private BaseRegistry _registry;
    [SerializeField] private Storage _storage;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Vector3 _baseOffset;
    [SerializeField] private Vector3 _storageOffset;

    private List<Base> _activeBases = new List<Base>();
    private Vector3 _mainBaseStartPosition = new Vector3(154, 5, 159);

    private void Awake()
    {
        Base firstBase = CreateBaseWithStorage(_mainBaseStartPosition);
        SetupNewBase(firstBase, true);
    }

    private void Build(Transform position, Worker worker)
    {
        Base newBase = CreateBaseWithStorage(position.position + _baseOffset);

        SetupNewBase(newBase, false);

        newBase.AddWorkerManually(worker);
        worker.SetBase(newBase);
    }

    private Base CreateBaseWithStorage(Vector3 position)
    {
        Base newBase = Instantiate(_basePrefab, position, Quaternion.identity);
        Storage newStorage = Instantiate(_storage, position + _storageOffset, Quaternion.identity);

        newBase.SetStorage(newStorage);
        return newBase;
    }

    private void SetupNewBase(Base targetBase, bool canSpawnWorkers)
    {
        targetBase.InitializeFromBuilder(_scanner, canSpawnWorkers);

        targetBase.BuildingBase += Build;

        _activeBases.Add(targetBase);
        _registry.RegisterBase(targetBase);
    }

    private void OnDestroy()
    {
        foreach (var activeBase in _activeBases)
        {
            if (activeBase != null)
                activeBase.BuildingBase -= Build;
        }
    }
}
