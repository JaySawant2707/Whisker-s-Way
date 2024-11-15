using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerLivesText;
    [SerializeField] TextMeshProUGUI scoreText;
    int score;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        playerLivesText.text = FindObjectOfType<PlayerMortility>().playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void UpdateScore(int scorePoints)
    {
        score += scorePoints;
        scoreText.text = score.ToString();
    }

    public void UpdateLives(int lives)
    {
        playerLivesText.text = lives.ToString();
    }

    public void ResetGameSession()
    {
        //FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);//Need to change this is hard coded.
        Destroy(gameObject);
    }
}
