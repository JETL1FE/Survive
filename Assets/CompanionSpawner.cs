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
                // 랜덤한 인덱스를 선택합니다.
                int startIndex = UnityEngine.Random.Range(0, Points.Length);
                int pointsIndex = Random.Range(0, Points.Length);

                // 스폰 포인트의 위치에 다양성을 부여하기 위해 임의의 x, y 값을 더해줍니다.
                float offsetX = Random.Range(-1f, 1f); // x 좌표에 더할 임의의 값
                float offsetY = Random.Range(-1f, 1f); // y 좌표에 더할 임의의 값

                // 새로운 스폰 포인트 위치 계산
                Vector3 spawnPosition = Points[pointsIndex].position + new Vector3(offsetX, offsetY, 0f);

                // 랜덤한 위치에 오브젝트를 생성합니다.
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
    //        // 랜덤한 스폰 위치 선택
    //        Transform randomSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

    //        // SpawnEnemy 함수 호출
    //        int randomIndex = UnityEngine.Random.Range(0, currentWave.enemiePrefabs.Length);
    //        GameObject clone = Instantiate(currentWave.enemiePrefabs[randomIndex], randomSpawnPoint.position, randomSpawnPoint.rotation);

    //        // 생성된 적에게 HP 슬라이더 추가
    //        SpawnAllyHpSlider(clone);

    //        // 생성된 적 수 증가
    //        spawnEnemyCount++;

    //        // 다음 적 생성을 위해 대기
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
