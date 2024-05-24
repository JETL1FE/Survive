using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform pointPrefab;  // 스폰 포인트 프리팹
    public Transform[] Points = new Transform[8];
    public float offset = 1.0f;
    public GameObject HpSliderPrefab;
    public float delay = 3f;
    public Transform canvasTransform;
    private Wave currentWave;

    public List<GameObject> enemies = new List<GameObject>();
    public void StartWave(Wave wave)
    {
        currentWave = wave;
        //vector2s = new Vector2[8];
        StartCoroutine(SpawnMonsterRepeter());
    }
    void Start()
    {
        InitializePoints();
    }
    private void Update()
    {
        CalculateSpawnPoints();
    }
    void InitializePoints()
    {
        for (int i = 0; i < Points.Length; i++)
        {
            if (Points[i] == null)
            {
                // 새 GameObject 생성 및 Transform 컴포넌트 할당
                Transform pointObject = Instantiate(pointPrefab, Vector3.zero, Quaternion.identity,transform);
                pointObject.name = "SpawnPoint_" + i;
                Points[i] = pointObject.transform;
            }
        }
    }
    void CalculateSpawnPoints()
    {
        if (Points == null || Points.Length != 8)
        {
            Debug.LogError("Points 배열 초기화 필요");
            return;
        }

        Camera mainCamera = Camera.main;

        // 뷰포트 좌표를 사용하여 스폰 포인트 계산
        Vector3[] viewportPoints = new Vector3[8];

        // 화면의 각 꼭지점 밖으로 offset 만큼 떨어진 지점
        viewportPoints[0] = new Vector3(-offset, 1 + offset, mainCamera.nearClipPlane); // 좌상단
        viewportPoints[1] = new Vector3(1 + offset, 1 + offset, mainCamera.nearClipPlane); // 우상단
        viewportPoints[2] = new Vector3(1 + offset, -offset, mainCamera.nearClipPlane); // 우하단
        viewportPoints[3] = new Vector3(-offset, -offset, mainCamera.nearClipPlane); // 좌하단

        // 화면의 각 변 중앙에서 offset 만큼 떨어진 지점
        viewportPoints[4] = new Vector3(0.5f, 1 + offset, mainCamera.nearClipPlane); // 상단 중앙
        viewportPoints[5] = new Vector3(1 + offset, 0.5f, mainCamera.nearClipPlane); // 우측 중앙
        viewportPoints[6] = new Vector3(0.5f, -offset, mainCamera.nearClipPlane); // 하단 중앙
        viewportPoints[7] = new Vector3(-offset, 0.5f, mainCamera.nearClipPlane); // 좌측 중앙

        // 뷰포트 좌표를 월드 좌표로 변환
        for (int i = 0; i < viewportPoints.Length; i++)
        {
            Vector3 worldPoint = mainCamera.ViewportToWorldPoint(viewportPoints[i]);
            if (Points[i] != null)
            {
                Points[i].position = worldPoint;
            }
        }
    }
    //private IEnumerator SpawnMonsterRepeter()
    //{
    //    int spawnCount = 0;
    //    while (spawnCount < currentWave.maxEnemyCount)
    //    {
    //        int enemyIndex = Random.Range(0, currentWave.enemiePrefabs.Length);
    //        int pointsIndex = Random.Range(0, Points.Length);
    //        GameObject clone = Instantiate(currentWave.enemiePrefabs[enemyIndex], Points[pointsIndex]);
    //        enemies.Add(clone);
    //        SpawnHpSlider(clone);
    //        spawnCount++;
    //    yield return new WaitForSeconds(currentWave.spawnTime);
    //    }
    //}
    private IEnumerator SpawnMonsterRepeter()
    {
        int spawnCount = 0;
        while (spawnCount < currentWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0, currentWave.enemiePrefabs.Length);
            int pointsIndex = Random.Range(0, Points.Length);

            // 스폰 포인트의 위치에 다양성을 부여하기 위해 임의의 x, y 값을 더해줍니다.
            float offsetX = Random.Range(-1f, 1f); // x 좌표에 더할 임의의 값
            float offsetY = Random.Range(-1f, 1f); // y 좌표에 더할 임의의 값

            // 새로운 스폰 포인트 위치 계산
            Vector3 spawnPosition = Points[pointsIndex].position + new Vector3(offsetX, offsetY, 0f);

            // 적 오브젝트 생성
            GameObject clone = Instantiate(currentWave.enemiePrefabs[enemyIndex], spawnPosition, Quaternion.identity);
            enemies.Add(clone);

            // 적의 HP 슬라이더 생성
            SpawnHpSlider(clone);

            spawnCount++;
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }
    void SpawnHpSlider(GameObject monster)
    {
        GameObject sliderClone = Instantiate(HpSliderPrefab);
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;
        sliderClone.GetComponent<SliderPositionAutoSetter>().SetUp(monster.transform);
        sliderClone.GetComponent<HpViewer>().SetUp(monster.GetComponent<LivingEntity>());
    }
    void OnDrawGizmos()
    {
        if (Points == null || Points.Length != 8)
            return;

        Gizmos.color = Color.red;

        foreach (Transform point in Points)
        {
            if (point != null)
            {
                Gizmos.DrawSphere(point.position, 0.5f);
            }
        }
    }
}
