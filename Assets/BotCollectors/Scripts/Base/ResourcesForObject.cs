using System.Collections.Generic;

public struct ResourcesForObject
{
    public static readonly Dictionary<ResourceType, int> WorkerCost = new()
    {
        { ResourceType.Gold, 1 },
        { ResourceType.Wood, 1 },
        { ResourceType.Ore, 1 }
    };

    public static readonly Dictionary<ResourceType, int> BaseCost = new()
    {
        { ResourceType.Gold, 3 },
        { ResourceType.Wood, 1 },
        { ResourceType.Ore, 1 }
    };
}
