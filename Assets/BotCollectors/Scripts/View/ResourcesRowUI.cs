using UnityEngine;
using TMPro;

public class ResourcesRowUI : MonoBehaviour
{
    [SerializeField] private ResourceType _type;
    [SerializeField] private TextMeshProUGUI _countText;

    public ResourceType Type => _type;

    public void UpdateText(int count)
    {
        _countText.text = $"{_type}: {count}";
    }
}
