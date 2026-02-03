using UnityEngine;
using System.Collections;
using System;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private UserInputReader _inputReader;
    [SerializeField] private LayerMask _baseMask;
    [SerializeField] private LayerMask _groundMask;

    private IFlagTarget _selectedTarget;

    private void OnEnable() => _inputReader.Clicked += ProcessClick;
    private void OnDisable() => _inputReader.Clicked -= ProcessClick;

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
            }

            _selectedTarget = null;
        }
    }
}
