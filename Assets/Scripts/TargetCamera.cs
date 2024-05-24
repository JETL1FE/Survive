using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[SerializeField]
//private Transform target;

//[SerializeField]
//private float smoothSpeed = 5f; // 이동 속도 조절 변수
////[SerializeField]
////private float zoomSpeed = 5f; // 확대 및 축소 속도 조절 변수
////[SerializeField]
////private float minZoom = 1f; // 최소 확대 정도
////[SerializeField]
////private float maxZoom = 5f; // 최대 확대 정도

//private void Start()
//{ 
//    StartCoroutine(FindPlayer());
//}
//IEnumerator FindPlayer()
//{
//    yield return new WaitForEndOfFrame(); // 씬이 완전히 로드될 때까지 기다립니다.
//    target = GameObject.FindWithTag("Player").transform;
//}
//void Update()
//{
//    if(target)
//    {
//        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
//        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
//        transform.position = smoothedPosition;

//        //float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
//        //float newSize = Mathf.Clamp(Camera.main.orthographicSize - scrollWheel * zoomSpeed, minZoom, maxZoom);
//        //Camera.main.orthographicSize = newSize;
//    }
//}
public class TargetCamera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.05f;
    public float maxDistance = 5f; // 최대 이동 가능한 거리
    public KeyCode zoomKey = KeyCode.C;

    private Vector3 offset;
    private bool zooming = false;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition;

        // C 키를 눌렀을 때 확대 모드로 전환
        if (Input.GetKeyDown(zoomKey))
        {
            zooming = true;
        }
        // C 키를 떼면 확대 모드 해제
        else if (Input.GetKeyUp(zoomKey))
        {
            zooming = false;
        }

        // 확대 모드일 때
        if (zooming)
        {
            // 마우스의 월드 포지션 계산
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 카메라의 중심을 마우스 위치로 이동
            desiredPosition = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

            // 타겟 위치로부터 일정 거리 이내로만 이동 가능하도록 제한
            Vector3 targetToMouse = mousePosition - target.position;
            if (targetToMouse.magnitude > maxDistance)
            {
                desiredPosition = target.position + targetToMouse.normalized * maxDistance;
            }
        }
        else
        {
            // 기본적으로 타겟 주변에 위치
            desiredPosition = target.position + offset;
        }

        // 카메라의 위치를 부드럽게 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothedPosition.z = transform.position.z; // 현재 z 값 유지
        transform.position = smoothedPosition;
    }
}