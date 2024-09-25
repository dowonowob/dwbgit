using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectClick : MonoBehaviour
{
    public string sceneName; // ����Ƽ �ν����Ϳ��� �� �̸��� ������ �� ����

    // ���� ��ȯ�ϴ� �޼���
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName); // ������ ������ ��ȯ
        }
        else
        {
            Debug.LogError("�� �̸��� �������� �ʾҽ��ϴ�.");
        }
    }
}
