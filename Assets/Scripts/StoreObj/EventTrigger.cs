using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectClick : MonoBehaviour
{
    public string sceneName; // 유니티 인스펙터에서 씬 이름을 설정할 수 있음

    // 씬을 전환하는 메서드
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName); // 설정된 씬으로 전환
        }
        else
        {
            Debug.LogError("씬 이름이 설정되지 않았습니다.");
        }
    }
}
