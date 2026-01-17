using System.Collections;
using UnityEngine;

public class ResourcesPool : GenericPool<ResourceItem>
{
    [SerializeField] private ResourceType _resourcesTypes;

    private float _minXSpawn = 150f;
    private float _maxXSpawn = 200f;
    private float _minZSpawn = 150f;
    private float _maxZSpawn = 200f;
    private float _delayForSpawn = 5f;

    private void Awake()
    {
        CreatePool();
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(SpawnCoroutine());
    }

    protected override void ActionOnGet(ResourceItem resource)
    {
        resource.transform.position = GetRandomSpawnPosition();
        resource.GetRandomType();

        base.ActionOnGet(resource);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float spawnPositionX = Random.Range(_minXSpawn, _maxXSpawn);
        float spawnPositionZ = Random.Range(_minZSpawn, _maxZSpawn);

        return new Vector3(spawnPositionX, 0.5f, spawnPositionZ);
    }

    private IEnumerator SpawnCoroutine()
    {
        while(enabled)
        {
            yield return new WaitForSeconds(_delayForSpawn);
            _pool.Get();
        }
    }
}
