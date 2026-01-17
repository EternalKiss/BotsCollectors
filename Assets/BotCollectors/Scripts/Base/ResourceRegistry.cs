using System.Collections.Generic;
using UnityEngine;

public class ResourceRegistry : MonoBehaviour
{
    [SerializeField] private Scunner _scunner;

    public List<ResourceItem> FreeResources { get; private set; } = new List<ResourceItem>();
    public List<ResourceItem> BusyResources { get; private set; } = new List<ResourceItem>();

    public void AddNewResource(ResourceItem resource)
    {
        if (!FreeResources.Contains(resource) && !BusyResources.Contains(resource))
            FreeResources.Add(resource);
    }

    public void SetBusy(ResourceItem resource)
    {
        if (FreeResources.Contains(resource))
        {
            FreeResources.Remove(resource);
            BusyResources.Add(resource);

            resource.IsTargeted = true;
        }
    }

    public void RemoveDeliveredResource(ResourceItem resource)
    {
        BusyResources.Remove(resource);
    }
}
