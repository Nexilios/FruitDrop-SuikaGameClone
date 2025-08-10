using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI scoreText;
    
    [Header("Debugging")]
    [SerializeField] private int currentScore;
    [SerializeField] private int highScore;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        // Hide Mouse Pointer
        Cursor.visible = false;
        
        highScore = PlayerPrefs.GetInt("high_score", 0);
        
        if (scoreText) scoreText.text = "SCORE: " + currentScore;
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText)
        {
            scoreText.text = "SCORE: " + score;
        }
    }

    public int AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        UpdateScoreText(currentScore);

        return currentScore;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        return highScore;
    }
}
