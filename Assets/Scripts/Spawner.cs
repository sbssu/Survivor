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
    [SerializeField] GameObject reaper;

    [Header("Position")]    
    [SerializeField] float radius;          // 반지름.
    [SerializeField] float spawnRate;       // 스폰 주기.
    [SerializeField] float offsetRate;      // 스폰 주기 차이.

    bool isSpawn;
    float rateTime;

    private void Start()
    {
        rateTime = spawnRate;
        reaper.SetActive(false);
    }

    void Update()
    {
        if (isPauseObject || !isSpawn)
            return;

        rateTime -= Time.deltaTime;
        if (rateTime <= 0.0f)
        {
            // 길이가 1인 벡터를 랜덤 방향으로 만들어 radius만큼 늘인다.
            Vector2 position = (Vector2)Player.Instance.transform.position + Random.insideUnitCircle.normalized * radius;
            Enemy newEnemy = Instantiate(GetEnemyPrefab(), transform);
            newEnemy.transform.position = position;
            newEnemy.Setup();
            rateTime = spawnRate - (offsetRate * GameManager.Instance.gameLevel);
        }

        // 시간이 다 흘러서 몬스터 전체 삭제.
        if (GameManager.Instance.gameTime >= GameManager.MAX_GAME_TIME)
        {
            reaper.SetActive(true);
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
                enemy.DeadForce();

            enabled = false;
        }
    }
        
    public void SwitchSpawner(bool isOn)
    {
        isSpawn = isOn;
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
        if (Player.Instance != null)
            position = Player.Instance.transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(position, radius);
    }
}
