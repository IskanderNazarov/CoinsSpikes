using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    private const string WIN_TEXT = "Level completed!";
    private const string LOSE_TEXT = "You died...";
    
    [SerializeField] private Camera cam;
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Canvas gameOverUI;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Level level;


    private int scoreCount;
    private bool isGameOver;

    private void Start() {
        player.OnCoinCollected += OnCoinCollected;
        player.OnPlayerDead += OnPlayerDead;
        UpdateScoreText();
        
        print("CoinsCount: " + level.TotalCoinsCount);
    }

    private void OnPlayerDead() {
        EndGame(false);
    }

    private void OnCoinCollected() {
        scoreCount++;
        UpdateScoreText();

        if (scoreCount == level.TotalCoinsCount) {
            EndGame(true);
        }
    }

    private void UpdateScoreText() {
        scoreText.text = scoreCount.ToString();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !isGameOver) {
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            player.AddPosition(mousePos);
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EndGame(bool isWon) {
        isGameOver = true;
        gameOverUI.gameObject.SetActive(true);
        gameOverText.text = isWon ? WIN_TEXT : LOSE_TEXT;
    }
}