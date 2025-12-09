using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    [SerializeField] private float _delayToDestroy;
    private float _accumulatedTime = 0;


    private void Update()
    {
        _accumulatedTime += Time.deltaTime;
        if (_accumulatedTime >= _delayToDestroy)
            Destroy(gameObject);
    }
}