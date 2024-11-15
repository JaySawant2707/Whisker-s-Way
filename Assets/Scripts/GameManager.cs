using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;

    public void PauseBtn()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void ResumeBtn()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartBtn()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StratBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
