using UnityEngine;
using System;

public class RayCaster : MonoBehaviour
{
    [SerializeField] private UserInputReader _inputReader;
    [SerializeField] private LayerMask _baseMask;
    [SerializeField] private LayerMask _groundMask;

    private IFlagTarget _selectedTarget;

    private void OnEnable() => _inputReader.Clicked += ProcessClick;
    private void OnDisable() => _inputReader.Clicked -= ProcessClick;

    public event Action<Transform> FlagPlaced;

    private void ProcessClick(Ray ray)
    {
        if (_selectedTarget == null)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _baseMask))
            {
                if (hit.collider.TryGetComponent(out IFlagTarget target))
                {
                    _selectedTarget = target;
                }
            }
        }
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundMask))
            {
                _selectedTarget.SetFlag(hit.point);
                FlagPlaced?.Invoke(hit.transform);
            }

            _selectedTarget = null;
        }
    }
}
