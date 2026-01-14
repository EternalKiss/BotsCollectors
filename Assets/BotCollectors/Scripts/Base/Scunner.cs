using System.Collections.Generic;
using UnityEngine;

public class Scunner : MonoBehaviour
{
    [SerializeField] private LayerMask _scanMask;

    private Vector3 _radius = new Vector3(100f, 20f, 100f);

    public List<ResourceItem> Research(Vector3 baseTransform)
    {
        Collider[] hitColliders = Physics.OverlapBox(baseTransform, _radius, Quaternion.identity, _scanMask);

        List<ResourceItem> freeResources = new List<ResourceItem>();

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].TryGetComponent<ResourceItem>(out ResourceItem resource))
            {
                if (resource.IsTargeted == false)
                {
                    freeResources.Add(resource);
                }
            }
        }

        return freeResources;
    }
}
