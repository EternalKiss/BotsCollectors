using System.Collections.Generic;
using UnityEngine;

public class ResourceRegistry : MonoBehaviour
{
    private readonly List<ResourceItem> _freeResources = new List<ResourceItem>();
    private readonly List<ResourceItem> _busyResources = new List<ResourceItem>();

    public IReadOnlyList<ResourceItem> FreeResources => _freeResources;
    public IReadOnlyList<ResourceItem> BusyResources => _busyResources;

    public void AddNewResource(ResourceItem resource)
    {
        if (!_freeResources.Contains(resource) && !_busyResources.Contains(resource))
        {
            _freeResources.Add(resource);
        }
    }

    public void SetBusy(ResourceItem resource)
    {
        if (_freeResources.Remove(resource))
        {
            _busyResources.Add(resource);
        }
    }

    public void RemoveDeliveredResource(ResourceItem resource)
    {
        _busyResources.Remove(resource);
    }
}
