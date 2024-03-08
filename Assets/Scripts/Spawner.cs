using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : RootBehaiour
{
    [System.Serializable]
    public class SpwanEnemy
    {
        public Enemy prefab;
        public int limitLevel;
        public int spawnWeight;
    }

    [Header("Object")]
    [SerializeField] SpwanEnemy[] enemyPrefabs;

    [Header("Position")]
    [SerializeField] Transform pivot;       // ������.
    [SerializeField] float radius;          // ������.
    [SerializeField] float spawnRate;       // ���� �ֱ�.
    [SerializeField] float offsetRate;      // ���� �ֱ� ����.

    float rateTime;

    private void Start()
    {
        rateTime = spawnRate;

        Debug.Log("Ȯ��ǥ");
        Debug.Log("=======================");
        float totalWeight = enemyPrefabs.Select(e => e.spawnWeight).Sum();
        foreach(var enemy in enemyPrefabs)
            Debug.Log($"{enemy.prefab.name} : {enemy.spawnWeight / totalWeight * 100f}%");
        Debug.Log("=======================");

    }

    void Update()
    {
        if (isPauseObject)
            return;

        rateTime -= Time.deltaTime;
        if (rateTime <= 0.0f)
        {
            // ���̰� 1�� ���͸� ���� �������� ����� radius��ŭ ���δ�.
            Vector2 position = (Vector2)pivot.position + Random.insideUnitCircle.normalized * radius;
            Enemy newEnemy = Instantiate(GetEnemyPrefab(), transform);
            newEnemy.transform.position = position;
            rateTime = spawnRate - (offsetRate * GameManager.Instance.gameLevel);
        }
    }

    private Enemy GetEnemyPrefab()
    {
        int level = GameManager.Instance.gameLevel;
        var group = enemyPrefabs.Where(e => level < e.limitLevel);
        int totalWeight = group.Select(e => e.spawnWeight).Sum();
        float pick = Random.value * totalWeight;

        int min = 0;
        int max = 0;
        foreach (SpwanEnemy enemy in group)
        {
            max += enemy.spawnWeight;
            if (min <= pick && pick < max)
                return enemy.prefab;
            min += enemy.spawnWeight;
        }

        return null;
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
