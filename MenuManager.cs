using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MenuManager : MonoBehaviour
{
    public GameObject mainBgPanel;
    public GameObject settingPanel;
    public GameObject gameUICanvas;

    public void StartGame()
    {
        gameObject.SetActive(false);
        gameUICanvas.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenSettingPanel()
    {
        mainBgPanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        mainBgPanel.SetActive(true);
        settingPanel.SetActive(false);
    }
}