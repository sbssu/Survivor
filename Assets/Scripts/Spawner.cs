using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : RootBehaiour
{
    [Header("Object")]
    [SerializeField] GameObject prefab;

    [Header("Position")]
    [SerializeField] Transform pivot;       // ������.
    [SerializeField] float radius;          // ������.
    [SerializeField] float spawnRate;       // ���� �ֱ�.

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
            // ���̰� 1�� ���͸� ���� �������� ����� radius��ŭ ���δ�.
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
