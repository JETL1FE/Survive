using System.Collections.Generic;
using UnityEngine;

public class OffScreenIndicator : MonoBehaviour
{
    public GameObject[] targets; // 화면에 표시할 대상의 위치
    public GameObject indicatorPrefab;
    private SpriteRenderer spriteRenderer;
    private float spriteWidth;
    private float spriteHeight;
    private Camera _camera;
    public Dictionary<GameObject, GameObject> targetIndicator = new Dictionary<GameObject, GameObject>();

    [SerializeField]
    private GameObject[] allyObjects;

    private void Start()
    {
        _camera = Camera.main;
        spriteRenderer = indicatorPrefab.GetComponent<SpriteRenderer>();
        var bounds = spriteRenderer.bounds;
        spriteWidth = bounds.size.x / 2f;
        spriteHeight = bounds.size.y / 2f;
        foreach (var target in targets)
        {
            var indicator = Instantiate(indicatorPrefab);
            indicator.SetActive(false);
            targetIndicator.Add(target, indicator);
        }
    }

    private void Update()
    {
        foreach (KeyValuePair<GameObject, GameObject> entry in targetIndicator)
        {
            var target = entry.Key;
            var indicator = entry.Value;

            if (target != null) // null 체크
            {
                UpdateTarget(target, indicator);
            }
            else
            {
                indicator.SetActive(false);
            }
        }
    }
    private void UpdateTarget(GameObject target, GameObject indicator)
    {
        var screenPos = _camera.WorldToViewportPoint(target.transform.position);
        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= 1 || screenPos.y <= 0 || screenPos.y >= 1;
        if (isOffScreen)
        {
            indicator.SetActive(true);
            var spriteSizeInViewPort = _camera.WorldToViewportPoint(new Vector3(spriteWidth, spriteHeight, 0)) - _camera.WorldToViewportPoint(Vector3.zero);

            screenPos.x = Mathf.Clamp(screenPos.x, spriteSizeInViewPort.x, 1 - spriteSizeInViewPort.x);
            screenPos.y = Mathf.Clamp(screenPos.y, spriteSizeInViewPort.y, 1 - spriteSizeInViewPort.y);

            var worldPosition = _camera.ViewportToWorldPoint(screenPos);
            worldPosition.z = 0;
            indicator.transform.position = worldPosition;
            Vector3 direction = target.transform.position - indicator.transform.position;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            indicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
        }
        else
        {
            indicator.SetActive(false);
        }
    }
}
//public void AddIndicator(GameObject gameObject)
//{
//    targetIndicator.Add(gameObject,indicatorPrefab);
//}