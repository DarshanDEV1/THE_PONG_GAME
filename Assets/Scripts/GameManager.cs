using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    [SerializeField] private Ball ball;
    [SerializeField] private TMP_Text player1HealthText;
    [SerializeField] private TMP_Text player2HealthText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject gameOverUI;

    private void OnEnable()
    {
        ball.OnScore += HandleScore;
        player1.OnLifeLost += CheckGameOver;
        player2.OnLifeLost += CheckGameOver;
    }

    private void OnDisable()
    {
        ball.OnScore -= HandleScore;
        player1.OnLifeLost -= CheckGameOver;
        player2.OnLifeLost -= CheckGameOver;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        player1HealthText.text = $"P1 Health: {player1.GetHealth()}%";
        player2HealthText.text = $"P2 Health: {player2.GetHealth()}%";
        livesText.text = $"Lives: P1 - {player1.GetLives()} | P2 - {player2.GetLives()}";
    }

    private void HandleScore(string player)
    {
        Debug.Log("Handle Score Called...");
        if (player == "Player1")
        {
            player1.TakeDamage();
        }
        else
        {
            player2.TakeDamage();
        }

        UpdateUI();
    }

    private void CheckGameOver()
    {
        if (player1.GetLives() <= 0)
        {
            GameOver("Player 2 Wins!");
        }
        else if (player2.GetLives() <= 0)
        {
            GameOver("Player 1 Wins!");
        }
    }

    private void GameOver(string winnerText)
    {
        gameOverUI.SetActive(true);
        gameOverUI.GetComponentInChildren<Text>().text = winnerText;
    }
}
