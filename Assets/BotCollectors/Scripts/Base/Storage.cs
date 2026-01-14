using System.Collections.Generic;
using UnityEngine;
using System;

public class Storage : MonoBehaviour, IResourceReceiver
{
    private Dictionary<ResourceType, int> _inventory = new Dictionary<ResourceType, int>();

    public event Action<ResourceType, int> ResourceCountChanged;
    public void AcceptResource(ResourceType type, int amount = 1)
    {
        if (!_inventory.ContainsKey(type))
        {
            _inventory[type] = 0;
        }

        _inventory[type] += amount;
        ResourceCountChanged?.Invoke(type, _inventory[type]);
    }
}
