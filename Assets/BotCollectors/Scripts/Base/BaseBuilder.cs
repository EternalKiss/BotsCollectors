using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Base _mainBase;
    [SerializeField] private Storage _storage;
    [SerializeField] private Scanner _scanner;
    [SerializeField] Vector3 _offset;

    private void Awake()
    {
        _mainBase.BuildingBase += Build;
    }

    private void Build(Transform position, Worker worker)
    {
        Base newBase = Instantiate(_basePrefab, position.position + _offset, Quaternion.identity);

        Storage storage = Instantiate(_storage, position.position, Quaternion.identity);

        newBase.SetStorage(storage);

        newBase.InitializeFromBuilder(_scanner, false);
        newBase.AddWorkerManually(worker);
        worker.SetBase(newBase);
    }

    private void OnDestroy()
    {
        _mainBase.BuildingBase -= Build;
    }
}
