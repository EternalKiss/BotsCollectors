using System.Collections.Generic;
using UnityEngine;

public class BaseRegistry : MonoBehaviour
{
    private List<Base> _allBases = new List<Base>();

    public void RegisterBase(Base newBase) => _allBases.Add(newBase);

    public void UnregisterBase(Base oldBase) => _allBases.Remove(oldBase);

    public void ScanAll()
    {
        foreach (var b in _allBases)
        {
            if (b != null) b.Scan();
        }
    }
}
