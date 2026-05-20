using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int totalSpawnEnemies;
        public int numberOfRandomSpawnPoint;
        public float delayStart;
        public float spawnInterval;
        public int numberOfPowerUp;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;

    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(StartWaves()); //ต้องทำหาย Enemy ตายหรือเอาออกจากเกมถึงจะเริ่มเวฟนะครับ
    }

    IEnumerator StartWaves()
    {
        foreach (var wave in waves)
        {
            yield return StartCoroutine(RunWave(wave));
        }
        Debug.Log("All waves completed!");
    }

    IEnumerator RunWave(Wave wave)
    {
        Debug.Log($"Wave {currentWave + 1} started!");

        List<Transform> selectedPoints = GetRandomSpawnPoints(wave.numberOfRandomSpawnPoint);

        for (int i = 0; i < wave.numberOfPowerUp; i++)
        {
            Transform point = selectedPoints[Random.Range(0, selectedPoints.Count)];
            Instantiate(powerUpPrefab, point.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(wave.delayStart);

        for (int i = 0; i < wave.totalSpawnEnemies; i++)
        {
            Transform point = selectedPoints[Random.Range(0, selectedPoints.Count)];
            Instantiate(enemyPrefab, point.position, Quaternion.identity);

            if (i < wave.totalSpawnEnemies - 1)
                yield return new WaitForSeconds(wave.spawnInterval);
        }

        yield return StartCoroutine(WaitForEnemiesDead()); //จุดเช็ค Enemy ตาย

        Debug.Log($"Wave {currentWave + 1} completed!");
        currentWave++;
    }

    IEnumerator WaitForEnemiesDead()
    {
        yield return new WaitForSeconds(1f);
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)//จุดเช็ค Enemy ตายหมดยัง
        {
            yield return new WaitForSeconds(1f);
        }
    }

    List<Transform> GetRandomSpawnPoints(int count)
    {
        List<Transform> pool = new List<Transform>(spawnPoints);
        List<Transform> selected = new List<Transform>();

        count = Mathf.Min(count, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, pool.Count);
            selected.Add(pool[rand]);
            pool.RemoveAt(rand);
        }

        return selected;
    }
}