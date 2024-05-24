using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[SerializeField]
//private Transform target;

//[SerializeField]
//private float smoothSpeed = 5f; // �̵� �ӵ� ���� ����
////[SerializeField]
////private float zoomSpeed = 5f; // Ȯ�� �� ��� �ӵ� ���� ����
////[SerializeField]
////private float minZoom = 1f; // �ּ� Ȯ�� ����
////[SerializeField]
////private float maxZoom = 5f; // �ִ� Ȯ�� ����

//private void Start()
//{ 
//    StartCoroutine(FindPlayer());
//}
//IEnumerator FindPlayer()
//{
//    yield return new WaitForEndOfFrame(); // ���� ������ �ε�� ������ ��ٸ��ϴ�.
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
    public float maxDistance = 5f; // �ִ� �̵� ������ �Ÿ�
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

        // C Ű�� ������ �� Ȯ�� ���� ��ȯ
        if (Input.GetKeyDown(zoomKey))
        {
            zooming = true;
        }
        // C Ű�� ���� Ȯ�� ��� ����
        else if (Input.GetKeyUp(zoomKey))
        {
            zooming = false;
        }

        // Ȯ�� ����� ��
        if (zooming)
        {
            // ���콺�� ���� ������ ���
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ī�޶��� �߽��� ���콺 ��ġ�� �̵�
            desiredPosition = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

            // Ÿ�� ��ġ�κ��� ���� �Ÿ� �̳��θ� �̵� �����ϵ��� ����
            Vector3 targetToMouse = mousePosition - target.position;
            if (targetToMouse.magnitude > maxDistance)
            {
                desiredPosition = target.position + targetToMouse.normalized * maxDistance;
            }
        }
        else
        {
            // �⺻������ Ÿ�� �ֺ��� ��ġ
            desiredPosition = target.position + offset;
        }

        // ī�޶��� ��ġ�� �ε巴�� �̵�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothedPosition.z = transform.position.z; // ���� z �� ����
        transform.position = smoothedPosition;
    }
}