using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Miner : MonoBehaviour
{
    private float _collectionTime = 2.0f;
    private WaitForSeconds _collectionWait;

    private void Awake()
    {
        _collectionWait = new WaitForSeconds(_collectionTime);
    }

    public IEnumerator MiningRoutine(Transform resource, CarryPoint hands)
    {
        yield return _collectionWait;

        if (resource != null)
        {
            resource.SetParent(hands.transform);
            resource.DOLocalMove(Vector3.zero, 0.3f);
        }
        else
        {
            yield break;
        }

        resource.SetParent(hands.transform);
        resource.DOLocalMove(Vector3.zero, 0.3f);
    }
}
