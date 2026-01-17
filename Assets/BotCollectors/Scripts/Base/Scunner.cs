using System.Collections.Generic;
using UnityEngine;

public class Scunner : MonoBehaviour
{
    [SerializeField] private ResourceRegistry _resourcesManager;
    [SerializeField] private LayerMask _scanMask;

    private Vector3 _radius = new Vector3(100f, 20f, 100f);
    private readonly Collider[] _results = new Collider[50];

    public List<ResourceItem> Research(Vector3 baseTransform)
    {
        int amount = Physics.OverlapBoxNonAlloc(baseTransform, _radius, _results, Quaternion.identity, _scanMask);

        List<ResourceItem> freeResources = new List<ResourceItem>();

        for (int i = 0; i < amount; i++)
        {
            if (_results[i].TryGetComponent<ResourceItem>(out ResourceItem resource))
            {
                if (resource.IsTargeted == false)
                {
                    freeResources.Add(resource);
                    _resourcesManager.AddNewResource(resource);
                }
            }

            _results[i] = null;
        }

        return freeResources;
    }
}
