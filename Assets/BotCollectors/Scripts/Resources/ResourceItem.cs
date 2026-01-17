using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    [SerializeField] private ResourceType _type;

    private ResourceType[] _resourceTypes = (ResourceType[])System.Enum.GetValues(typeof(ResourceType));

    public ResourceType GetRandomType()
    {
        _type = _resourceTypes[Random.Range(0, _resourceTypes.Length)];

        return _type;
    }

    public ResourceType GetResourceType()
    {
        return _type; 
    }
}
