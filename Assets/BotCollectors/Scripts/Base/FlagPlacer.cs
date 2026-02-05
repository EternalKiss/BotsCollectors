using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    public Flag CreateFlag(Vector3 position)
    {
        return Instantiate(_flagPrefab, position, Quaternion.identity);
    }
}
