using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    public GameObject[] allys;
    public GameObject allyHpSliderPrefab;

    public Transform canvasTransform;
    public Transform[] Points;

    public bool waving = false;
    public float spawnDelay = 5f;



    public void StartRepeater(float time)
    {
        spawnDelay = time;
        StartCoroutine(SpawnAllyRepeater());
    }
    public void StopSpawn()
    {
        StopCoroutine(SpawnAllyRepeater());
    }
    //private IEnumerator SpawnAllyRepeater()
    //{
    //    while (true)
    //    {
    //        if (!waving)
    //        {
    //            Transform randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
    //            int randomIndex = UnityEngine.Random.Range(0, allys.Length);
    //            GameObject clone = Instantiate(allys[randomIndex], randomSpawnPoint.position, randomSpawnPoint.rotation);
    //            clone.name = allys[randomIndex].name;
    //            SpawnAllyHpSlider(clone);
    //            waving = true;
    //            yield return new WaitForSeconds(spawnDelay);
    //            waving = false;
    //        }
    //        else
    //        {
    //            yield return null;
    //        }
    //    }
    //}
    private IEnumerator SpawnAllyRepeater()
    {
        while (true)
        {
            if (!waving)
            {
                // ������ �ε����� �����մϴ�.
                int startIndex = UnityEngine.Random.Range(0, Points.Length);
                int pointsIndex = Random.Range(0, Points.Length);

                // ���� ����Ʈ�� ��ġ�� �پ缺�� �ο��ϱ� ���� ������ x, y ���� �����ݴϴ�.
                float offsetX = Random.Range(-1f, 1f); // x ��ǥ�� ���� ������ ��
                float offsetY = Random.Range(-1f, 1f); // y ��ǥ�� ���� ������ ��

                // ���ο� ���� ����Ʈ ��ġ ���
                Vector3 spawnPosition = Points[pointsIndex].position + new Vector3(offsetX, offsetY, 0f);

                // ������ ��ġ�� ������Ʈ�� �����մϴ�.
                int randomIndex = UnityEngine.Random.Range(0, allys.Length);
                GameObject clone = Instantiate(allys[randomIndex], spawnPosition, Quaternion.identity);
                clone.name = allys[randomIndex].name;
                SpawnAllyHpSlider(clone);
                waving = true;
                yield return new WaitForSeconds(spawnDelay);
                waving = false;
            }
            else
            {
                yield return null;
            }
        }
    }
    //private IEnumerator SpawnAlly()
    //{
    //    int spawnEnemyCount = 0;
    //    while (spawnEnemyCount < currentWave.maxEnemyCount)
    //    {
    //        // ������ ���� ��ġ ����
    //        Transform randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

    //        // SpawnEnemy �Լ� ȣ��
    //        int randomIndex = UnityEngine.Random.Range(0, currentWave.enemiePrefabs.Length);
    //        GameObject clone = Instantiate(currentWave.enemiePrefabs[randomIndex], randomSpawnPoint.position, randomSpawnPoint.rotation);

    //        // ������ ������ HP �����̴� �߰�
    //        SpawnAllyHpSlider(clone);

    //        // ������ �� �� ����
    //        spawnEnemyCount++;

    //        // ���� �� ������ ���� ���
    //        yield return new WaitForSeconds(currentWave.spawnTime);
    //    }
    //}
    void SpawnAllyHpSlider(GameObject _ally)
    {
        GameObject sliderClone = Instantiate(allyHpSliderPrefab);
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;
        sliderClone.GetComponent<SliderPositionAutoSetter>().SetUp(_ally.transform);
        sliderClone.GetComponent<HpViewer>().SetUp(_ally.GetComponent<LivingEntity>());
    }

}
