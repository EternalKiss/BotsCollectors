using System.Collections.Generic;
using UnityEngine;

public class CounterUI : MonoBehaviour
{
    [SerializeField] private Storage _storage;

    private Dictionary<ResourceType, ResourcesRowUI> _rows = new Dictionary<ResourceType, ResourcesRowUI>();

    private void Awake()
    {
        var childRows = GetComponentsInChildren<ResourcesRowUI>();

        foreach (var row in childRows)
        {
            _rows[row.Type] = row;
        }
    }

    private void OnEnable()
    {
        _storage.ResourceCountChanged += OnResourceChanged;
    }

    private void OnDisable()
    {
        _storage.ResourceCountChanged -= OnResourceChanged;
    }

    private void OnResourceChanged(ResourceType type, int totalAmount)
    {
        if (_rows.TryGetValue(type, out var row))
        {
            row.UpdateText(totalAmount);
        }
    }
}
