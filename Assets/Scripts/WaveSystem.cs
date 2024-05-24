using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentIndex = -1;
    public void StartWave()
    {
        if (enemySpawner.enemies.Count == 0 && currentIndex < waves.Length - 1)
        {
            //Debug.Log($"{currentIndex} // Current Index");
            Debug.Log($"{currentIndex+1} Wave 시작");
            currentIndex++;
            enemySpawner.StartWave(waves[currentIndex]);
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log($"{currentIndex} // Current Index");
            Debug.Log($"test code Wave 시작");
            Debug.Log($"{currentIndex + 1} Wave 시작");
            currentIndex++;
            enemySpawner.StartWave(waves[currentIndex]);
        }
    }
}
[System.Serializable]
public struct Wave
{
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemiePrefabs;
}
