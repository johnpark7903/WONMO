using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Player;   // 따라갈 플레이어 트랜스폼
    public Vector3 offset = new Vector3(0, 3, -5);     // 카메라 위치 오프셋
    public float sensitivity = 3f;  // 마우스 감도
    public float distance = 6f;     // 플레이어와 카메라 거리
    public float minY = -35f;       // 카메라 각도 제한(아래)
    public float maxY = 70f;        // 카메라 각도 제한(위)

    float yaw = 0f;                 // 좌우 회전
    float pitch = 20f;              // 상하 회전

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 커서 고정
    }
    void Update()
    {
        transform.position = Player.position + offset;
    }

    void LateUpdate()
    {
        // 마우스 입력
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        // 상하 각도 제한
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // 카메라 회전 적용
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // 플레이어 뒤쪽으로 오프셋 거리 유지
        Vector3 camPosition = Player.position + rotation * new Vector3(0, 0, -distance) + Vector3.up * offset.y;
        transform.position = camPosition;
        transform.LookAt(Player.position + Vector3.up * offset.y);
    }
    // Start is called before the first frame update

    // Update is called once per frame
    // public Transform target;
    // public Vector3 offset;

 
    // #2
    // public float sensitivityX = 15.0f; // 좌우 민감도
    // public float sensitivityY = 15.0f; // 상하 민감도

    // public float minimumY = -60.0f; // 상하 각도 제한
    // public float maximumY = 60.0f;

    // float rotationY = 0.0f;

    // void Update()
    // {
    //     float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

    //     rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
    //     rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

    //     transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    // }
}
