using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ovrtest : MonoBehaviour
{
    // ī�޶� Ʈ������ (XR Origin�� �ִ� ī�޶� ������Ʈ ����)
    public Transform cameraTransform;

    // �� ������ ������Ʈ
    void Update()
    {
        Debug.Log("Update �޼��� ȣ���"); // Update �޼��� ȣ�� �� �α� �߰�
        BtnDown();
    }

    // ��ư �Է� ó��
    void BtnDown()
    {
        // A ��ư Ŭ�� ��
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            HandleClick();
            Debug.Log("A ��ư Ŭ�� ����"); // A ��ư Ŭ�� �� �α� �߰�
        }

        // B ��ư Ŭ�� ��
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Debug.Log("B ��ư Ŭ�� ����"); // B ��ư Ŭ�� �� �α� �߰�
            HandleClick();
        }
    }

    // Ŭ�� ó�� �Լ� (����ĳ��Ʈ�� ����� ������Ʈ Ŭ�� ����)
    void HandleClick()
    {
        RaycastHit hit;

        // ī�޶��� ��ġ���� ī�޶� ���� �������� ���� �߻�
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        // ���̰� �浹�� ������Ʈ Ȯ��
        if (Physics.Raycast(ray, out hit))
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
}