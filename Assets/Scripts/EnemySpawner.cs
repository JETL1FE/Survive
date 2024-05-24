using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform pointPrefab;  // ���� ����Ʈ ������
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
                // �� GameObject ���� �� Transform ������Ʈ �Ҵ�
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
            Debug.LogError("Points �迭 �ʱ�ȭ �ʿ�");
            return;
        }

        Camera mainCamera = Camera.main;

        // ����Ʈ ��ǥ�� ����Ͽ� ���� ����Ʈ ���
        Vector3[] viewportPoints = new Vector3[8];

        // ȭ���� �� ������ ������ offset ��ŭ ������ ����
        viewportPoints[0] = new Vector3(-offset, 1 + offset, mainCamera.nearClipPlane); // �»��
        viewportPoints[1] = new Vector3(1 + offset, 1 + offset, mainCamera.nearClipPlane); // ����
        viewportPoints[2] = new Vector3(1 + offset, -offset, mainCamera.nearClipPlane); // ���ϴ�
        viewportPoints[3] = new Vector3(-offset, -offset, mainCamera.nearClipPlane); // ���ϴ�

        // ȭ���� �� �� �߾ӿ��� offset ��ŭ ������ ����
        viewportPoints[4] = new Vector3(0.5f, 1 + offset, mainCamera.nearClipPlane); // ��� �߾�
        viewportPoints[5] = new Vector3(1 + offset, 0.5f, mainCamera.nearClipPlane); // ���� �߾�
        viewportPoints[6] = new Vector3(0.5f, -offset, mainCamera.nearClipPlane); // �ϴ� �߾�
        viewportPoints[7] = new Vector3(-offset, 0.5f, mainCamera.nearClipPlane); // ���� �߾�

        // ����Ʈ ��ǥ�� ���� ��ǥ�� ��ȯ
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

            // ���� ����Ʈ�� ��ġ�� �پ缺�� �ο��ϱ� ���� ������ x, y ���� �����ݴϴ�.
            float offsetX = Random.Range(-1f, 1f); // x ��ǥ�� ���� ������ ��
            float offsetY = Random.Range(-1f, 1f); // y ��ǥ�� ���� ������ ��

            // ���ο� ���� ����Ʈ ��ġ ���
            Vector3 spawnPosition = Points[pointsIndex].position + new Vector3(offsetX, offsetY, 0f);

            // �� ������Ʈ ����
            GameObject clone = Instantiate(currentWave.enemiePrefabs[enemyIndex], spawnPosition, Quaternion.identity);
            enemies.Add(clone);

            // ���� HP �����̴� ����
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
