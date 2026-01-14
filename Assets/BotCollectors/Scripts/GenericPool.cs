using UnityEngine;
using UnityEngine.Pool;

public class GenericPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] protected int _poolCapacity;
    [SerializeField] protected int _poolMaxSize;

    protected ObjectPool<T> _pool;

    protected virtual void CreatePool()
    {
        _pool = new ObjectPool<T>(
            createFunc: CreatePrefab,
            actionOnGet: ActionOnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: Destroy,
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    protected virtual T CreatePrefab()
    {
        
        T prefab = Instantiate(_prefab);

        return prefab;
    }

    protected virtual void ActionOnGet(T prefab)
    {
        prefab.gameObject.SetActive(true);
    }

    protected virtual void OnRelease(T prefab)
    {
        prefab.gameObject.SetActive(false);
    }

    public virtual void Get()
    {
        _pool.Get();
    }
}
