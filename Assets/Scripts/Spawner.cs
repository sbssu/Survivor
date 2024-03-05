using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : RootBehaiour
{
    [Header("Object")]
    [SerializeField] GameObject prefab;

    [Header("Position")]
    [SerializeField] Transform pivot;       // 기준점.
    [SerializeField] float radius;          // 반지름.
    [SerializeField] float spawnRate;       // 스폰 주기.

    float rateTime;
    private void Start()
    {
        rateTime = spawnRate;
    }

    void Update()
    {
        if (isPauseObject)
            return;

        rateTime -= Time.deltaTime;
        if(rateTime <= 0.0f)
        {
            // 길이가 1인 벡터를 랜덤 방향으로 만들어 radius만큼 늘인다.
            Vector2 position = (Vector2)pivot.position + Random.insideUnitCircle.normalized * radius;
            GameObject spawnObject = Instantiate(prefab);
            spawnObject.transform.position = position;

            rateTime = spawnRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;
        if (pivot != null)
            position = pivot.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(position, radius);
    }
}
