using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ovrtest : MonoBehaviour
{
    // Right Controller의 트랜스폼 (XR Origin의 Right Controller 참조)
    public Transform ControllerTransform;
    // Main Camera의 트랜스폼(플레이어의 시점)
    public Transform CameraTransform;
    // 조이스틱 입력 최소값 (이 값 이상으로 움직여야 작동)
    public float cameraInputThreshold = 0.1f;
    // 플레이어의 이동 속도
    public float moveSpeed = 2.0f;
    // 플레이어 시점의 카메라 회전 속도
    public float rotateSpeed = 100.0f;
    // 플레이어의 시점 제한
    public float pitchMin = -85f; // 아래로 85도 까지
    public float pitchMax = 85f; // 위로 85도 까지

    // 매 프레임 업데이트
    void Update()
    {
        BtnDown();
        MoveCamera();
    }

    // 버튼 입력 처리
    void BtnDown()
    {
        // A 버튼 클릭 시
        if (OVRInput.Get(OVRInput.Button.One))
        {
            HandleClick();
            Debug.Log("A 버튼 클릭 감지"); // A 버튼 클릭 시 로그 추가
        }

        // B 버튼 클릭 시
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            Debug.Log("B 버튼 클릭 감지"); // B 버튼 클릭 시 로그 추가
            HandleClick();
        }
    }

    // 클릭 처리 함수 (레이캐스트를 사용해 오브젝트 클릭 감지)
    void HandleClick()
    {
        RaycastHit hit;

        // 컨트롤러의 위치에서 컨트롤러가 보는 방향으로 레이 발사
        Ray ray = new Ray(ControllerTransform.position, ControllerTransform.forward);

        // 레이를 시각적으로 그립니다 (길이를 1000으로 설정)
        Debug.DrawRay(ControllerTransform.position, ControllerTransform.forward * 3000, Color.red, 15.0f);

        // 레이가 충돌한 오브젝트 확인
        if (Physics.Raycast(ray, out hit, 1000)) // Raycast의 거리도 1000으로 설정
        {
            Debug.Log("Hit object: " + hit.collider.name); // 충돌한 오브젝트 이름 출력

            // 충돌한 오브젝트에 ObjectClick 컴포넌트가 있는지 확인
            ObjectClick objectClick = hit.collider.GetComponent<ObjectClick>();
            if (objectClick != null)
            {
                objectClick.LoadScene(); // 클릭된 오브젝트의 LoadScene 메서드 호출
            }
        }
    }

    void MoveCamera()
    {
        // 왼쪽 컨트롤러의 조이스틱 입력값 받기
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // 조이스틱 입력의 절대값이 일정 수치 이상일 때 작동 (진동 등의 미세한 움직임에 의한 이동 방지)
        if (Mathf.Abs(joystickInput.x) > cameraInputThreshold || Mathf.Abs(joystickInput.y) > cameraInputThreshold)
        {
            // 조이스틱 x축으로 수평 이동
            float moveX = joystickInput.x * moveSpeed * Time.deltaTime;

            // 조이스틱 y축으로 수직 이동
            float moveZ = joystickInput.y * moveSpeed * Time.deltaTime;

            // 카메라의 현재 위치를 기준으로 이동 적용
            Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

            // 카메라의 로컬 좌표계를 기준으로 이동
            CameraTransform.Translate(moveDirection);

            Debug.Log("조이스틱 입력 감지: X=" + joystickInput.x + " Y=" + joystickInput.y);
        }
    }

    void RotateCamera()
    {
        // 오른쪽 컨트롤러의 조이스틱 입력 값 받기
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // 조이스틱 입력의 절대값이 일정 수치 이상일 때 작동 (진동 등의 미세한 움직임에 의한 이동 방지)
        if (Mathf.Abs(joystickInput.x) > cameraInputThreshold || Mathf.Abs(joystickInput.y) > cameraInputThreshold)
        {
            // 조이스틱 x축을 기준으로 수평 회전 (Yaw 회전)
            float rotateY = joystickInput.x * rotateSpeed * Time.deltaTime;

            // 조이스틱 y축을 기준으로 수직 회전 (Pitch 회전)
            float rotateX = -joystickInput.y * rotateSpeed * Time.deltaTime;  // 위로 밀면 아래로 회전하므로 반대 방향으로

            // 현재 Pitch 값을 제한
            float currentPitch = CameraTransform.eulerAngles.x;

            // eulerAngles는 0~360도 범위로 나오므로 이를 -180~180도로 변환
            if (currentPitch > 180) currentPitch -= 360;

            // 새로운 Pitch 값 계산 및 제한 적용
            float newPitch = Mathf.Clamp(currentPitch + rotateX, pitchMin, pitchMax);

            // 카메라의 회전 적용 (Pitch와 Yaw만, Roll은 그대로 유지)
            CameraTransform.eulerAngles = new Vector3(newPitch, CameraTransform.eulerAngles.y + rotateY, 0);

            Debug.Log("오른쪽 조이스틱 입력 감지: X=" + joystickInput.x + " Y=" + joystickInput.y);
        }
    }
}
