using System.Collections.Generic;
using UnityEngine;

public class CounterUI : MonoBehaviour
{
    private Storage _storage;
    private Dictionary<ResourceType, ResourcesRowUI> _rows = new Dictionary<ResourceType, ResourcesRowUI>();

    public void Initialize(Storage storage)
    {
        _storage = storage;
        _storage.ResourceCountChanged += OnResourceChanged;

        var childRows = GetComponentsInChildren<ResourcesRowUI>();

        foreach (var row in childRows)
        {
            _rows[row.Type] = row;
        }
    }

    public void SetPosition(Vector3 position, Vector3 offset)
    {
        transform.position = position + offset;
    }


    private void OnResourceChanged(ResourceType type, int totalAmount)
    {
        if (_rows.TryGetValue(type, out var row))
        {
            row.UpdateText(totalAmount);
        }
    }

    private void OnDestroy()
    {
        if (_storage != null)
            _storage.ResourceCountChanged -= OnResourceChanged;
    }
}
