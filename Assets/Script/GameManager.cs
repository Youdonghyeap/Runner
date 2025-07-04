using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameOver = false;
    public TextMeshProUGUI scoreText;
    public GameObject GameoverUi;
    private int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 씬에서 GameoverUi가 활성화되어 있더라도 무조건 비활성화
        if (GameoverUi != null)
            GameoverUi.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameOver && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(0);
            GameoverUi.SetActive(false);
        }
    }

    public void AddScore(int newScore)
    {
        if (!isGameOver)
        {
            score += newScore;
            scoreText.text = "Score: " + score;
        }
    }

    public void OnPlayerDead()
    {
        isGameOver = true;
        if (GameoverUi != null)
            GameoverUi.SetActive(true);
    }
}
