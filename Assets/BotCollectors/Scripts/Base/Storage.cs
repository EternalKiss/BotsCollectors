using System;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour, IResourceReceiver
{
    private Dictionary<ResourceType, int> _inventory = new Dictionary<ResourceType, int>();

    public event Action<ResourceType, int> ResourceCountChanged;
    public event Action ReachedResourcesForWorker;
    public event Action ReachedResourcesForBase;

    public void AcceptResource(ResourceType type, int amount = 1)
    {
        if (!_inventory.ContainsKey(type))
        {
            _inventory[type] = 0;
        }

        _inventory[type] += amount;
        ResourceCountChanged?.Invoke(type, _inventory[type]);

        ResourcesCounter();
    }

    public void SpendResources(Dictionary<ResourceType, int> cost)
    {
        if (!IsEnoughResources(cost)) return;

        foreach (var pair in cost)
        {
            _inventory[pair.Key] -= pair.Value;
            ResourceCountChanged?.Invoke(pair.Key, _inventory[pair.Key]);
        }
    }

    private void ResourcesCounter()
    {
        if (IsEnoughResources(ResourcesForObject.WorkerCost))
        {
            ReachedResourcesForWorker?.Invoke();
        }

        if (IsEnoughResources(ResourcesForObject.BaseCost))
        {
            ReachedResourcesForBase?.Invoke();
        }
    }

    private bool IsEnoughResources(Dictionary<ResourceType, int> cost)
    {
        foreach (var pair in cost)
        {
            ResourceType typeNedeed = pair.Key;
            float amountNedeed = pair.Value;

            if(!_inventory.TryGetValue(typeNedeed, out int currentAmount) || currentAmount < amountNedeed)
            {
                return false;
            }
        }

        return true;
    }
}
