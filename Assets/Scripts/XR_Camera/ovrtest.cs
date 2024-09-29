using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ovrtest : MonoBehaviour
{
    // Right Controller�� Ʈ������ (XR Origin�� Right Controller ����)
    public Transform ControllerTransform;
    // Main Camera�� Ʈ������(�÷��̾��� ����)
    public Transform CameraTransform;
    // ���̽�ƽ �Է� �ּҰ� (�� �� �̻����� �������� �۵�)
    public float cameraInputThreshold = 0.1f;
    // �÷��̾��� �̵� �ӵ�
    public float moveSpeed = 2.0f;
    // �÷��̾� ������ ī�޶� ȸ�� �ӵ�
    public float rotateSpeed = 100.0f;
    // �÷��̾��� ���� ����
    public float pitchMin = -85f; // �Ʒ��� 85�� ����
    public float pitchMax = 85f; // ���� 85�� ����

    // �� ������ ������Ʈ
    void Update()
    {
        BtnDown();
        MoveCamera();
    }

    // ��ư �Է� ó��
    void BtnDown()
    {
        // A ��ư Ŭ�� ��
        if (OVRInput.Get(OVRInput.Button.One))
        {
            HandleClick();
            Debug.Log("A ��ư Ŭ�� ����"); // A ��ư Ŭ�� �� �α� �߰�
        }

        // B ��ư Ŭ�� ��
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            Debug.Log("B ��ư Ŭ�� ����"); // B ��ư Ŭ�� �� �α� �߰�
            HandleClick();
        }
    }

    // Ŭ�� ó�� �Լ� (����ĳ��Ʈ�� ����� ������Ʈ Ŭ�� ����)
    void HandleClick()
    {
        RaycastHit hit;

        // ��Ʈ�ѷ��� ��ġ���� ��Ʈ�ѷ��� ���� �������� ���� �߻�
        Ray ray = new Ray(ControllerTransform.position, ControllerTransform.forward);

        // ���̸� �ð������� �׸��ϴ� (���̸� 1000���� ����)
        Debug.DrawRay(ControllerTransform.position, ControllerTransform.forward * 3000, Color.red, 15.0f);

        // ���̰� �浹�� ������Ʈ Ȯ��
        if (Physics.Raycast(ray, out hit, 1000)) // Raycast�� �Ÿ��� 1000���� ����
        {
            Debug.Log("Hit object: " + hit.collider.name); // �浹�� ������Ʈ �̸� ���

            // �浹�� ������Ʈ�� ObjectClick ������Ʈ�� �ִ��� Ȯ��
            ObjectClick objectClick = hit.collider.GetComponent<ObjectClick>();
            if (objectClick != null)
            {
                objectClick.LoadScene(); // Ŭ���� ������Ʈ�� LoadScene �޼��� ȣ��
            }
        }
    }

    void MoveCamera()
    {
        // ���� ��Ʈ�ѷ��� ���̽�ƽ �Է°� �ޱ�
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // ���̽�ƽ �Է��� ���밪�� ���� ��ġ �̻��� �� �۵� (���� ���� �̼��� �����ӿ� ���� �̵� ����)
        if (Mathf.Abs(joystickInput.x) > cameraInputThreshold || Mathf.Abs(joystickInput.y) > cameraInputThreshold)
        {
            // ���̽�ƽ x������ ���� �̵�
            float moveX = joystickInput.x * moveSpeed * Time.deltaTime;

            // ���̽�ƽ y������ ���� �̵�
            float moveZ = joystickInput.y * moveSpeed * Time.deltaTime;

            // ī�޶��� ���� ��ġ�� �������� �̵� ����
            Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

            // ī�޶��� ���� ��ǥ�踦 �������� �̵�
            CameraTransform.Translate(moveDirection);

            Debug.Log("���̽�ƽ �Է� ����: X=" + joystickInput.x + " Y=" + joystickInput.y);
        }
    }

    void RotateCamera()
    {
        // ������ ��Ʈ�ѷ��� ���̽�ƽ �Է� �� �ޱ�
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // ���̽�ƽ �Է��� ���밪�� ���� ��ġ �̻��� �� �۵� (���� ���� �̼��� �����ӿ� ���� �̵� ����)
        if (Mathf.Abs(joystickInput.x) > cameraInputThreshold || Mathf.Abs(joystickInput.y) > cameraInputThreshold)
        {
            // ���̽�ƽ x���� �������� ���� ȸ�� (Yaw ȸ��)
            float rotateY = joystickInput.x * rotateSpeed * Time.deltaTime;

            // ���̽�ƽ y���� �������� ���� ȸ�� (Pitch ȸ��)
            float rotateX = -joystickInput.y * rotateSpeed * Time.deltaTime;  // ���� �и� �Ʒ��� ȸ���ϹǷ� �ݴ� ��������

            // ���� Pitch ���� ����
            float currentPitch = CameraTransform.eulerAngles.x;

            // eulerAngles�� 0~360�� ������ �����Ƿ� �̸� -180~180���� ��ȯ
            if (currentPitch > 180) currentPitch -= 360;

            // ���ο� Pitch �� ��� �� ���� ����
            float newPitch = Mathf.Clamp(currentPitch + rotateX, pitchMin, pitchMax);

            // ī�޶��� ȸ�� ���� (Pitch�� Yaw��, Roll�� �״�� ����)
            CameraTransform.eulerAngles = new Vector3(newPitch, CameraTransform.eulerAngles.y + rotateY, 0);

            Debug.Log("������ ���̽�ƽ �Է� ����: X=" + joystickInput.x + " Y=" + joystickInput.y);
        }
    }
}
